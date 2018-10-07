using poc.Cadastro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Coletas.DTO
{
    public class ParadaRoteiroColetaDTO
    {
        private String endereco;
        private String municipio;
        private String uf;
        private int codigoMunicipio;
        private String cep;
        private String janelaInicio;
        private String janelaFim;
        private float distancia;
        private String horarioChegada;

        public string Endereco { get => endereco; set => endereco = value; }
        public string JanelaInicio { get => janelaInicio; set => janelaInicio = value; }
        public string JanelaFim { get => janelaFim; set => janelaFim = value; }
        public float Distancia { get => distancia; set => distancia = value; }
        public string HorarioChegada { get => horarioChegada; set => horarioChegada = value; }
        public string Cep { get => cep; set => cep = value; }
        public string Municipio { get => municipio; set => municipio = value; }
        public string Uf { get => uf; set => uf = value; }
        public int CodigoMunicipio { get => codigoMunicipio; set => codigoMunicipio = value; }
    }
}