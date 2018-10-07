<template>
	<div  style="height:100%">
		<div class="msg-load" v-show="(mensagemCarregamento != '')">
			{{mensagemCarregamento}}
		</div>

		<div id="itens" class="div-com-borda div-com-tabela">
			<table id="tabela-itens">
				<tr>
					<th colspan="7">
						Itens a coletar
					</th>
				</tr>
				<tr>
					<th width="1%"><input type="checkbox" value="1" style="display:inline" v-model="selecionarTudo" @change="selecionarDesmarcarTudo" /></th>
					<th width="20%">Descrição</th>
					<th width="5%">Peso<br/>(kg)</th>
					<th width="5%">Volume<br/>(m³)</th>
					<th width="7%">Data limite<br/>para coleta</th>
					<th width="14%">Janela de<br/>coleta</th>
					<th width="26%">Endereço</th>
				</tr>
				<tr v-for="(item, index) in itensPendentes">
					<td><input :value="index" v-model="indicesItensSelecionados" type="checkbox" style="display:inline" @change="atualizarItensSelecionados" /></td>
					<td>{{item.descricao}}</td>
					<td align="center">{{item.peso}}</td>
					<td align="center">{{item.volume}}</td>
					<td align="center">{{item.dataLimite}}</td>
					<td align="center">{{item.janelaInicio}} - {{item.janelaFim}}</td>
					<td>{{item.endereco}}</td>
				</tr>
			</table>
		</div>
		
		<fieldset id="roteiro">				
			<legend>Roteiro</legend>

			<fieldset style="padding-bottom:2%;padding-top:2%;">
				<legend>Tipo</legend>
				<input type="radio" v-model="tipoRoteiro" value="manual" :disabled="etapas.itensASelecionar || etapas.roteiroDefinido"/> Roteiro Manual
				<input type="radio" v-model="tipoRoteiro" value="auto" :disabled="etapas.itensASelecionar || etapas.roteiroDefinido"/> Roteiro Automático
			</fieldset>

			<div id="div-tabela-roteiro" class="div-com-tabela div-com-borda">
				<table :disabled="etapas.roteiroDefinido">
					<tr>
						<th></th>				
						<th>Endereço</th>
						<th>Janela</th>
					</tr>
					<tr v-for="(p, index) in paradas">
						<td>
							<img class="seta-reordem-cima" :class="(index==0)||(etapas.roteiroDefinido)?'seta-reordem-inativa':''" @click="moverCima(index)" src="../assets/seta-cima.png" v-show="(tipoRoteiro == 'manual')&&(!etapas.roteiroDefinido)" />
							<br/>
							<img class="seta-reordem-baixo" :class="(index==(paradas.length - 1))||(etapas.roteiroDefinido)?'seta-reordem-inativa':''" @click="moverBaixo(index)" src="../assets/seta-baixo.png" v-show="(tipoRoteiro == 'manual')&&(!etapas.roteiroDefinido)" />
						</td>
						<td>
							{{p.endereco}}
						</td>
						<td align="center">
							{{p.janelaInicio}}
							 - 
							{{p.janelaFim}}
						</td>
					</tr>
				</table>
			</div>
			<div id="comandos-roteiro">
				<div>
					<button style="width: 140px" @click="confirmarRoteiro" :disabled="!etapas.tipoRoteiroEsperandoRoteiro">
						Confirmar Roteiro
					</button>

					<button style="width: 140px" @click="redefinirRoteiro" :disabled="!etapas.roteiroDefinido">
						Redefinir Roteiro
					</button>
				</div>				
			</div>
		</fieldset>

		<div id="parametros-e-veiculos" class="div-com-borda">
			<fieldset id="parametros" style="border:0">
				<div>
					Peso total
					<input type="text" style="width:85px" :value="pesoTotal.replace('.', ',') + ((pesoTotal > 0)?' kg':'')" />
				</div>
				
				<div>
					Volume total
					<input type="text" style="width:85px" :value="volumeTotal.replace('.', ',') + ((volumeTotal > 0)?' m³':'')" />
				</div>

				<div>
					Data limite
					<input type="text" style="width:85px" :value="dataLimiteRoteiro"/>
				</div>

				<div>
					Distância
					<input type="text" style="width:85px" :value="distancia.toString().replace('.', ',') + ((distancia != '')?' km':'')" />
				</div>
			</fieldset>

			<div id="veiculos" class="div-com-borda div-com-tabela">				
				<table id="tabela-veiculos">
					<tr>
						<th colspan="6">
							Veículos disponíveis
						</th>
					</tr>
					<tr>
						<th width="20%">Modelo</th>
						<th width="auto">Placa</th>
						<th width="20%">Motorista</th>
						<th width="auto">Volume<br/>suportado</th>
						<th width="auto">Peso<br/>suportado</th> 
						<th >Disponibilidade</th>
					</tr>
					<tr v-for="v in veiculosFiltrados" :class="((coleta.roteiro!=null)?'linha-veiculo ':'') + ((placaVeiculoSelecionado == v.placa)?'linha-veiculo-selecionado':'')" :key="v.placa">
						<td align="center" @click="selecionarVeiculo(v)">
							{{v.modelo}}
						</td>
						<td align="center" @click="selecionarVeiculo(v)">
							{{v.placa}}
						</td>
						<td align="center" @click="selecionarVeiculo(v)">
							{{v.nomeMotorista }}
						</td>
						<td align="center" @click="selecionarVeiculo(v)">
							{{v.volume}} m³
						</td>
						<td align="center" @click="selecionarVeiculo(v)">
							{{v.peso}} kg
						</td>
						<td>
							<table class="tabela-disponibilidade">
								<tr>
									<th @click="selecionarVeiculo(v)">De</th>
									<th @click="selecionarVeiculo(v)">Até</th>
								</tr>
								<tr v-for="d in v.disponibilidade">								
									<td width="50%" @click="selecionarVeiculo(v, d)">
										{{d.dataHoraInicio}}
									</td>
								
									<td width="50%" @click="selecionarVeiculo(v, d)">
										{{(!d.dataHoraFim)?"em diante" : d.dataHoraFim}}
									</td>
								</tr>
							</table>
						</td>
					</tr>						
				</table>
			</div>
		</div>
		
		<div id="conclusao" >
			<fieldset id="partida" v-show="etapas.veiculoSelecionado">
				<legend>Partida</legend>
				<div>
					Data
					<input type="text" v-model="coleta.dataPartida" style="width:85px" :disabled="etapas.aConfirmarColeta"/>
				</div>
				<div>
					Hora
					<input type="text" v-model="coleta.horaPartida" style="width:70px" :disabled="etapas.aConfirmarColeta"/>					
				</div>
				<div>
					<br/>
					<button style="vertical-align:middle" @click="definirPartida" :disabled="etapas.aConfirmarColeta">Definir</button>
				</div>
			</fieldset>

			<fieldset id="estimativa-retorno" v-show="etapas.aConfirmarColeta">
				<legend>Retorno estimado para</legend>
				<div>
					Data
					<input type="text" style="width:85px" :value="coleta.dataRetornoEstimada" />
				</div>
				<div>
					Hora
					<input type="text" style="width:70px" :value="coleta.horaRetornoEstimada"/>
				</div>
			</fieldset>

			<fieldset id="roteiro-final" v-show="etapas.aConfirmarColeta">
				<legend>Roteiro final</legend>
				<div id="div-tabela-roteiro-final" class="div-com-tabela div-com-borda">
					<table>
						<tr>
							<th>Endereço</th>
							<th width="auto" align="center">Janela</th>
							<th width="auto" align="center">Chegada<br/>estimada</th>
						</tr>
						<tr v-for="(p, index) in paradas">
							<td>
								{{p.endereco}}
							</td>
							<td align="center">
								{{p.janelaInicio}}
								 - 
								{{p.janelaFim}}
							</td>
							<td align="center" :class="p.horarioChegada?(((p.horarioChegada.split(' ')[1] < p.janelaInicio) || (p.horarioChegada.split(' ')[1] > p.janelaFinal))?'janela-fora':''):''">
								{{p.horarioChegada?p.horarioChegada:''}}
							</td>
						</tr>
					</table>
				</div>
			</fieldset>
			<div id="concluir" v-show="etapas.aConfirmarColeta">
				<button class="principal" @click="confirmarColeta">Confirmar agendamento de coleta</button>
				<button @click="redefinirAgendamento">Redefinir <br/>agendamento</button>
			</div>			
		</div>
	</div>
