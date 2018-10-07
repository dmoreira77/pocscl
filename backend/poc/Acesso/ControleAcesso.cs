using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Acesso
{
    public class ControleAcesso
    {
        public static Usuario Login(string cpf, string senha)
        {
            Usuario u = (new DAOUsuario()).obter(cpf);

            if(u != null)
            { 
                if(!u.Senha.Equals(senha))
                {
                    u = null;
                }
            }

            return u;
        }
    }
}
