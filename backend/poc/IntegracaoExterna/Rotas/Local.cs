using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.IntegracaoExterna.Rotas
{
    public class Local
    {
        String endereco;
        String municipio;
        String uf;

        public string Endereco { get => endereco; set => endereco = value; }
        public string Municipio { get => municipio; set => municipio = value; }
        public string Uf { get => uf; set => uf = value; }

        public String enderecoCompleto()
        {
            return Endereco + " - " + Municipio + "/" + Uf;
        }
    }
}
