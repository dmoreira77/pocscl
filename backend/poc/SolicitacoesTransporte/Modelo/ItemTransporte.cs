using poc.Cadastro;
using poc.Cadastro.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.SolicitacoesTransporte.Modelo
{
    public class ItemTransporte
    {
        int codigo;
        String descricao;
        Unidade unidade;
        float quantidade;
        float volume;
        float peso;
        String cuidados;
        Cliente cliente;
        String enderecoColeta;
        Municipio municipioColeta;
        String cepColeta;
        DateTime dataLimiteColeta;
        String janelaInicioColeta;
        String janelaFimColeta;
        String nomeDestinatario;
        String enderecoEntrega;
        Municipio municipioEntrega;
        String cepEntrega;
        DateTime dataLimiteEntrega;
        CentroDistribuicao centroDistribuicao;
        SituacaoItem situacaoAtual;

        public int Codigo { get => codigo; set => codigo = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public Unidade Unidade { get => unidade; set => unidade = value; }
        public float Quantidade { get => quantidade; set => quantidade = value; }
        public float Volume { get => volume; set => volume = value; }
        public float Peso { get => peso; set => peso = value; }
        public string Cuidados { get => cuidados; set => cuidados = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }
        public string EnderecoColeta { get => enderecoColeta; set => enderecoColeta = value; }
        public Municipio MunicipioColeta { get => municipioColeta; set => municipioColeta = value; }
        public string CepColeta { get => cepColeta; set => cepColeta = value; }
        public DateTime DataLimiteColeta { get => dataLimiteColeta; set => dataLimiteColeta = value; }
        public string JanelaInicioColeta { get => janelaInicioColeta; set => janelaInicioColeta = value; }
        public string JanelaFimColeta { get => janelaFimColeta; set => janelaFimColeta = value; }
        public string EnderecoEntrega { get => enderecoEntrega; set => enderecoEntrega = value; }
        public Municipio MunicipioEntrega { get => municipioEntrega; set => municipioEntrega = value; }
        public string CepEntrega { get => cepEntrega; set => cepEntrega = value; }
        public DateTime DataLimiteEntrega { get => dataLimiteEntrega; set => dataLimiteEntrega = value; }
        public CentroDistribuicao CentroDistribuicao { get => centroDistribuicao; set => centroDistribuicao = value; }
        public SituacaoItem SituacaoAtual { get => situacaoAtual; set => situacaoAtual = value; }
        public string NomeDestinatario { get => nomeDestinatario; set => nomeDestinatario = value; }
    }
}
