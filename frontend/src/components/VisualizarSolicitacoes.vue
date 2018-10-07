<template>
	<div>
		<div class="topo">
			Solicitações de transporte
		</div>

		<div class="conteudo">
			<table class="tabela-solicitacoes">
				<tr>
					<th><input type="checkbox" value="1" style="display:inline" v-model="selecionarTudo" @change="selecionarDesmarcarTudo" /></th>
					<th>Item</th>
					<th>Quantidade</th>
					<th>Cuidados<br/>especiais</th>
					<th>Endereço<br/>coleta</th>
					<th>Data limite<br/>coleta</th>
					<th>Janela<br/>coleta</th>
					<th>Cidade<br/>entrega</th>
					<th>Data limite<br/>entrega</th>
					<th>Centro de<br/>Distribuição</th>
				</tr>
				<tr v-for="(s, index) in solicitacoes">
					<td><input type="checkbox" :value="s.codigo" v-model="codigosSelecionados" @change="informarSelecao"/></td>
					<td :title="dicaItem(s)">{{s.descricao}}</td>
					<td>{{s.quantidade}}</th>
					<td>{{s.cuidados}}</td>
					<td>{{s.enderecoColeta}}</td>
					<td>{{s.dataLimiteColeta?s.dataLimiteColeta.split(" ")[0]:''}}</td>
					<td>{{s.janelaColeta}}</td>
					<td :title="dicaCidadeEntrega(s)">{{s.municipioEntrega}} / {{s.ufEntrega}}</td>
					<td>{{s.dataLimiteEntrega?s.dataLimiteEntrega.split(" ")[0]:''}}</td>
					<td><combo-cd :solicitacao="s" :cds="cdsExistentes"/></td>					
				</tr>
			</table>
		</div>
	</div>
</template>

<script>

	
	
	import ComboCd from './ComboCD.vue'

	export default{
		name: 'view-solicitacoes',
		data(){
			return {
				codigosSelecionados: [],
				selecionarTudo: [],
			}
		},
		props:['solicitacoes', 'cdsExistentes'],
		methods:{
			informarSelecao(){
				
				this.$emit('selecaoAtualizada', this.codigosSelecionados)
			},
			dicaItem(sol)
			{
				return "Volume: " + sol.volume + "m³ / Peso: " + sol.peso + "kg"
			},
			dicaCidadeEntrega(sol)
			{
				return sol.enderecoEntrega + " - " + sol.municipioEntrega + "/" + sol.ufEntrega;
			},
			selecionarDesmarcarTudo(){
				this.codigosSelecionados = [];

				if(this.selecionarTudo == 1){
					this.solicitacoes.forEach(function(s){
						this.codigosSelecionados.push(s.codigo);
					}, this);
				};

				this.informarSelecao();
			}
		},
		components:{
			ComboCd
		}
	}
</script>

<style scoped>
.topo{
	text-align:center;
	width: 100%;
	font-size: 20px;
	font-weight: 700;
	margin-top: 10px;
}

.conteudo{
	width: 100%;
}

.tabela-solicitacoes{
	width:100%;
	border-collapse: collapse;
	margin-top: 10px;
}

.tabela-solicitacoes tr td, .tabela-solicitacoes tr th{
	font-size: 11px !important;
	text-align: center;		
}

.tabela-solicitacoes th{
	border-top: 1px solid #888;
	border-bottom: 1px solid #888;
}

.tabela-solicitacoes td{	
	
}

</style>
