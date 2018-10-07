<template>
	<select v-model="solicitacao.cd">
		<option value="0">- Selecione o CD -</option>
		<option v-for="cd in cds" :title="cd.endereco" :value="cd.codigo" >
			{{cd.nome}}
		</option>
	</select>
</template>

<script>
	import axios from 'axios'

	export default{
		name: 'combo-cd',
		props: ['solicitacao', 'cds']
		
		
	}
</script>

<style></style>