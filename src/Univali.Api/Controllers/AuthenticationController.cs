using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Univali.Api.Models;

namespace Univali.Api.Controllers
{
    // Define a rota base para o controlador
    [Route("Api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Construtor do controlador que utiliza injeção de dependência
        // Recebe uma instância de IConfiguration para acessar configurações
        public AuthenticationController(IConfiguration configuration)
        {
            // Armazena a instância de IConfiguration para uso posterior
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // Define a rota e o método HTTP para autenticação do usuário
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestDto authenticationRequestDto)
        {
            // Valida as credenciais do usuário chamando o método ValidateUserCredentials
            var user = ValidateUserCredentials(authenticationRequestDto.Username!, authenticationRequestDto.Password!);

            // Verifica se o usuário é nulo, o que significa que as credenciais são inválidas
            if (user == null)
            {
                // Retorna uma resposta HTTP 401 (Unauthorized)
                return Unauthorized();
            }

            // Cria uma chave de segurança com base na chave secreta configurada em IConfiguration
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Authentication:SecretKey"]
                ?? throw new ArgumentNullException(nameof(_configuration))
            ));

            // Define as credenciais de assinatura usando a chave de segurança e o algoritmo HmacSha256
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Cria uma lista de reivindicações (claims) para adicionar ao token JWT
            var claims = new List<Claim>();
            claims.Add(new Claim("sub", user.UserId.ToString()));
            claims.Add(new Claim("given_name", user.Name));

            // Cria um token JWT com as informações fornecidas
            var jwt = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
            );

            // Gera o token JWT como uma string
            var jwtToReturn = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Retorna uma resposta HTTP 200 (OK) com o token JWT
            return Ok(jwtToReturn);
        }

        // Método para validar as credenciais do usuário
        private InfoUser? ValidateUserCredentials(string userName, string password)
        {
            // Simula uma consulta ao banco de dados para obter o usuário com as credenciais fornecidas
            var userFromDatabase = new Entities.User
            {
                Id = 1,
                Name = "Ada Lovelace",
                Username = "love",
                Password = "MinhaSenha"
            };

            // Verifica se as credenciais do usuário correspondem às credenciais fornecidas
            if (userFromDatabase.Username == userName && userFromDatabase.Password == password)
            {
                // Retorna as informações do usuário como um objeto InfoUser
                return new InfoUser(userFromDatabase.Id, userName, userFromDatabase.Name);
            }

            // Retorna nulo se as credenciais forem inválidas
            return null;
        }

        // Classe interna para representar as informações do usuário
        private class InfoUser
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }

            public InfoUser(int userId, string userName, string name)
            {
                // Armazena as informações do usuário nas propriedades da classe
                UserId = userId;
                Username = userName;
                Name = name;
            }
        }
    }
}
