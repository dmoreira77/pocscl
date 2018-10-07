using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc.Acesso
{
    public class UsuarioAutenticado
    {
        private readonly IHttpContextAccessor _accessor;

        public UsuarioAutenticado(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Email => _accessor.HttpContext.User.Identity.Name;
    }
}
