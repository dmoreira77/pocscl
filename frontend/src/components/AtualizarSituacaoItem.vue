<template>
	<div align="center">
		<div v-show="carregandoDados">
			Carregando dados...
		</div>
		<div id="viagem" v-show="!carregandoDados">
			<span>Viagem n° {{viagem.codigo}} </span>
			<br/><br/>
			<span>Tipo de viagem: {{viagem.tipoViagem}} </span>
			<br/><br/>
			<span>Placa do veículo: {{viagem.placa}} </span>
			<br/>
			<br/>
		</div>
		<div id="paradas" v-show="!carregandoDados">
			<div v-for="(p, index) in viagem.paradas" :id="'parada-' + index" :key="index" class="parada">
				<button :id="'bloco-parada-' + index" v-on:click="mostrarOcultarItens(index)" class="bloco bloco-parada">
					<span class="bloco-parada-local">{{p.municipio}}/{{p.uf}}</span>
						
					{{p.endereco}}
				</button>
				
				<div :id="'itens-parada-' + index" v-show="p.mostrar_itens" class="itens-parada">
					<div v-for="(item, ii) in p.itens" :id="'item-' + item.codigo" class="item">
						
						<button @click="mostrarOcultarSituacoes(index, ii)" class="bloco bloco-item">
							{{item.codigo}} - {{item.descricao}} <br/>
							{{item.descricaoSituacao}}
						</button>

						<div :id="'situacoes-item-' + item.codigo" v-show="item.mostrar_situacoes" class="situacoes-item">
							<div v-for="sit in situacoesPorTipoOperacao(item.tipoOperacao)" class="situacao">
								<input type="radio" :value="sit.codigo" v-model="item.codigo_situacao_nova" class="radio-situacao" :disabled="atualizandoItem"/>
								<span style="vertical-align: middle;height:100%;display:inline-block;">{{sit.descricao}}</span>
							</div>

							<textarea style="width:100%; height:200px; margin-top: 3%;  display:inline-block; font-size: 200%; border:1px solid #000;" v-model="item.observacao" :disabled="atualizandoItem">
							</textarea>

							<button @click="atualizarSituacao(item)" class="botao-confirmar" :disabled="atualizandoItem">
								<span v-show="!atualizandoItem">Confirmar</span>
								<span v-show="atualizandoItem">Atualizando item</span>
							</button>
						</div>
						
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>

	import axios from 'axios';

	export default{
		name: 'atualizar-situacao-item',
		data(){
			return {
				viagem: {},
				situacoes_item: [],
				atualizandoItem: false,
				carregandoDados: true
			}
		},		
		created(){
			this.carregandoDados = true;
			axios.get("https://localhost:44395/api/acompanhamento-logistica/situacoes-viagem").then(
					response => {
						this.situacoes_item = response.data

						axios.get("https://localhost:44395/api/acompanhamento-logistica/viagem-atual/" + (this.$auth.user()?this.$auth.user().cpf:"")).then(
								response => {
									this.carregandoDados = false;
									
									this.viagem = response.data;

									this.viagem.paradas.forEach(function(p){
										this.$set(p, 'mostrar_itens', false);
										p.itens.forEach(function(item){
											this.$set(item, 'mostrar_situacoes', false);
											this.$set(item, 'situacao_foi_salva', false);
											this.$set(item, 'codigo_situacao_nova', 0);
											this.$set(item, 'observacao', "");
										}, this)
									}, this);
							}).catch(
								e => {
									alert("Não foi possível obter os dados da viagem.");
									this.carregandoDados = false;
								}
							);
				}).catch(
					e => {
						alert("Não foi possível obter os dados necessários ao acompanhamento da viagem.");
						this.carregandoDados = false;
					}
				);

			
		},
		methods:{
			mostrarOcultarItens(ip){
				this.viagem.paradas[ip].mostrar_itens = !this.viagem.paradas[ip].mostrar_itens
			},
			mostrarOcultarSituacoes(ip, ii){
				this.viagem.paradas[ip].itens[ii].mostrar_situacoes = (!this.viagem.paradas[ip].itens[ii].mostrar_situacoes) && (!this.viagem.paradas[ip].itens[ii].situacao_atualizada)
			},
			classeResultado(item){
				var situacoes = this.situacoes_item.filter(
					function(sit){
						return sit.codigo == item.codigoSituacao
					}
				);
					
				if(situacoes.length > 0)
				{
					if(situacoes[0].consideradoSucesso)
					{
						return "bloco-item-sucesso";
					}
					else
					{
						return "bloco-item-fracasso";
					}
				}
				else
				{
					return '';
				}
				
			},
			situacoesPorTipoOperacao(descricaoTipo){
				
				var situacoes = this.situacoes_item.filter(function(sit){
					return (sit.tipoOperacao.toLowerCase() == descricaoTipo.toLowerCase());
				});

				return situacoes;
				
			},

			atualizarSituacao(item){
				var situacaoNova = this.situacoes_item.filter(function(sit){
					return sit.codigo == item.codigo_situacao_nova;
				})[0];

				this.atualizandoItem = true;
				axios.put("https://localhost:44395/api/acompanhamento-logistica/itens/" + item.codigo + "/situacao-atual", {situacao: situacaoNova, observacao: item.observacao}).then(
						response => {
							this.atualizandoItem = false;

							if(response.data.sucesso){
								item.situacao_atualizada = true;
								item.mostrar_situacoes = false;

								item.codigoSituacao = item.codigo_situacao_nova;
								item.descricaoSituacao = situacaoNova.descricao;	
							}
							else{
								alert(response.data.mensagem);
							}
						}
					).catch(
						(error) => {
							this.atualizandoItem = false;
							alert("Não foi possível a comunicação com o serviço de atualização.");
						}
					);
			}
		}
	}
