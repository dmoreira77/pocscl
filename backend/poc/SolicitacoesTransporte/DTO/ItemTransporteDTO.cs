using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.SolicitacoesTransporte.DTO
{
    public class ItemTransporteDTO
    {
        int codigo;
        String descricao;
        String unidade;
        float quantidade;
        float volume;
        float peso;
        String cuidados;
        String cliente;
        String enderecoColeta;
        String complementoColeta;
        String municipioColeta;
        String ufColeta;
        String cepColeta;
        String dataLimiteColeta;
        String janelaColeta;
        String nomeDestinatario;
        String enderecoEntrega;
        String complementoEntrega;
        String municipioEntrega;
        String ufEntrega;
        String dataLimiteEntrega;
        int codigoSituacao;
        String descricaoSituacao;
        String tipoOperacao;
        int cd;

        public int Codigo { get => codigo; set => codigo = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public float Quantidade { get => quantidade; set => quantidade = value; }
        public float Volume { get => volume; set => volume = value; }
        public float Peso { get => peso; set => peso = value; }
        public string Cuidados { get => cuidados; set => cuidados = value; }
        public string Cliente { get => cliente; set => cliente = value; }
        public string EnderecoColeta { get => enderecoColeta; set => enderecoColeta = value; }
        public string MunicipioColeta { get => municipioColeta; set => municipioColeta = value; }
        public string UfColeta { get => ufColeta; set => ufColeta = value; }
        public string CepColeta { get => cepColeta; set => cepColeta = value; }
        public string DataLimiteColeta { get => dataLimiteColeta; set => dataLimiteColeta = value; }
        public string JanelaColeta { get => janelaColeta; set => janelaColeta = value; }
        public string EnderecoEntrega { get => enderecoEntrega; set => enderecoEntrega = value; }
        public string MunicipioEntrega { get => municipioEntrega; set => municipioEntrega = value; }
        public string UfEntrega { get => ufEntrega; set => ufEntrega = value; }
        public string DataLimiteEntrega { get => dataLimiteEntrega; set => dataLimiteEntrega = value; }
        public int Cd { get => cd; set => cd = value; }
        public int CodigoSituacao { get => codigoSituacao; set => codigoSituacao = value; }
        public string DescricaoSituacao { get => descricaoSituacao; set => descricaoSituacao = value; }
        public string TipoOperacao { get => tipoOperacao; set => tipoOperacao = value; }
        public string NomeDestinatario { get => nomeDestinatario; set => nomeDestinatario = value; }
        public string ComplementoColeta { get => complementoColeta; set => complementoColeta = value; }
        public string ComplementoEntrega { get => complementoEntrega; set => complementoEntrega = value; }
    }
}
