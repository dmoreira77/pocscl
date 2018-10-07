using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoInterna.GestaoFrotas.Modelo
{
    public class Veiculo
    {
        private String placa;
        private String modelo;
        private float volumeCapacidade;
        private float pesoCapacidade;
        private int velocidadeMedia;
        private Motorista motorista;

        public string Placa { get => placa; set => placa = value; }
        public string Modelo { get => modelo; set => modelo = value; }
        public float VolumeCapacidade { get => volumeCapacidade; set => volumeCapacidade = value; }
        public float PesoCapacidade { get => pesoCapacidade; set => pesoCapacidade = value; }
        public int VelocidadeMedia { get => velocidadeMedia; set => velocidadeMedia = value; }
        public Motorista Motorista { get => motorista; set => motorista = value; }
    }
}