</script>

<style scoped>
	
	*{
		font-family: Verdana,sans-serif;
		font-size: 135%;
	}

	#viagem{
		font-weight:bold;
		background-color:#eee;
		text-align:left;
		font-size:100%;
		padding-top:50px;
		padding-left:20px;
		border-bottom:1px solid #555;
	}

	.parada{
		width: 100%;	
		border-radius: 1rem;
	}

	.bloco{
		border-radius: 1rem;
		border: 1px solid transparent;
		font-size: 100%;
		font-weight: bold;
		cursor:pointer;
		width:100%;
		display:block;
	}

	.bloco-parada{
		margin-top:10px;
		background-color: #007acc;
		color: #ffffff;		
		border-color: #007acc;
		padding-top:50px;
		padding-bottom:50px;
	}

	span.bloco-parada-local{
		display:block;
		font-size:70% !important;
		padding-bottom:10px;
	}

	.itens-parada{
		width: 100%;
		padding-bottom:15px;
	}

	.item{
		width: 100%;
		border-radius: 1rem;		
		margin-top:10px;
	}

	.bloco-item{
		margin-top:10px;
		font-size: 50%;
		padding-top:35px;
		padding-bottom:35px;
		text-align: left;
		background-color: #ccffff;
		color: #000000;		
		border-color: #007acc;
		border-width: 2px;
		padding-left:25px;
	}

	.bloco-item-sucesso{
		border-color: #006622 !important;
		border-width: 5px;
		background-color: #99ffbb !important;
		color: #000;
	}

	.bloco-item-fracasso{
		border-color: #b30000 !important;
		border-width: 3px;
		background-color: #ff9999 !important;
		color: #000;
	}

	span.bloco-item-codigo{
		display:block;
		font-size:100% !important;
		padding-bottom:10px;
		padding-top:10px;
	}

	.situacoes-item{
		font-size:30%;
		font-weight: bold;
		text-align: left;
		padding-left: 15px;
		padding-top:20px;		
	}

	.situacao{
		padding-top:7px;
		width:100%;
		vertical-align:middle;
		margin-left: auto;
		margin-right: auto;
		padding-bottom:20px;
	}

	.radio-situacao{
		width: 40px;
		height: 40px;
		vertical-align:middle;
		fill: #fff;
		border:1px solid #000;
	}

	.botao-confirmar{
		width:50%;
		height: 100px;
		margin-left:45%;
		/*margin-right:5%;*/
		margin-top: 25px;
		margin-bottom: 5%;
		background-color: #99ffcc;
		border-radius: 5rem;
		border: 2px solid transparent;
		border-color:#006600;
		font-weight: bold;
		font-size:150%;
		cursor:pointer;
		padding-top:8px;
		padding-bottom:8px;
		display:inline-block;
		color: #000;
	}

</style>