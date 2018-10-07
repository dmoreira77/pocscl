using Dapper;
using MySql.Data.MySqlClient;
using poc.Cadastro.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.DAO
{
    public class DAOSupervisorLogistica
    {
        MySqlConnection conn = ConfiguracaoAcessoDados.getConexao();

        public SupervisorLogistica obter(String cpfSupervisor)
        {
            SupervisorLogistica sup = null;
            var res = conn.Query<SupervisorLogistica, CentroDistribuicao, Municipio, UF, SupervisorLogistica>("select s.cpf as cpfSupervisor, s.*, " +
                                                                                        "cd.codigo as codigoCd, cd.*, " +
                                                                                        "m.codigo as codigoMunicipio, m.*," +
                                                                                        "uf.* " +
                                                                                        "from Supervisor_Logistica s " +
                                                                                        "inner join Centro_Distribuicao cd " +
                                                                                            "on s.codCentroDistribuicao = cd.codigo " +
                                                                                        "inner join Municipio m on cd.codMunicipio = m.codigo " +
                                                                                        "inner join Uf uf on m.siglaUf = uf.sigla " +
                                                                                        "where s.cpf = @cpf",
                                                                                        (supervisor, cd, mun, uf) => {
                                                                                            supervisor.CentroDistribuicao = cd;
                                                                                            cd.Municipio = mun;
                                                                                            mun.Uf = uf;
                                                                                            return supervisor;
                                                                                        } ,
                                                                                        new { cpf = cpfSupervisor },
                                                                                        splitOn: "codigoCd, codigoMunicipio, siglaUf"
                                                                                        );
            if(res.Count() > 0)
            {
                sup = res.ElementAt(0);
            }

            return sup;
        }
    }
}
