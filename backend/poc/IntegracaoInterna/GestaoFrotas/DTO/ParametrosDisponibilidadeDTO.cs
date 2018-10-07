using poc.Coletas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoInterna.GestaoFrotas.DTO
{
    public class ParametrosDisponibilidadeDTO
    {
        private float distancia;
        private float peso;
        private float volume;
        private String dataLimite;

        public float Peso { get => peso; set => peso = value; }
        public float Volume { get => volume; set => volume = value; }
        public string DataLimite { get => dataLimite; set => dataLimite = value; }
        public float Distancia { get => distancia; set => distancia = value; }
    }
}
