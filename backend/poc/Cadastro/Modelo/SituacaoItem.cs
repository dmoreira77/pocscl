using Dapper;
using Dapper.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.Modelo
{
    public class SituacaoItem
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string TipoOperacao { get; set; }
        public bool ConsideradoSucesso { get; set; }
        public bool EscopoViagem { get; set; }
    }
}
