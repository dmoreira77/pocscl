using poc.SolicitacoesTransporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Coletas.DTO
{
    public class ItemColetaDTO
    {
        private int codigo;
        private String descricao;
        private float volume;
        private float peso;
        private String dataLimite;
        private String janelaInicio;
        private String janelaFim;
        private String endereco;
        private String complemento;
        private String municipio;
        private int codigoMunicipio;
        private String uf;
        private String cep;

        public string Descricao { get => descricao; set => descricao = value; }
        public float Volume { get => volume; set => volume = value; }
        public float Peso { get => peso; set => peso = value; }
        public string DataLimite { get => dataLimite; set => dataLimite = value; }
        public string JanelaInicio { get => janelaInicio; set => janelaInicio = value; }
        public string JanelaFim { get => janelaFim; set => janelaFim = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public int Codigo { get => codigo; set => codigo = value; }
        public string Municipio { get => municipio; set => municipio = value; }
        public string Uf { get => uf; set => uf = value; }
        public string Cep { get => cep; set => cep = value; }
        public int CodigoMunicipio { get => codigoMunicipio; set => codigoMunicipio = value; }
        public string Complemento { get => complemento; set => complemento = value; }
    }
}
