<template>
	<div>
		<div style="margin-left:20px;margin-top:15px" v-show="$auth.ready() && (mensagemCarregamento=='')">
			<strong>Usuário:</strong> {{$auth.user().nome}}
			<br/><br/>
			<strong>Perfil:</strong> {{$auth.user().perfil}}
			<div v-if="$auth.check('Supervisor de Distribuição')">
				<br/>
				<strong>Centro de Distribuição: </strong> {{centroDistribuicaoSupervisor()}}
			</div>
		</div>

		<div style="text-align:center; width:100%; font-size:120%;font-weight:bold;margin-top:20px" v-show="mensagemCarregamento!=''">
			{{mensagemCarregamento}}
		</div>
	</div>
</template>

<script>
	import axios from 'axios';


	export default {
	  name: 'home',
	  data(){
	  	return {
	  		cd: "",
	  		mensagemCarregamento: ""
	  	}
	  },
	  methods:{
	  	centroDistribuicaoSupervisor(){
	  		
	  		if(this.$auth.user().perfil == "Supervisor de Distribuição")
	  		{
		  		if(this.cd == "")
		  		{
		  			this.mensagemCarregamento = "Obtendo dados de perfil do usuário";
		  			axios.get("https://localhost:44395/api/centros-distribuicao/supervisores-distribuicao/" + this.$auth.user().cpf).then(
									response => {
										this.cd = response.data.centroDistribuicao;
										this.mensagemCarregamento = "";
									}
								).catch(
									e => {
										this.mensagemCarregamento = "";
									}
								);
		  		}

	  		}
	  		return this.cd.nome;
	  	}
	  }
	}
</script>

<style>
	router-link{
		display:block;
	}
</style>