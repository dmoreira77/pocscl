using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.Modelo
{
    public class Unidade
    {
        private String descricao;
        private String sigla;

        public string Descricao { get => descricao; set => descricao = value; }
        public string Sigla { get => sigla; set => sigla = value; }
    }
}
