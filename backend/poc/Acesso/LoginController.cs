using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using poc.Cadastro.DAO;
using poc.Cadastro.Modelo;

namespace poc.Acesso
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("api/acesso/login")]
        public object Post(
            [FromBody]Usuario usuario,
            [FromServices]DAOUsuario usersDAO,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            bool credenciaisValidas = false;
            Usuario usuarioBase = null;

            if (usuario != null && !String.IsNullOrWhiteSpace(usuario.Cpf))
            {
                usuarioBase = usersDAO.obter(usuario.Cpf);
                credenciaisValidas = (usuarioBase != null &&
                    usuario.Cpf == usuarioBase.Cpf &&
                    usuario.Senha == usuarioBase.Senha);
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(usuarioBase.Cpf, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuarioBase.Cpf),
                        new Claim("role", usuarioBase.Perfil)
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao                   
                });
                var token = handler.WriteToken(securityToken);

                CentroDistribuicao cd = null;
                if(usuarioBase.Perfil == "Supervisor de Distribuição")
                {
                    SupervisorLogistica sup = (new DAOSupervisorLogistica()).obter(usuarioBase.Cpf);
                    if (sup != null) cd = sup.CentroDistribuicao;
                }

                return new
                {
                    sucesso = true,
                    criacaoToken = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    tokenExpiraEm = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    token = token,
                    mensagem = "OK",
                    usuario = usuarioBase,
                    cd = cd
                };
            }
            else
            {
                return new
                {
                    sucesso = false,
                    mensagem = "Falha na autenticação."
                };
            }
        }

        
        [Authorize("Bearer")]
        [HttpGet("api/acesso/logado")]
        public Usuario usuarioLogado([FromServices] IHttpContextAccessor acessor, [FromServices] DAOUsuario dao)
        {
            String cpfToken = acessor.HttpContext.User.Identity.Name;

            return dao.obter(cpfToken);
        }
    }


}