</template>


<script>
	function converterParaDataHora(data, hora)
	{
		if(data.indexOf(" ") > -1)
		{
			hora = data.split(" ")[1];
			data = data.split(" ")[0];
		}

		if(!hora)
		{
			hora = "00:00:00";
		}
		
		var arrayData = data.split("/");
		var dia = arrayData[0];
		var mes = arrayData[1];
		var ano = arrayData[2];
		
		return Date.parse(mes + "/" + dia + "/" + ano + " " + hora);
	}

	function padZero(str){
		return ("0" + str.toString()).slice(-2);
	}

	function converterParaDataHoraString(dataHora){
		
		var dt = new Date(dataHora);
		return padZero(dt.getDate()) + "/" + padZero(dt.getMonth() + 1) + "/" + dt.getFullYear() + " " + padZero(dt.getHours()) + ":" + padZero(dt.getMinutes()) + ":" + padZero(dt.getSeconds());
	}

	import axios from 'axios';

	export default{
		name: "definir-coleta",
		data(){
			return {
				itensPendentes: [],
				veiculosDisponiveis: [],
				veiculosFiltrados:[],
				coleta: {
					itens: [],
					roteiro: null,
					placa: "",
					dataPartida: "",
					horaPartida: "",
					dataRetornoEstimada: "",
					horaRetornoEstimada: ""
				},
				veiculoSelecionado: {},
				indicesItensSelecionados: [],
				selecionarTudoMarcado: "0",
				paradas: [],
				tipoRoteiro: "",
				selecionarTudo: [],
				mensagemCarregamento: ""
			}
		},
		computed: {
			pesoTotal: function(){
				var peso = 0;
				this.coleta.itens.forEach(
					function(item){
						peso += item.peso;
					}, this
				);
				return peso.toFixed(3);
			},
			volumeTotal: function(){
				var volume = 0;
				this.coleta.itens.forEach(
					function(item){
						volume += item.volume;
					}, this
				);
				return volume.toFixed(3);
			},
			dataLimiteItensPendentes: function(){
				var dataLim = null;
				this.itensPendentes.forEach(function(item){
					if(dataLim){
						if(converterParaDataHora(dataLim) < converterParaDataHora(item.dataLimite)){
							dataLim = item.dataLimite;
						}
					}
					else{
						dataLim = item.dataLimite;
					}
				});
				return dataLim;
			},
			dataLimiteRoteiro: function(){
				var dataLim = null;
				this.coleta.itens.forEach(function(item){
					if(dataLim){
						if(converterParaDataHora(dataLim) > converterParaDataHora(item.dataLimite)){
							dataLim = item.dataLimite;
						}
					}
					else{
						dataLim = item.dataLimite;
					}
				});

				if(!dataLim) dataLim = this.dataLimiteItensPendentes;

				return dataLim;
			},
			distancia: function(){
				if(this.coleta.roteiro != null){
					return this.coleta.roteiro.distancia.toFixed(2);	
				}
				else{
					return "";
				}
				 
			},
			placaVeiculoSelecionado: function(){
				return this.veiculoSelecionado?this.veiculoSelecionado.placa:"";
			},
			etapas: function(){
				return {
					itensASelecionar: (this.coleta.itens.length == 0),
					paradasEsperandoTipoRoteiro: (this.paradas.length > 0) && (this.coleta.roteiro == null),
					tipoRoteiroEsperandoRoteiro: ((this.tipoRoteiro != "") && (this.coleta.roteiro == null)),
					roteiroDefinido: (this.coleta.roteiro != null),
					roteiroDefinidoEsperandoVeiculo: ((this.coleta.roteiro != null) && (this.veiculoSelecionado == null)),
					veiculoSelecionado: ((this.coleta.roteiro != null) && (this.veiculoSelecionado != null)),
					veiculoSelecionadoEsperandoHorario: ((this.coleta.roteiro != null) && (this.veiculoSelecionado != null) && (this.coleta.dataRetornoEstimada == "")),					
					horariosDefinidos: ((this.coleta.roteiro != null) && (this.veiculoSelecionado != null) && (this.coleta.dataRetornoEstimada != "")),
					aConfirmarColeta: ((this.coleta.roteiro != null) && (this.veiculoSelecionado != null) && (this.coleta.dataRetornoEstimada != "")) // && (this.disponibilidadeVeiculoCompativel())
				}
			}
		},
		methods: {
			selecionarDesmarcarTudo(){
				var i = 0;
				this.indicesItensSelecionados = [];

				if(this.selecionarTudo == 1){
					this.itensPendentes.forEach(function(item){
						this.indicesItensSelecionados.push(i);
						i++;
					}, this);
				}

				this.atualizarItensSelecionados();
			},
			atualizarItensSelecionados(){
				var lista = [];
				this.indicesItensSelecionados.forEach(
					function(i){
						lista.push(this.itensPendentes[i]);
					}, this);

				this.coleta.itens = lista;
				this.atualizarParadas();
				this.preFiltrarVeiculos();
			},
			atualizarParadas(){
				var vetor_paradas = [];
				this.coleta.itens.forEach(function(item){
					if(
						vetor_paradas.filter(function(parada){
							return parada.endereco == item.endereco;
						}).length == 0
					){
						vetor_paradas.push({endereco: item.endereco, municipio: item.municipio, uf: item.uf, codigoMunicipio: item.codigoMunicipio, codigoUf: item.codigoUf, cep: item.cep, janelaInicio: item.janelaInicio, janelaFim: item.janelaFim});
					}
				}, this);

				
				var vetor_antes = this.paradas;

				this.paradas = [];

				vetor_antes.forEach(function(pa){
					if(vetor_paradas.filter(function(pp){
						return (pp.endereco == pa.endereco);
					}).length > 0){
						this.paradas.push(pa);
					}
					else{
						this.redefinirRoteiro();
					}
				}, this);

				vetor_paradas.forEach(function(pp){
					if(vetor_antes.filter(function(pa){
						return (pp.endereco == pa.endereco);
					}).length == 0){
						this.paradas.push(pp);
						this.redefinirRoteiro();
					}
				}, this);
			},
			redefinirRoteiro(){
				this.coleta.roteiro = null;
				this.tipoRoteiro = "";
				this.coleta.dataRetornoEstimada = "";
				this.coleta.horaRetornoEstimada = "";
			},
			moverCima(indice){
				if((indice > 0) && (!this.etapas.roteiroDefinido)){
					var paradas = this.paradas;
					paradas.splice(indice - 1, 2, paradas[indice], paradas[indice - 1]);
				}
			},
			moverBaixo(indice){
				if((indice < (this.paradas.length - 1)) && (!this.etapas.roteiroDefinido)){
					var paradas = this.paradas;
					paradas.splice(indice, 2, paradas[indice + 1], paradas[indice]);
				}
			},
			confirmarRoteiro(){
				//Mock - Chamada à API para calcular o Roteiro e distância correspondente
				this.mensagemCarregamento = "Calculando roteiro";
				axios.request({
				  method: 'post',
				  url: "https://localhost:44395/api/controle-coletas/calculos/roteiro/" + this.tipoRoteiro,
				  data: this.paradas
				}).then(
						response => {
							this.mensagemCarregamento = "";
							this.coleta.roteiro = response.data;
							this.paradas = this.coleta.roteiro.paradas;
							this.filtrarVeiculos();
						}
					).catch(e => {
						this.mensagemCarregamento = "";
				      	alert(e);
				    });
			},
			preFiltrarVeiculos(){
				this.veiculosFiltrados = [];
				this.veiculoSelecionado = null;
				this.veiculosDisponiveis.forEach(function(v){

					let temDataDisponivel = (converterParaDataHora(v.disponibilidade[0].dataHoraInicio, "00:00:00") <= converterParaDataHora(this.dataLimiteRoteiro, "23:59:59"));

					if((v.peso >= this.pesoTotal) && (v.volume >= this.volumeTotal) && temDataDisponivel){
						this.veiculosFiltrados.push(v);
					}

				}, this);
			},
			filtrarVeiculos(){
				this.veiculoSelecionado = null;

				this.mensagemCarregamento = "Filtrando veículos";
				axios.request({
				  method: 'get',
				  url: "https://localhost:44395/api/integracao-interna/gestao-frotas/veiculos-disponiveis",
				  params: {
				  			datalimite: this.dataLimiteRoteiro,
				  			peso: this.pesoTotal,
				  			volume: this.volumeTotal,
				  			distancia: this.coleta.roteiro.distancia,
				  			qtdParadas: this.coleta.roteiro.paradas.length
				  }
				}).then(
						response => {
							this.mensagemCarregamento = "";
							this.veiculosFiltrados = response.data;
						}
					).catch(e => {
						this.mensagemCarregamento = "";
				      	alert(e);
				    });
			},
			selecionarVeiculo(v, intervaloDisponivel)
			{
				if(this.coleta.roteiro == null){
					alert("Antes de selecionar o veículo, é necessário definir o roteiro.");
					return;
				}

				this.veiculoSelecionado = v;

				this.coleta.dataPartida = "";
				this.coleta.horaPartida = "";

				this.coleta.dataRetornoEstimada = "";
				this.coleta.horaRetornoEstimada = "";

				if(intervaloDisponivel){
					this.coleta.dataPartida = intervaloDisponivel.dataHoraInicio.split(" ")[0];
					this.coleta.horaPartida = intervaloDisponivel.dataHoraInicio.split(" ")[1];
				}
			},
			definirPartida(){
				if((this.coleta.dataPartida == "")||(this.coleta.horaPartida == "")){
					alert("É necessário informar a data de horário de partida.");
					return;
				}

				if(this.veiculoSelecionado == null)
				{
					alert("É necessário escolher um dos veículos disponíveis para prosseguir.");
					return;
				}

				if((this.dataPartida == "") || (this.horaPartida == "")){
					alert("É necessário informar uma data e uma hora válidas para a partida.");
					return;	
				}

				var payload = {roteiro: this.coleta.roteiro, velocidade: this.veiculoSelecionado.velocidadeMedia, dataHoraPartida: this.coleta.dataPartida + " " + this.coleta.horaPartida};

				this.mensagemCarregamento = "Completando roteiro";
				axios.request(
					{
						method: "post",
						url: "https://localhost:44395/api/controle-coletas/calculos/roteiro-completo",
						data: payload
					}
				).then(
					response => {
						this.mensagemCarregamento = "";
						var strRetorno = response.data.dataHoraRetorno;
						this.coleta.dataRetornoEstimada = strRetorno.toString().split(" ")[0];
						this.coleta.horaRetornoEstimada = strRetorno.toString().split(" ")[1];

						this.coleta.roteiro = response.data;
						this.paradas = response.data.paradas;
						
						if(!this.disponibilidadeVeiculoCompativel()){
							alert("Os horários são incompatíveis com a disponibilidade do veículo escolhido.");
						}

						if(!this.dataLimiteRespeitada()){
							alert("A data limite de " + this.dataLimiteRoteiro + " não está sendo respeitada.");	
						}
					}
				).catch(
					e => {
						this.mensagemCarregamento = "";
					}
				);
			},
			disponibilidadeVeiculoCompativel(){
				var dataHoraPartida = converterParaDataHora(this.coleta.dataPartida, this.coleta.horaPartida);

				var retorno = converterParaDataHora(this.coleta.dataRetornoEstimada + " " + this.coleta.horaRetornoEstimada);

				return (this.veiculoSelecionado.disponibilidade.filter(function(intervalo){
					var inicioOk = (dataHoraPartida >= converterParaDataHora(intervalo.dataHoraInicio));

					var fimOk = (!intervalo.dataHoraFim) || (retorno <= converterParaDataHora(intervalo.dataHoraFim));

					return (inicioOk && fimOk);
				}).length > 0);
			},
			dataLimiteRespeitada(){
				return (converterParaDataHora(this.coleta.dataRetornoEstimada) <= converterParaDataHora(this.dataLimiteRoteiro));
			},			
			redefinirAgendamento(){
				this.coleta.dataRetornoEstimada = "";
				this.coleta.horaRetornoEstimada = "";
			},
			confirmarColeta(){
				this.coleta.placa = this.veiculoSelecionado.placa;

				this.mensagemCarregamento = "Confirmando coleta";
				axios.request({
				  method: 'post',
				  url: "https://localhost:44395/api/controle-coletas/coletas",
				  data: this.coleta
				}).then(
					response => {
						this.mensagemCarregamento = "";
						alert(response.data.mensagem);
						if(response.data.sucesso)
						{
							this.voltar();
						}
					}
				).catch(
					e => {
						this.mensagemCarregamento = "";
					}
				);
			},
			voltar(){
				this.$router.push({ name: "default"});
			}
		},
		created(){
			this.mensagemCarregamento = "Carregando dados dos itens";
			axios.get("https://localhost:44395/api/controle-coletas/itens-pendentes").then(
					response => {
						this.itensPendentes = response.data;

						if(this.itensPendentes.length > 0)
						{
							this.mensagemCarregamento = "Filtrando veículos";
							axios.get("https://localhost:44395/api/integracao-interna/gestao-frotas/veiculos-disponiveis?datalimite=" + this.dataLimiteItensPendentes).then(
								response => {
									this.mensagemCarregamento = "";
									this.veiculosDisponiveis = response.data;
									this.preFiltrarVeiculos();
								}
							).catch(
								e => {
									this.mensagemCarregamento = "";
								}
							);
						}
						else{
							this.mensagemCarregamento = "";
							alert("No momento, não há itens a coletar.");
						}
						
					}
				).catch(
					e => {
						this.mensagemCarregamento = "";
					}
				);
		}
	}
