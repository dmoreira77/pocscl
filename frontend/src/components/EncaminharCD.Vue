<template>
	<div style="height:100%">
		<div id="solicitacoes" class="solicitacoes">
			<div style="text-align:center; width:100%; font-size:120%;font-weight:bold;margin-top:20px" v-show="carregandoSolicitacoes">
				Carregando solicitações...
			</div>
			<div style="text-align:center; width:100%; font-size:120%;font-weight:bold;margin-top:20px" v-show="enviandoEncaminhamento">
				Encaminhando solicitações...
			</div>
			<view-solicitacoes :solicitacoes="solicitacoes" @selecaoAtualizada="atualizarSelecao" v-show="(!carregandoSolicitacoes && !enviandoEncaminhamento)" :cdsExistentes="cds">
			</view-solicitacoes>
		</div>

		<div class="comandos" v-show="!carregandoSolicitacoes">
			<button @click="encaminhar" class="principal" v-show="!enviandoEncaminhamento">
				Encaminhar para CD
			</button>
			<button @click="voltarTela" v-show="!enviandoEncaminhamento">
				Voltar
			</button>
		</div>
	</div>
</template>

<script>
	import viewSolicitacoes from './VisualizarSolicitacoes.vue';
	import axios from 'axios';

	export	default{
		name:'encaminhar-cd',
		data(){
			return {
				solicitacoes: [],
				codigosSelecionados: [],
				carregandoSolicitacoes: true,
				enviandoEncaminhamento: false,
				cds: []
			}
		},
		methods: {
			atualizarSelecao(codigosSelecionados){
				this.codigosSelecionados = codigosSelecionados
			},
			encaminhar(){
				var encaminhamento = [];
				var cods = this.codigosSelecionados;

				if(this.codigosSelecionados.length > 0)
				{
					if(confirm('Deseja realmente encaminhar cada solicitação selecionada ao respectivo Centro de Distribuição indicado?'))
					{
						this.solicitacoes.forEach(
								function(s){
									if(cods.indexOf(s.codigo) > -1)
									{
										encaminhamento.push({
															codigoItem: s.codigo,
															codigoCD: s.cd
															});								
									}
								}
							);				

						if(encaminhamento.length > 0){
							
							this.enviandoEncaminhamento = true;
							axios.request({
								method: "POST",
								url: "https://localhost:44395/api/controle-solicitacoes/encaminhamentos-para-cd",
								data:  encaminhamento
							}).then(
								response => {
									this.enviandoEncaminhamento = false;

									if(response.data.sucesso)
									{
										this.solicitacoes = this.solicitacoes.filter(function(s){return (cods.indexOf(s.codigo) == -1)});

										this.codigosSelecionados = [];
									}
								}
							).catch(
								e => {
							      this.carregandoSolicitacoes = false;
							      alert(e);
							    }
							);
						}
					}
				}
				else
				{
					alert('Nenhuma solicitação foi selecionada');
				}
			},
			voltarTela(){
				this.$router.push({ name: "default"})
			}
		},
		created(){
			this.carregandoSolicitacoes = true;

			axios.get("https://localhost:44395/api/centros-distribuicao")
			.then(response => {
				this.cds = response.data;

				axios.get(`https://localhost:44395/api/controle-solicitacoes/solicitacoes/nao-encaminhadas-para-cd`)
			    .then(response => {
			      	this.solicitacoes = response.data;
			      	this.carregandoSolicitacoes = false;
			    })
			    .catch(e => {
			      alert(e);
			      this.carregandoSolicitacoes = false;
			    });

			}).catch(e => {
		      alert(e);
		      this.carregandoSolicitacoes = false;
		    });

			
		},
		components:{
			viewSolicitacoes
		}
	}
	
</script>

<style scoped>

div.comandos{
	width: 100%;
	padding-top: 50px;
	text-align:center;	
}

div.solicitacoes{
	height: 70% !important;
	display:block;
	overflow: auto;
}

button{
	width: 200px;
	height: 35px;
	border: 1px solid transparent;
    border-radius: 4px;
    cursor:pointer;
    font-weight:bold;
}

button.principal{
	background-color:#337ab7;
	border-color: #2e6da4;
	color: #fff;
}

</style>