using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Acesso
{
    public class Usuario
    {
        private String cpf;
        private String nome;
        private String senha;
        private String perfil;

        public string Cpf { get => cpf; set => cpf = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Senha { get => senha; set => senha = value; }
        public string Perfil { get => perfil; set => perfil = value; }
    }
}
