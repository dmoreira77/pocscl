using Dapper;
using Dapper.Mapper;
using MySql.Data.MySqlClient;
using poc.GestaoFrotas.DTO;
using poc.GestaoFrotas.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoInterna.GestaoFrotas
{
    public class ConectorGestaoFrotas
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=rdsrs7!;persistsecurityinfo=False;database=poc_transp");


        public IEnumerable<VeiculoDisponivelDTO> obterVeiculosDisponiveis(String dataLimite, float distancia, int qtdParadas, float pesoTotal, float volumeTotal)
        {
            IEnumerable<DisponibilidadeVeiculo> disponibilidades = obterDisponibilidades(dataLimite, distancia, qtdParadas, pesoTotal, volumeTotal);

            String placaRef = "";
            List<VeiculoDisponivelDTO> vd = new List<VeiculoDisponivelDTO>();
            foreach (var d in disponibilidades)
            {
                if (placaRef != d.Veiculo.Placa)
                {
                    vd.Add(new VeiculoDisponivelDTO()
                    {
                        Modelo = d.Veiculo.Modelo,
                        Peso = d.Veiculo.PesoCapacidade,
                        Placa = d.Veiculo.Placa,
                        VelocidadeMedia = d.Veiculo.VelocidadeMedia,
                        Volume = d.Veiculo.VolumeCapacidade,
                        NomeMotorista = d.Veiculo.Motorista.Nome,
                        Disponibilidade = new List<DisponibilidadeDTO>()
                    });

                    placaRef = d.Veiculo.Placa;
                }

                vd.Last().Disponibilidade.Add(
                    new DisponibilidadeDTO()
                    {
                        DataHoraFim = d.DataHoraFim.HasValue ? d.DataHoraFim.Value.ToString("dd/MM/yyyy HH:mm:ss") : null,
                        DataHoraInicio = d.DataHoraInicio.ToString("dd/MM/yyyy HH:mm:ss")
                    }
                );
            }

            return vd;
        }

        public IEnumerable<DisponibilidadeVeiculo> obterDisponibilidades(String dataLimite, float distancia, int qtdParadas, float pesoTotal, float volumeTotal)
        {
            DynamicParameters parametros = new DynamicParameters();

            String sql = "select * " +
                            "from disponibilidade_veiculo a " +
                            "inner join veiculo v on a.placa = v.placa " +
                            "inner join motorista mot on mot.cpfMotorista = v.cpfMotorista " +
                            "where " +
                                    "a.dataHoraInicio < @dataLimite and a.dataHoraInicio > '2018-09-02 00:00:00'";

            String dataParametro = dataLimite.Split("/")[2] + "-" + dataLimite.Split("/")[1] + "-" + dataLimite.Split("/")[0];

            parametros.Add("@dataLimite", dataParametro, DbType.DateTime, ParameterDirection.Input);

            if ((distancia > 0) && (qtdParadas > 0))
            {
                sql += "and(" +

                       "(a.dataHoraFim is null) " +

                       "or " +

                       "((((hour(timediff(a.dataHoraFim, a.dataHoraInicio)) + (minute(timediff(a.dataHoraFim, a.dataHoraInicio)) / 60) + (second(timediff(a.dataHoraFim, a.dataHoraInicio)) / 3600)) - (@qtdParadas * (0.5))) * v.velocidadeMedia) >= @distancia) " +

                       ") ";

                parametros.Add("@qtdParadas", qtdParadas, DbType.Int32, ParameterDirection.Input);
                parametros.Add("@distancia", distancia, DbType.Double, ParameterDirection.Input);

            }

            if (pesoTotal > 0)
            {
                sql += "and pesoCapacidade >= @pesoTotal ";
                parametros.Add("@pesoTotal", pesoTotal, DbType.Double, ParameterDirection.Input);
            }

            if (volumeTotal > 0)
            {
                sql += "and volumeCapacidade >= @volumeTotal ";
                parametros.Add("@volumeTotal", volumeTotal, DbType.Double, ParameterDirection.Input);
            }

            sql += " order by v.placa, dataHoraInicio";

            var disponibilidades = conn.Query<DisponibilidadeVeiculo, Veiculo, Motorista>(sql, parametros, splitOn: "placa, cpfMotorista");

            return disponibilidades;
        }

        public object atualizarDisponibilidade(String placa, DateTime dataHoraInicio, DateTime dataHoraFimPrevisto)
        {
            //Aqui viria o código responsável pelo encaminhamento de uma requisição ao Sistema de Gestão de Frotas e o retorno de uma resposta

            if(true)
            {
                return new { sucesso = true, mensagem = "O Sistema de Gestão de Frotas agendou a viagem com sucesso." };
            }
            else
            {
                return new { sucesso = false, mensagem = "O Sistema de Gestão de Frotas falhou em agendar a viagem. Tente novamente." };
            }
        }
    }
}
