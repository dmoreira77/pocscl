<template>
		<div>
			<center>
			<div style="font-size:120%;font-weight:bold;margin-top:20px" v-show="autenticando">
	      			Aguardando a autenticação
	      	</div>
	      	</center>

			<div class="centralizadora-div">
				<div class="login-div" v-show="!autenticando">
			        <label>CPF</label>			     
			        <input class="login-input" type="text" v-model="user.cpf"/>
			        <br/>
			        
			        <label>Senha</label>
			        <input class="login-input" type="password"  v-model="user.senha"/>
			        <br/>

			        <button @click="logar()">
			        	Entrar
			        </button>
		    	</div>	          	
      		</div>
      		</center>
        </div>
        
</template>

<script>
	export default {
	  name: 'login',
	  data () {
	    return {
	      user: {
	        cpf: null,
	        senha: null
	      },
	      autenticando: false
	    }
	  },
	  methods: {
	    logar: function(){
	    	var redirect = this.$auth.redirect();

	    	this.autenticando = true;
            this.$auth.login({
                data: this.user,
                redirect: {name: (!!redirect?redirect.from.name:'default')}
            })
            .then(
            	(response) => {
                		this.autenticando = false;

                		if(response.data.sucesso){
                			this.$auth.user(response.data.usuario);
                		}
                		else{
                			alert(response.data.mensagem);
                		}
            		}, 
            	(res) => {
	                	this.autenticando = false;
	                	alert("O sistema não responde.");
            		}
            ).catch(e => {
            	this.autenticando = false
            });
	    }
	  }
	}
</script>

<style scoped>
	
	

body {
	background-color: #053970 !important;
	height:100%;
	width:100%;
}

.pagina_total {
	width:100%;
	height:100%;
}

.centralizadora-div {
	margin: 0 auto;
	width: 30%;
}

.login-div {
	margin-top:20%;
	width: 40%;
	margin-left:20%;
	border-radius: 15px;
	
	background-color: #EEE;
	padding:13px 30px 30px 30px;
	/*width:350px;*/
	height:120px;
	font-family:Verdana, Geneva, sans-serif;
	font-size:12px;
	color:#000;
	
   box-shadow: 3px 3px 5px rgba(0,0,0,0.2);
   
}

input.login-input {
	width: 90%;
	padding: 3px 3px 3px 3px;
	border-radius: 5px;
	display:block !important;

    -moz-box-shadow: 0 0 0;
    -webkit-box-shadow: 0 0 0;
 
    border: 1px solid rgb(169, 169, 169);
}

button	{
		width:90%;
		font-size: 16.25px;
		border-radius: 5px;
		border: 1px solid rgba(0,0,0,0.2);
		color: #fff;
		background-color: #053970;
		box-shadow: 0 1px 2px rgba(0,0,0,0.05);
		cursor: pointer;
}

button:hover {
		background-color: #2384d3;
		color:#fff;
}

@media screen and (min-width: 424px) and (max-width: 1000px) {
	.centralizadora-div {
		margin: 0 auto;
		width: 70%;
	}
}		

@media screen and (max-width: 424px) {
	.centralizadora-div {
		margin: 0 0 0 0;
		width: 100%;
		height: 100%;
	}

	.login-div {
		margin-top:0;
		width: calc(100% - 10px);
		margin-left:0;
		padding:30px 0 30px 10px;
		height:100%;
		font-family:Verdana, Geneva, sans-serif;
		font-size:130%;
		box-shadow: 3px 3px 5px rgba(0,0,0,0.2);
	}

	input.login-input {
		width: calc(100% - 20px);
		height: 40px;
		padding: 0 0 0 0;
		border-radius: 5px;
		display:block !important;
		font-size:130%;

	    -moz-box-shadow: 0 0 0;
	    -webkit-box-shadow: 0 0 0;
	 
	    border: 1px solid rgb(169, 169, 169);
	}

	button	{
		width:60%;
		height:40px;
		margin-left: 20%;
		font-size: 16.25px;
		border-radius: 5px;
		border: 1px solid rgba(0,0,0,0.2);
		color: #fff;
		background-color: #053970;
		box-shadow: 0 1px 2px rgba(0,0,0,0.05);
		cursor: pointer;
	}

}


</style>