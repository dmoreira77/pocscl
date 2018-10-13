using poc.Cadastro.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.DTO
{
    public class CentroDistribuicaoDTO
    {
        private int codigo;
        private String nome;
        private String endereco;

        public int Codigo { get => codigo; set => codigo = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Endereco { get => endereco; set => endereco = value; }
    }
}
