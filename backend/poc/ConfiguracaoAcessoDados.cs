using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Mapper;
using MySql.Data.MySqlClient;

namespace poc
{
    public class ConfiguracaoAcessoDados
    {
        public static String connectionString()
        {
            return "server=localhost;user id=root;password=rdsrs7!;persistsecurityinfo=False;database=poc_transp";
        }

        public static MySqlConnection getConexao()
        {
            return new MySqlConnection(connectionString());
        }
    }
}
