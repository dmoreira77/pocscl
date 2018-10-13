using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using poc.GestaoFrotas.DTO;
using poc.IntegracaoInterna.GestaoFrotas;

namespace poc.GestaoFrotas
{
    [ApiController]
    public class GestaoFrotasController : ControllerBase
    {
        [HttpGet("api/integracao-interna/gestao-frotas/veiculos-disponiveis")]
        public IEnumerable<VeiculoDisponivelDTO> GetVeiculosDisponiveis([FromQuery] String dataLimite, [FromQuery] float distancia, [FromQuery] int qtdParadas, [FromQuery] float peso, [FromQuery] float volume)
        {
            IEnumerable<VeiculoDisponivelDTO> vs = (new FacadeGestaoFrotas()).obterVeiculosDisponiveis(dataLimite, distancia, qtdParadas, peso, volume);

            return vs;
        }
    }
}