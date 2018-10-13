using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using poc.AcompanhamentoLogistica.DAO;
using poc.AcompanhamentoLogistica.DTO;
using poc.Cadastro;
using poc.Cadastro.DAO;
using poc.Cadastro.Modelo;
using poc.SolicitacoesTransporte;
using poc.SolicitacoesTransporte.DAO;
using poc.SolicitacoesTransporte.Modelo;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;

namespace poc.AcompanhamentoLogistica
{
    [ApiController]
    public class AcompanhamentoLogisticaController : ControllerBase
    {
        [Authorize("Bearer")]
        [HttpGet("api/acompanhamento-logistica/situacoes-viagem")]
        public IEnumerable<SituacaoItem> getSituacoesViagem()
        {
            IEnumerable<SituacaoItem> sits = (new DAOSituacaoItem()).obterTodasSituacoesViagem();

            return sits;
        }

        [Authorize("Bearer")]
        [HttpGet("api/acompanhamento-logistica/viagem-atual/{cpfUrl}")]
        public ViagemDTO getViagemAtual([FromRoute] String cpfUrl, [FromServices] IHttpContextAccessor acessor)
        {
            String cpfToken = acessor.HttpContext.User.Identity.Name;
            String perfilToken = acessor.HttpContext.User.Claims.FirstOrDefault(a => a.Type.Contains("role"))?.Value;
            
            if ((cpfToken.Equals(cpfUrl)) || ((perfilToken != null) && (perfilToken.Equals("Supervisor de Logística"))))
            {
                ViagemDTO v = (new DAOViagem()).obterViagemAtual(cpfUrl);
                return v;
            }
            else
            {
                return null;
            }
        }



        [Authorize("Bearer")]
        [HttpPut("api/acompanhamento-logistica/itens/{id}/situacao-atual")]
        public object putSituacao(int id, [FromBody] dynamic corpo)
        {
            int codigoSituacaoNova = (int) corpo.situacao.codigo;
            String observacao = (String) corpo.observacao;
            
            if((new DAOItemTransporte()).atualizarSituacao(id, codigoSituacaoNova, observacao))
            {
                return new {
                            sucesso = true,
                            mensagem = "",
                            links = new {
                                self = "api/acompanhamento-logistica/itens/" + id + "/situacao-atual",
                                historicoItem = "api/acompanhamento-logistica/itens/" + id + "/historico"}
                };
            }
            else
            {
                return new
                {
                    sucesso = false,
                    mensagem = "Não foi possível atualizar a situação do item."
                };
            }

            
        }

        [Authorize("Bearer")]
        [HttpGet("api/acompanhamento-logistica/itens/{id}/situacao-atual")]
        public SituacaoItem getSituacao(int id)
        {
            ItemTransporte item = new ItemTransporte() { Codigo = id, SituacaoAtual = new SituacaoItem() { Codigo = 0 } };

            SituacaoItem situacaoAtual = item.SituacaoAtual;

            return situacaoAtual;
        }
    }
}