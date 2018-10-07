using Dapper;

using MySql.Data.MySqlClient;
using poc.AcompanhamentoLogistica.DTO;
using poc.AcompanhamentoLogistica.Modelo;
using poc.IntegracaoInterna.GestaoFrotas;
using poc.SolicitacoesTransporte;
using poc.SolicitacoesTransporte.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.AcompanhamentoLogistica.DAO
{
    public class DAOViagem
    {
        MySqlConnection conn = ConfiguracaoAcessoDados.getConexao();

        String sqlViagemDTO = "select " +
                            "v.codigo, v.placaVeiculo as placa, v.tipoViagem, mot.cpfMotorista, mot.nome as nomeMotorista, " +

                            "parada_map.codigo, parada_map.endereco, mun.nome as municipio, mun.siglaUf as uf, parada_map.cep, " +

                            "item.codigo as codigoItem, item.*, item.siglaUnidade as unidade, cli.nome as cliente, munC.nome as municipioColeta, " +
                                "munC.siglaUf as ufColeta, concat(janelaInicioColeta, ' - ', janelaFimColeta) as janela," +
                                " munE.nome as municipioEntrega, munE.siglaUf as ufEntrega, " +
                                "codSituacaoAtual as codigoSituacao, sit.descricao as descricaoSituacao, " +
                                "sit.tipoOperacao, item.codCentroDistribuicao as cd " +

                             "from viagem v inner join  " +
                                    "(select ip.codItem, p.* " +
                                    "from item_parada ip inner join parada_viagem p on ip.codParada = p.codigo " +
                                    ") as parada_map on parada_map.codViagem = v.codigo " +
                            "inner join item_transp item on parada_map.codItem = item.codigo " +
                            "inner join Veiculo vei on vei.placa = v.placaVeiculo " +
                            "inner join Motorista mot on mot.cpfMotorista = vei.cpfMotorista " +
                            "inner join Municipio mun on mun.codigo = parada_map.codMunicipio " +
                            "inner join Municipio munC on munC.codigo = item.codMunicipioColeta " +
                            "inner join Municipio munE on munE.codigo = item.codMunicipioEntrega " +
                            "inner join Cliente cli on cli.codigo = item.codCliente " +
                            "inner join Situacao_item sit on sit.codigo = item.codSituacaoAtual";


        public bool incluir(Viagem viagem)
        {
            conn.Open();
            bool ok = false;
            using (var t = conn.BeginTransaction())
            {
                try
                {
                    conn.Execute("Insert into viagem (placaVeiculo, tipoViagem, dataHoraPartida, dataHoraRetornoPrevisto, distanciaTotal)" +
                                    " values(@placaVeiculo, @tipoViagem, @dataHoraPartida, @dataHoraRetornoPrevisto, @distanciaTotal)",
                                    new { placaVeiculo = viagem.Veiculo.Placa, tipoViagem = viagem.TipoViagem, dataHoraPartida = viagem.DataHoraPartida, dataHoraRetornoPrevisto = viagem.DataHoraRetornoPrevisto, distanciaTotal = viagem.DistanciaTotal });

                    int codigoGerado = conn.QuerySingle("select codigo from viagem where " +
                                        "placaVeiculo = @placaVeiculo " +
                                        "and tipoViagem = @tipoViagem " +
                                        "and dataHoraPartida = @dataHoraPartida",
                                        new
                                        {
                                            placaVeiculo = viagem.Veiculo.Placa,
                                            tipoViagem = viagem.TipoViagem,
                                            dataHoraPartida = viagem.DataHoraPartida
                                        }
                                        ).codigo;

                    viagem.Codigo = codigoGerado;

                    foreach (var parada in viagem.Paradas)
                    {
                        conn.Execute("Insert into parada_viagem (ordem, codViagem, endereco, codMunicipio, cep, " +
                                        "distanciaTrecho, dataHoraChegadaPrevista)" +
                                        "values(@ordem, @codViagem, @endereco, @codMunicipio, @cep, " +
                                        "@distanciaTrecho, @dataHoraChegadaPrevista)",
                                        new
                                        {
                                            ordem = parada.Ordem,
                                            codViagem = viagem.Codigo,
                                            endereco = parada.Endereco,
                                            codMunicipio = parada.Municipio.Codigo,
                                            cep = parada.Cep,
                                            distanciaTrecho = parada.DistanciaTrecho,
                                            dataHoraChegadaPrevista = parada.DataHoraChegadaPrevista
                                        }
                                        );

                        codigoGerado = conn.QuerySingle("select codigo from parada_viagem where " +
                                       "ordem = @ordem " +
                                       "and codViagem = @codViagem",
                                        new
                                        {
                                            ordem = parada.Ordem,
                                            codViagem = viagem.Codigo
                                        }
                                       ).codigo;
                        parada.Codigo = codigoGerado;

                        foreach (var item in parada.Itens)
                        {
                            var param = new {
                                codItem = item.Codigo,
                                codParada = parada.Codigo};

                            conn.Execute("insert into item_parada values(@codItem, @codParada)", param);

                            conn.Execute("Update item_transp set codSituacaoAtual = 12 where codigo = @codItem",
                                        param);

                            conn.Execute("Insert into historico_item (codItem, codSituacao, dataHora) " +
                                        "values (@codItem, 12, now())", param);

                        }

                        dynamic agendamentoGestaoFrotas = (new ConectorGestaoFrotas()).atualizarDisponibilidade(
                                                                                viagem.Veiculo.Placa,
                                                                                viagem.DataHoraPartida,
                                                                                viagem.DataHoraRetornoPrevisto);
                        if (!agendamentoGestaoFrotas.sucesso)
                        {
                            throw new Exception(agendamentoGestaoFrotas.mensagem);
                        }
                        
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

        public ViagemDTO obter(int codigoViagem)
        {
            String sql = sqlViagemDTO + " where parada_map.codViagem = @codViagem";

            return consultarViagem(sql, new { codViagem = codigoViagem });
        }

        private ViagemDTO consultarViagem(String sql, object param)
        {
            ViagemDTO viagemRes = null;
            ParadaDTO paradaAtual = null;

            String enderecoAnterior = "";

            conn.Open();

            IEnumerable<ViagemDTO> v = conn.Query<ViagemDTO, ParadaDTO, ItemTransporteDTO, ViagemDTO>(
                                                sql,
                                                (viagem, parada, item) =>
                                                {
                                                    if (viagemRes == null)
                                                    {
                                                        viagemRes = viagem;
                                                        viagemRes.Paradas = new List<ParadaDTO>();
                                                    }

                                                    if (parada.Endereco != enderecoAnterior)
                                                    {
                                                        paradaAtual = parada;
                                                        ((List<ParadaDTO>)viagemRes.Paradas).Add(paradaAtual);
                                                        paradaAtual.Itens = new List<ItemTransporteDTO>();

                                                        enderecoAnterior = parada.Endereco;
                                                    }

                                                    ((List<ItemTransporteDTO>)paradaAtual.Itens).Add(item);

                                                    return viagemRes;
                                                },
                                                param: param,
                                                splitOn: "codigo, codigoItem");

            conn.Close();

            return viagemRes;
        }

        public ViagemDTO obterViagemAtual(String cpf)
        {
            String sql = sqlViagemDTO + " where mot.cpfMotorista = @cpfMotorista and dataHoraPartida < now() and dataHoraRetornoReal is null";

            return consultarViagem(sql, new { cpfMotorista = cpf });
        }
    }
}
