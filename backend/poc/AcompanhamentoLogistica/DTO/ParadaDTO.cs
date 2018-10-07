using poc.SolicitacoesTransporte;
using poc.SolicitacoesTransporte.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.AcompanhamentoLogistica.DTO
{
    public class ParadaDTO
    {
        private String endereco;
        private String municipio;
        private String uf;
        private String cep;
        private IEnumerable<ItemTransporteDTO> itens;

        public string Endereco { get => endereco; set => endereco = value; }
        public string Municipio { get => municipio; set => municipio = value; }
        public string Uf { get => uf; set => uf = value; }
        public string Cep { get => cep; set => cep = value; }
        public IEnumerable<ItemTransporteDTO> Itens { get => itens; set => itens = value; }
        
    }
}
