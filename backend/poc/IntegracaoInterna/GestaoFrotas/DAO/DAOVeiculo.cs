using MySql.Data.MySqlClient;
using poc.IntegracaoInterna.GestaoFrotas.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoInterna.GestaoFrotas.DAO
{
    public class DAOVeiculo
    {
        MySqlConnection conn = ConfiguracaoAcessoDados.getConexao();


        public IEnumerable<Veiculo> obterVeiculosDisponiveis()
        {


            return null;
        }
        
    }
}
