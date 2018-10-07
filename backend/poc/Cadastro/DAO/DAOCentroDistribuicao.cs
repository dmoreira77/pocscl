using Dapper;
using Dapper.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using poc.Cadastro.DTO;
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
    public class DAOCentroDistribuicao
    {
        MySqlConnection conn = ConfiguracaoAcessoDados.getConexao();

        public IEnumerable<CentroDistribuicaoDTO> obterTodosDTO()
        {
            return conn.Query<CentroDistribuicaoDTO>("Select codigo, nome, endereco from centro_distribuicao").ToList();
        }

        public IEnumerable<CentroDistribuicao> obterTodos()
        {
            var lista = conn.Query<CentroDistribuicao, Municipio, UF, CentroDistribuicao>("Select cd.codigo as codigoCD, cd.*, " +
                                                    "m.codigo as codigoMunicipio, m.*, " +
                                                    "uf.* " +
                                                    "from centro_distribuicao cd " +
                                                    "inner join municipio m on cd.codMunicipio = m.codigo " +
                                                    "inner join uf as uf on uf.sigla = m.siglaUf",
                                                    (cd, m, uf) =>
                                                    {
                                                        cd.Municipio = m;
                                                        m.Uf = uf;
                                                        return cd;
                                                    },
                                                    splitOn: "codigoCD, codigoMunicipio, siglaUf"
                                                    );

            if(lista.Count() > 0)
            {
                return lista;
            }
            else
            {
                return null;
            }
        }
    }
}