</script>


<style>
	.div-com-borda{
		border: 1px solid #ccc;
	}

	.div-com-tabela{
		overflow: auto;
	}

	button{
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

	fieldset{
		padding:0;
		font-size: 80%;
		border: 1px solid #ccc;
	}

	fieldset legend{
		display: inline;
		padding:0;
		margin-left:5%;
	}

	table{
		width:100%;		
		vertical-align:top;
		border-collapse: collapse;
	}

	table td, table th, table tr{
		border: 1px solid #ccc;
		font-size: 90%;
	}

	table tr{
		vertical-align:middle;	
	}

	table th{
		background-color: #eee;
	}

	.tabela-disponibilidade tr, .tabela-disponibilidade td, .tabela-disponibilidade th{
		font-size: 100%;
		text-align:center;
	}

	input[type="text"]{
		display:block;
		width:60px;
	}

	

	.linha-veiculo-selecionado td, .linha-veiculo-selecionado th{
		background-color: #cce6ff;
	}

	.linha-veiculo{
		cursor: pointer;
	}

	#itens{
		width:72%;
		height:40%;
		float:left;
		display:block;
		vertical-align:top;
	}

	#roteiro{
		width:27%;
		height: 40%;
		margin-left: 0.5%;
		display:block;		
		float:left;
	}

		#roteiro div, #roteiro fieldset{
			width:95%;
			margin-left:auto;
			margin-right:auto;
		}

		#div-tabela-roteiro{
			display:block;			
			width:100% !important;
			height: 50%;
			float:left;
			margin-top: 3%;
		}

		#comandos-roteiro{
			padding-top: 1%;
			padding-left: 1%;
			vertical-align:middle;
			float:left;
			text-align:center;
			width:100%;
		}

			#comandos-roteiro button{
				margin-left: auto;
				margin-right: auto;
			}

		.seta-reordem-cima, .seta-reordem-baixo{
			display:inline-block;
			
			cursor:pointer;
		}


		.seta-reordem-inativa{
			cursor: default !important;
		}


	#parametros-e-veiculos{
		margin-top:1%;
		width:60%;
		height:48%;
		display:block;
		float:left;		
	}


		#parametros{
				width:100%;
				height: 60px;
				display:block;
				float:left;
			}

				#parametros div{			
					display:block;
					margin-left:3%;
					margin-top:1%;
					float:left;
				}

		#veiculos{
			width:100%;
			height:calc(100% - 60px);
			display:block;
			float:left;		
		}

	#conclusao{
		margin-top:0.5%;
		margin-left:0.5%;
		width:39%;
		height:40%;
		display:block;
		float:left;
		
	}

		#partida{
			margin-left:1%;
			display:block;			
			float:left;	
			width: 60%;
			height:25%;
		}

			#partida div{
				display:block;
				margin-left:5%;				
				float: left;
			}

			#partida button{
				width:100px;
				min-width:100px;
			}

		#estimativa-retorno{
			margin-left:1%;
			width: 36%;
			display:block;
			height:25%;
		}

			#estimativa-retorno div{
				display:inline-block;
				margin-left:5%;				
				float: left;
			}

		#roteiro-final{
			margin-left:1%;
			margin-top:1%;
			width: 97%;
			height: 65%;
		}

			#div-tabela-roteiro-final{
				height: 80%;
				width: 95%;
				margin-left: auto;
				margin-right: auto;
			}

		#concluir{
			padding-left: 1%;
			padding-top: 4%;
			width: 100%;
			display:block;
			
			text-align:center;
			vertical-align: middle;
		}

			#concluir button{
				width:200px;
				height:40px;
			}

		.janela-fora{
			color: red;
		}

		.msg-load{
			position: absolute;
			width: 20%;
			height: 20%;
			left: 40%;
			top: 120px;
			background-color: #EEE;
			text-align: center;
			font-size: 20px;
			font-weight: bold;
			padding-top: 5%;
			border: 2px solid #555;
			color: #053970;
		}

</style>