import Vue from 'vue'
import App from './App.vue'

import VueRouter from 'vue-router';

Vue.use(VueRouter)

const routes = [{
        path: '/',
        name: 'default',
        component: require('./components/Home.vue').default,
        meta: {auth: true}
    },
    {
        path: '/login',
        name: 'login',
        component: require('./components/Login.vue').default,
        meta: {auth: false}
    },
    {
        path: '/encaminhar-cd',
        name: 'encaminhar-cd',
        component: require('./components/EncaminharCd.vue').default,
        meta: {auth: 'Gerente de Logística'}
    },
    {
        path: '/viagem-atual',
        name: 'viagem-atual',
        component: require('./components/AtualizarSituacaoItem.vue').default,
        meta: {auth: true}
        
    },
    {
        path: '/agendar-coleta',
        name: 'agendar-coleta',
        component: require('./components/DefinirColeta.vue').default,
        meta: {auth: 'Supervisor de Distribuição'}
    },
    {
        path: '/403',
        name: '403',
        component: require('./components/Forbidden.vue').default
    }]

const router = new VueRouter({
	hashbang: true,
    routes: routes
})

Vue.router = router


import axios from 'axios';

Vue.axios = axios;
Vue.axios.defaults.baseURL = 'https://localhost:44395/api/';



Vue.use(require('../node_modules/@websanova/vue-auth/src/index.js'), {
    //auth: require('../node_modules/@websanova/vue-auth/drivers/auth/bearer.js'), 
     auth: {
        request(req, token) {
            /* eslint no-underscore-dangle: ["error", { "allow": ["_setHeaders"] }] */
            this.options.http._setHeaders.call(this, req, { Authorization: `Bearer ${token}` });
        },
        response: function(response) {
            return response.data.token;
        },
    },   
    http: require('../node_modules/@websanova/vue-auth/drivers/http/axios.1.x.js'),
    router: require('../node_modules/@websanova/vue-auth/drivers/router/vue-router.2.x.js'),
    rolesVar: 'perfil',
    loginData: {url: 'acesso/login', method: 'POST', redirect: {name:((this.$auth) && this.$auth.redirect() && (this.$auth.redirect() != '403'))?this.$auth.redirect():'default'}, fetchUser: false},
    //logoutData: {redirect: '/login'},
    fetchData: {enabled: false},
    refreshData: {enabled: false}
});

new Vue({
    el: "#app",
    router: router,
    render: h => h(App)
})


