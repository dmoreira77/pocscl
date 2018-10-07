using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using poc.AcompanhamentoLogistica;
using poc.AcompanhamentoLogistica.DAO;
using poc.AcompanhamentoLogistica.Modelo;
using poc.Cadastro;
using poc.Cadastro.DAO;
using poc.Cadastro.Modelo;
using poc.Coletas.DTO;
using poc.Coletas.Modelo;
using poc.IntegracaoExterna;
using poc.IntegracaoExterna.Rotas;
using poc.SolicitacoesTransporte;
using poc.SolicitacoesTransporte.DAO;

namespace poc.Coletas
{
    [ApiController]
    public class ColetasController : ControllerBase
    {
        [Authorize("Bearer", Roles = "Supervisor de Distribuição")]
        [HttpGet("api/controle-coletas/itens-pendentes")]
        public IEnumerable<ItemColetaDTO> getItensPendentes([FromServices] IHttpContextAccessor accessor)
        {
            String cpfLogado = accessor.HttpContext.User.Identity.Name;
            SupervisorLogistica sup = (new DAOSupervisorLogistica()).obter(cpfLogado);

            if(sup == null)
            {
                return null;
            }
            else
            {
                return (new DAOItemTransporte()).obterItensPendentesColeta(sup.CentroDistribuicao.Codigo);
            }
        }

        [Authorize("Bearer", Roles = "Supervisor de Distribuição")]
        [HttpPost("api/controle-coletas/calculos/roteiro/{tipo}")]
        public async Task<RoteiroColetaDTO> getRoteiroCalculado(String tipo, [FromBody] ParadaRoteiroColetaDTO[] paradas, [FromServices] IHttpContextAccessor accessor)
        {
            String cpfLogado = accessor.HttpContext.User.Identity.Name;

            SupervisorLogistica sup = (new DAOSupervisorLogistica()).obter(cpfLogado);

            RoteiroColetaDTO roteiro = null;

            if (sup != null)
            {
                if (tipo == "auto")
                {
                    roteiro = FacadeRotas.gerarRoteiroAutomatico(paradas, sup.CentroDistribuicao);
                }
                else
                {
                    roteiro = FacadeRotas.gerarRoteiroManual(paradas, sup.CentroDistribuicao);
                }
            }

            return roteiro;
        }

        [Authorize("Bearer", Roles = "Supervisor de Distribuição")]
        [HttpPost("api/controle-coletas/calculos/roteiro-completo")]
        public RoteiroColetaDTO getRoteiroCompleto([FromBody] ParametrosCalculoRoteiroCompletoDTO param)
        {
            RoteiroColetaDTO roteiro = param.Roteiro;
            ParadaRoteiroColetaDTO[] paradas = roteiro.Paradas;
            DateTime dataHoraPartida = DateTime.Parse(param.DataHoraPartida, new CultureInfo("pt-BR"));
            float velocidade = param.Velocidade;
            float distanciaTotal = roteiro.Distancia;
            float distanciaAcc = 0;
            int qtdParadas = paradas.Length;

            DateTime dataHoraAnterior = dataHoraPartida;
            foreach (var parada in paradas)
            {
                float distanciaTrecho = parada.Distancia;

                //Calculando o tempo de deslocamento no trecho, adicionando 10% de margem de segurança
                DateTime dataHoraChegada = dataHoraAnterior.AddMilliseconds((distanciaTrecho / velocidade) * (60 * 60 * 1000));
                parada.HorarioChegada = dataHoraChegada.ToString("dd/MM/yyyy HH:mm:ss");

                dataHoraAnterior = dataHoraChegada.AddMinutes(30); //Contabilizando 30 minutos para carregamento do veículo
                distanciaAcc += distanciaTrecho;
            }

            roteiro.DataHoraRetorno = dataHoraAnterior.AddMilliseconds(((distanciaTotal - distanciaAcc)/velocidade) * (60 * 60 * 1000)).ToString("dd/MM/yyyy HH:mm:ss");

            return roteiro;
        }

        [Authorize("Bearer", Roles = "Supervisor de Distribuição")]
        [HttpPost("api/controle-coletas/coletas")]
        public object postColeta([FromBody] ColetaDTO coleta)
        {
            Viagem viagemColeta = ConversorColetaViagem.converterParaViagem(coleta);

            if((new DAOViagem()).incluir(viagemColeta))
            {
                return new {
                        sucesso = true,
                        mensagem = "Coleta agendada com sucesso.",
                        links =
                            new {
                                self = "api/controle-coletas/coletas/" + viagemColeta.Codigo.ToString(), 
                                viagem = "api/acompanhamento-logistica/viagens/" + viagemColeta.Codigo.ToString()
                            }
                };
            }
            else
            {
                return new
                {
                    sucesso = false,
                    mensagem = "Não foi possível realizar o agendamento da coleta."
                };
            }
            
        }

        [HttpPost("api/controle-coletas/coletas/{id}")]
        public ColetaDTO getColeta(int id)
        {
            return new ColetaDTO();
        }
    }
}