using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.AcompanhamentoLogistica
{
    public class AtualizacaoSituacaoDTO
    {
        private int codigoSituacao;
        private String descricaoSituacao;

        public int CodigoSituacao { get => codigoSituacao; set => codigoSituacao = value; }
        public string DescricaoSituacao { get => descricaoSituacao; set => descricaoSituacao = value; }
    }
}
