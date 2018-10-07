using poc.Cadastro;
using poc.Cadastro.Modelo;
using poc.SolicitacoesTransporte;
using poc.SolicitacoesTransporte.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.AcompanhamentoLogistica.Modelo
{
    public class Parada
    {
        private int codigo;
        private int ordem;
        private String endereco;
        private Municipio municipio;
        private String cep;
        private List<ItemTransporte> itens;
        private float distanciaTrecho;
        private DateTime dataHoraChegadaPrevista;
        private DateTime dataHoraChegadaReal;

        public int Codigo { get => codigo; set => codigo = value; }
        public int Ordem { get => ordem; set => ordem = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public Municipio Municipio { get => municipio; set => municipio = value; }
        public string Cep { get => cep; set => cep = value; }
        public List<ItemTransporte> Itens { get => itens; set => itens = value; }
        public DateTime DataHoraChegadaReal { get => dataHoraChegadaReal; set => dataHoraChegadaReal = value; }
        public DateTime DataHoraChegadaPrevista { get => dataHoraChegadaPrevista; set => dataHoraChegadaPrevista = value; }
        public float DistanciaTrecho { get => distanciaTrecho; set => distanciaTrecho = value; }
    }
}
