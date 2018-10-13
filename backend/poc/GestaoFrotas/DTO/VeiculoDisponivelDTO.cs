using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.GestaoFrotas.DTO
{
    public class VeiculoDisponivelDTO
    {
        private String modelo;
        private String placa;
        private float volume;
        private float peso;
        private String nomeMotorista;
        private int velocidadeMedia;
        private List<DisponibilidadeDTO> disponibilidade;

        public string Modelo { get => modelo; set => modelo = value; }
        public string Placa { get => placa; set => placa = value; }
        public float Volume { get => volume; set => volume = value; }
        public float Peso { get => peso; set => peso = value; }
        public string NomeMotorista { get => nomeMotorista; set => nomeMotorista = value; }
        public int VelocidadeMedia { get => velocidadeMedia; set => velocidadeMedia = value; }
        public List<DisponibilidadeDTO> Disponibilidade { get => disponibilidade; set => disponibilidade = value; }
        
    }
}
