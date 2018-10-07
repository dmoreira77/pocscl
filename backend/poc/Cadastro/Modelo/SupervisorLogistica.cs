using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Cadastro.Modelo
{
    public class SupervisorLogistica
    {
        private String cpf;
        private String nome;
        private CentroDistribuicao centroDistribuicao;

        public string Cpf { get => cpf; set => cpf = value; }
        public string Nome { get => nome; set => nome = value; }
        public CentroDistribuicao CentroDistribuicao { get => centroDistribuicao; set => centroDistribuicao = value; }
    }
}
