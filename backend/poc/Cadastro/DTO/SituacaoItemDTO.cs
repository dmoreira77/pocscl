using poc.Cadastro.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.DTO
{
    public class SituacaoItemDTO
    {
        private String descricao;
        private int codigo;
        private String tipoOperacao;
        private bool consideradoSucesso;

        public string Descricao { get => descricao; set => descricao = value; }
        public int Codigo { get => codigo; set => codigo = value; }
        public string TipoOperacao { get => tipoOperacao; set => tipoOperacao = value; }
        public bool ConsideradoSucesso { get => consideradoSucesso; set => consideradoSucesso = value; }

        public static SituacaoItemDTO criar(SituacaoItem situacao)
        {
            SituacaoItemDTO dto = new SituacaoItemDTO()
            {
                Descricao = situacao.Descricao,
                Codigo = situacao.Codigo,
                TipoOperacao = situacao.TipoOperacao,
                ConsideradoSucesso = situacao.ConsideradoSucesso
            };

            return dto;
        }
    }
}
