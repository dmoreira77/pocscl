using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using poc.Cadastro.DAO;
using poc.Cadastro.DTO;
using poc.Cadastro.Modelo;

namespace poc.Cadastro
{
    [ApiController]
    public class CentrosDistribuicaoController : ControllerBase
    {
        [HttpGet("api/centros-distribuicao")]
        public IEnumerable<CentroDistribuicaoDTO> getCentrosDistribuicao()
        {
            return (new DAOCentroDistribuicao()).obterTodosDTO();
        }

        [HttpGet("api/centros-distribuicao/supervisores-distribuicao/{cpf}")]
        public SupervisorLogistica getCentrosDistribuicao(String cpf)
        {
            return (new DAOSupervisorLogistica()).obter(cpf);
        }
    }
}