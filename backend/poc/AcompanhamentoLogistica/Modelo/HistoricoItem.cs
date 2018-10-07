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
    public class HistoricoItem
    {
        private int codigo;
        private ItemTransporte item;
        private SituacaoItem situacao;
        private DateTime dataHora;
        private String observacao;

        public ItemTransporte Item { get => item; set => item = value; }
        public SituacaoItem Situacao { get => situacao; set => situacao = value; }
        public DateTime DataHora { get => dataHora; set => dataHora = value; }
        public string Observacao { get => observacao; set => observacao = value; }
        public int Codigo { get => codigo; set => codigo = value; }
    }
}
