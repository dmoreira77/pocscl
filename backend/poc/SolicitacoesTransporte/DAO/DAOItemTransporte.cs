using Dapper;
using MySql.Data.MySqlClient;
using poc.Cadastro;
using poc.Cadastro.Modelo;
using poc.Coletas;
using poc.Coletas.DTO;
using poc.IntegracaoExterna.Rotas;
using poc.SolicitacoesTransporte.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.SolicitacoesTransporte.DAO
{
    public class DAOItemTransporte
    {
        MySqlConnection conn = ConfiguracaoAcessoDados.getConexao();

        String sqlItemTransporteDTO = "Select item.codigo, item.descricao, item.quantidade, siglaUnidade as unidade, volume, peso, cuidados, " +
                                    "c.nome as cliente,concat(janelaInicioColeta, ' - ', janelaFimColeta) as janelaColeta, " +
                                    "enderecoColeta, mC.codigo as codigoMunicipioColeta, mC.nome as municipioColeta, mC.siglaUF as ufColeta, cepColeta, dataLimiteColeta,  " +
                                    "enderecoEntrega, mE.codigo as codigoMunicipioEntrega, mE.nome as municipioEntrega, mE.siglaUF as ufEntrega, dataLimiteEntrega, " +
                                    "codSituacaoAtual as codigoSituacao, sit.descricao as descricaoSituacao, sit.tipoOperacao, nomeDestinatario " +
                                    "from item_transp item " +
                                    "inner join Cliente c on item.codCliente= c.codigo " +
                                    "inner join Municipio mC on item.codMunicipioColeta = mC.codigo " +
                                    "inner join Municipio mE on item.codMunicipioEntrega = mE.codigo " +
                                    "inner join Situacao_Item sit on sit.codigo = item.codSituacaoAtual";

        public IEnumerable<ItemColetaDTO> obterItensPendentesColeta(int codCentroDistribuicao)
        {
            return conn.Query<ItemColetaDTO>("Select item.codigo, item.descricao, volume, peso, dataLimiteColeta as dataLimite, " +
                                                "date_format(dataLimiteColeta, '%d/%m/%Y') as dataLimite, date_format(janelaInicioColeta, '%H:%i:%S') as janelaInicio, date_format(janelaFimColeta, '%H:%i:%S') as janelaFim, " +
                                                "enderecoColeta as endereco, complementoEnderecoColeta as complemento, m.nome as municipio, " +
                                                "codMunicipioColeta as codigoMunicipio, m.siglaUF as uf, " +
                                                "cepColeta as cep from Item_transp item " +
                                                "inner join Municipio m on item.codMunicipioColeta = m.codigo " +
                                                "where item.codSituacaoAtual = 11 and codCentroDistribuicao = @cd", new { cd = codCentroDistribuicao }).ToList();
        }

        public IEnumerable<ItemTransporteDTO> obterItensNaoEncaminhados()
        {
            var itens = conn.Query<ItemTransporteDTO>(sqlItemTransporteDTO + " where item.codSituacaoAtual = 10").ToList();

            List<Local> locaisColetaItens = new List<Local>();
            foreach (var item in itens)
            {
                locaisColetaItens.Add(new Local()
                {
                    Endereco = item.EnderecoColeta,
                    Municipio = item.MunicipioColeta,
                    Uf = item.UfColeta
                });
            }

            CentroDistribuicao[] cdsProximos = FacadeRotas.centrosDistribuicaoMaisProximos(locaisColetaItens.ToArray());

            var i = 0;
            foreach (var cd in cdsProximos)
            {
                ItemTransporteDTO item = itens[i];

                item.Cd = 0;

                if (cd != null)
                {
                    item.Cd = cd.Codigo;
                }
                i++;
            }
            
            return itens;
        }

        public Boolean encaminharItensParaCDs(EncaminhamentoCdDTO[] encs)
        {
            bool ok = false;

            conn.Open();
            using (var t = conn.BeginTransaction())
            {
                try
                {
                    foreach (var enc in encs)
                    {
                        conn.Execute("Update Item_Transp set codCentroDistribuicao = @codigoCd, codSituacaoAtual = 11 where codigo = @codigoItem", enc);
                        conn.Execute("Insert into Historico_item (codItem, codSituacao, dataHora) values (@codigoItem, 11, now())", enc);
                    }

                    t.Commit();

                    ok = true;
                }
                catch (Exception e)
                {
                    t.Rollback();
                }
                finally
                {
                    conn.Close();
                }
                return ok;
            }
        }

        public Boolean atualizarSituacao(int codigoItem, int codigoSituacao, String obs)
        {
            bool ok = false;

            conn.Open();
            using (var t = conn.BeginTransaction())
            {
                try
                {
                    conn.Execute("Update Item_transp set codSituacaoAtual = @codSituacao where codigo = @codItem",
                        new { codSituacao = codigoSituacao, codItem = codigoItem });

                    conn.Execute("Insert into historico_item (codItem, codSituacao, dataHora, observacao) " +
                                    "values(@codItem, @codSituacao, now(), @observacao);",
                                    new { codItem = codigoItem, codSituacao = codigoSituacao, observacao = obs });
                    t.Commit();
                    ok = true;
                }
                catch(Exception e)
                {
                    t.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }

            return ok;
        }
    }
}
