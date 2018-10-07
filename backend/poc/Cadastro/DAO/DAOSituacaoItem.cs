using Dapper;
using Dapper.Mapper;
using MySql.Data.MySqlClient;
using poc.Cadastro.Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.DAO
{
    public class DAOSituacaoItem
    {
        MySqlConnection conn = ConfiguracaoAcessoDados.getConexao();

        public IEnumerable<SituacaoItem> obterTodasSituacoesViagem()
        {
            return conn.Query<SituacaoItem>("select * from situacao_item where escopoViagem = true").ToList();
        }

        public IEnumerable<SituacaoItem> obterTodasSituacoes()
        {
            return conn.Query<SituacaoItem>("select * from situacao_item where escopoViagem").ToList();
        }
    }
}
