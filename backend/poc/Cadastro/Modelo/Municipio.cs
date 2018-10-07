using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.Modelo
{
    public class Municipio
    {
        private int codigo;
        private String nome;
        private UF uf;

        public int Codigo { get => codigo; set => codigo = value; }
        public string Nome { get => nome; set => nome = value; }
        public UF Uf { get => uf; set => uf = value; }
    }
}
