<template>
  <div id="app" width="100%"  style="height:100%">
    <div class="header" v-if="$auth.check()">
        <div class="menu_topo">
          <div class="menu_topo_links" v-if="!$auth.check('Motorista')" >
              <nav>
                    <ul>
                        <li>
                          <a href="#">
                            <router-link :to="{name: 'default'}">
                              Início
                            </router-link>
                          </a>
                        </li>
                        <li v-if="$auth.check('Gerente de Logística')">
                          <a href="#">Solicitações de transporte</a>
                          <ul>
                                <li>
                                  <router-link :to="{name: 'encaminhar-cd'}">
                                    Encaminhar para Centro de Distribuição
                                  </router-link>
                                </li>
                                <li>Terceirizar para concorrente</li>
                                <li>Monitorar transferidas</li>
                            </ul>
                        </li>
                        <li v-if="$auth.check('Supervisor de Distribuição')">
                          <a href="#">Coletas</a>
                            <ul>
                                <li>
                                  <router-link :to="{name: 'agendar-coleta'}">
                                    Agendar coletas
                                  </router-link>
                                </li>
                                <li>Liberar coletas</li>
                                <li>Encerrar coleta</li>
                            </ul>
                        </li>
                        <li v-if="$auth.check('Motorista')">
                          <a href="#">
                            <router-link :to="{name: 'viagem-atual'}">
                              Viagem
                            </router-link>
                          </a>
                        </li>
                        <li><a href="#" @click="logout()">Sair</a></li>
                    </ul>
                </nav>
          </div>
        </div>

    </div>

    <div style="height:58px" v-if="$auth.check()&&!$auth.check('Motorista')" ></div>


    <div width="100%" style="height:100%">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
  export default{
    name: "app",
    methods: {
      logout: function(){
        this.$auth.logout();
        this.$router.push({ name: "login"})
      }
    }
  } 
</script>

<style>
  html, body{
    height:100%;
  }

  .header .menu_topo {
    /*width:100%;*/
    color:#FFF;
    margin: 0 auto;
  }

  .menu_topo_links {
  /*width:300px;*/
  float:left;
  height:58px;
  margin-left:-25px;
  margin-top:-5px;
  }
  
  .menu_topo_links ul li {
  float:left;
  list-style-type: none;
  padding: 10px 10px 10px 10px;
  font-size:12px;
  }


  body {
    /*background-color: #064384;*/
    margin-left: 0px;
    margin-top: 0px;
    margin-right: 0px;
    margin-bottom: 0px;
    font-family: Arial, Helvetica, sans-serif;
  }

  

  .header {
    position:fixed;
      top:0;
      width:100%;
      background-color: #064384;
    z-index:100;
  }
  
  .sombra {
    position: fixed;
    top:58px; 
    width:100%;
    height:12px;
    background-repeat: repeat-x;
    display:block;
    z-index:99;
    /*background-color: #fff;*/
  }
  
  
  .header .menu_topo {
    width:100%;
    color:#FFF;
    margin: 0 auto;
  }
  
  .menu_topo_links {
    
    float:left;
    height:58px;
    margin-left:-25px;
    margin-top:-5px;
  }
  
  .menu_topo_links ul li {
    float:left;
    list-style-type: none;
    padding: 10px 10px 10px 10px;
    font-size:12px;
  }
  
  
  a:link {
    color: #FFFFFF;
    text-decoration: none;
    font-weight:bold;
  }
  a:visited {
    color: #FFFFFF;
    text-decoration: none;
    font-weight:bold;
  }
  a:hover {
    color: #FFFFFF;
    text-decoration: none;
    font-weight:bold;
  }
  
  a:active {
    color: #FFFFFF;
    text-decoration: none;
    font-weight:bold;
  }


  nav ul ul {
      display: none;
  }
  nav ul li:hover > ul {
      display: block;
    width:350px;
    margin-left:-10px;
  }

  nav {
      margin: 0 auto;
      text-align: center;
  }

  nav ul {
    list-style: none;
    position: relative;
  }

  nav ul:after {
    content: ""; clear: both; display: block;
  }

  nav ul li {
    float: left;
  }
  
  nav ul li:hover {
    background: #2384d3;
  }
  
  nav ul li:hover a {
    color: #fff;
  }
    
  nav ul ul {
    background: #053970; border-radius: 0px; padding: 0;
    position: absolute; top: 100%;
    box-shadow: 3px 3px 5px rgba(0, 0, 0, 0.2);
  }
    
  nav ul ul li {
    cursor: pointer;
    width:330px;
    text-align: left;
    font-weight: 400;
  }
</style>
