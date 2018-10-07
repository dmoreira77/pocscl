using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Acesso
{
    public class DAOUsuario
    {
        MySqlConnection conn = ConfiguracaoAcessoDados.getConexao();

        public Usuario obter(String cpf)
        {
            IEnumerable<Usuario> res = conn.Query<Usuario>("Select * from Usuario where cpf = @cpf", new { cpf = cpf });
            if(res.Count() > 0)
            {
                return res.ElementAt(0);
            }
            else
            {
                return null;
            }
            
        }
    }
}
