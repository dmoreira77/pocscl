using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using poc.SolicitacoesTransporte.DAO;
using Microsoft.AspNetCore.Authorization;
using poc.SolicitacoesTransporte.DTO;
using poc.SolicitacoesTransporte.Modelo;
using poc.Cadastro.Modelo;

namespace poc.SolicitacoesTransporte
{
    
    [ApiController]
    public class SolicitacoesTransporteController : ControllerBase
    {
        [Authorize("Bearer", Roles = "Gerente de Logística")]
        [HttpGet("api/controle-solicitacoes/solicitacoes/nao-encaminhadas-para-cd")]
        public IEnumerable<ItemTransporteDTO> getSolicitacoesRecebidas()
        {
            IEnumerable<ItemTransporteDTO> solicitacoes = (new DAOItemTransporte()).obterItensNaoEncaminhados();
            
            return solicitacoes;
        }

        [HttpPost("api/controle-solicitacoes/encaminhamentos-para-cd")]
        public Object postSolicitacoes([FromBody] EncaminhamentoCdDTO[] encs)
        {
            if((new DAOItemTransporte()).encaminharItensParaCDs(encs))
            {
                return new {sucesso = true, mensagem = "Itens encaminhados com sucesso.", links = new {self = "api/controle-solicitacoes/encaminhamentos-para-cd/{id_item}", detalhesItem = "api/controle-solicitacoes/solicitacoes/{id_item}", listaNaoEncaminhadosAtualizada = "api/controle-solicitacoes/solicitacoes/nao-encaminhadas-para-cd"}};
            }
            else
            {
                return new {sucesso = false, mensagem = "Não foi possível realizar a operação"};
            }
            
        }


        [HttpGet("api/controle-solicitacoes/encaminhamentos-para-cd/{id}")]
        public EncaminhamentoCdDTO getEncaminhamentoParaCd(int id)
        {
            ItemTransporte item = new ItemTransporte()
            {
                Codigo = id,
                CentroDistribuicao = new CentroDistribuicao() { Codigo = 0 }
            };
            
            return new EncaminhamentoCdDTO() { CodigoItem = item.Codigo, CodigoCD = item.CentroDistribuicao.Codigo };
        }
    }
}