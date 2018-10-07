using poc.AcompanhamentoLogistica;
using poc.AcompanhamentoLogistica.Modelo;
using poc.Cadastro;
using poc.Cadastro.Modelo;
using poc.Coletas.DTO;
using poc.IntegracaoInterna.GestaoFrotas.Modelo;
using poc.SolicitacoesTransporte;
using poc.SolicitacoesTransporte.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Coletas.Modelo
{
    public class ConversorColetaViagem
    {
        public static Viagem converterParaViagem(ColetaDTO coleta)
        {
            Viagem viagemColeta = new Viagem()
            {
                TipoViagem = "Coleta",
                Veiculo = new Veiculo() { Placa = coleta.Placa },
                Paradas = new List<Parada>(),
                DataHoraPartida = DateTime.Parse(coleta.DataPartida + " " + coleta.HoraPartida, CultureInfo.CreateSpecificCulture("pt-BR")),
                DataHoraRetornoPrevisto = DateTime.Parse(coleta.DataRetornoEstimada + " " + coleta.HoraRetornoEstimada, CultureInfo.CreateSpecificCulture("pt-BR")),
                DistanciaTotal = coleta.Roteiro.Distancia
            };

            int ordem = 1;
            foreach (var paradaColeta in coleta.Roteiro.Paradas)
            {

                Parada paradaViagem = new Parada()
                {
                    Endereco = paradaColeta.Endereco,
                    Cep = paradaColeta.Cep,
                    Municipio = new Municipio() { Codigo = paradaColeta.CodigoMunicipio},
                    DataHoraChegadaPrevista = DateTime.Parse(paradaColeta.HorarioChegada, CultureInfo.CreateSpecificCulture("pt-BR")),
                    DistanciaTrecho = paradaColeta.Distancia,
                    Ordem = ordem,
                    Itens = new List<ItemTransporte>()
                };

                foreach (var item in coleta.Itens)
                {
                    if (item.Endereco == paradaViagem.Endereco)
                    {
                        paradaViagem.Itens.Add(new ItemTransporte()
                        {
                            Codigo = item.Codigo
                        });
                    }
                }

                viagemColeta.Paradas.Add(paradaViagem);

                ordem++;
            }

            return viagemColeta;
        }
    }
}
