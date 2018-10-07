using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.Modelo
{
    public class CentroDistribuicao
    {
        private int codigo;
        private String nome;
        private String endereco;
        private Municipio municipio;
        private UF uf;
        private String cep;

        public int Codigo { get => codigo; set => codigo = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public Municipio Municipio { get => municipio; set => municipio = value; }
        public UF UF { get => uf; set => uf = value; }
        public string Cep { get => cep; set => cep = value; }
    }
}
