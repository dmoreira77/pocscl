﻿using Dapper;
using Dapper.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.Modelo
{
    [Table("situacao_item")]
    public class SituacaoItem
    {
        [Key]
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string TipoOperacao { get; set; }
        public bool ConsideradoSucesso { get; set; }
        public bool EscopoViagem { get; set; }
    }
}
