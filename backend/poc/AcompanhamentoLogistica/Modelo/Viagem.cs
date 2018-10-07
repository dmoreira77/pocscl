using poc.IntegracaoInterna.GestaoFrotas;
using poc.IntegracaoInterna.GestaoFrotas.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.AcompanhamentoLogistica.Modelo
{
    public class Viagem
    {
        private int codigo;
        private Veiculo veiculo;
        private String tipoViagem;
        private DateTime dataHoraPartida;
        private DateTime dataHoraRetornoPrevisto;
        private DateTime dataHoraRetornoReal;
        private float distanciaTotal;

        List<Parada> paradas;

        public int Codigo { get => codigo; set => codigo = value; }
        public Veiculo Veiculo { get => veiculo; set => veiculo = value; }
        public string TipoViagem { get => tipoViagem; set => tipoViagem = value; }
        public List<Parada> Paradas { get => paradas; set => paradas = value; }
        public DateTime DataHoraPartida { get => dataHoraPartida; set => dataHoraPartida = value; }
        public float DistanciaTotal { get => distanciaTotal; set => distanciaTotal = value; }
        public DateTime DataHoraRetornoPrevisto { get => dataHoraRetornoPrevisto; set => dataHoraRetornoPrevisto = value; }
        public DateTime DataHoraRetornoReal { get => dataHoraRetornoReal; set => dataHoraRetornoReal = value; }
    }
}
