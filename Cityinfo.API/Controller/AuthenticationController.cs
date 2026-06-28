using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cityinfo.API.Controller
{
    [Route("api/authentication")]
    [ApiController]

    public class AuthenticationController : ControllerBase
    {
        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        public class CityInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }

            public CityInfoUser(int UserId, string UserName, string FirstName, string LastName, string City)
            {
                this.UserId = UserId;
                this.UserName = UserName;
                this.FirstName = FirstName;
                this.LastName = LastName;
                this.City = City;
            }
        }

        IConfiguration _configuration;
        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("Authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody
            authenticationRequestBody)
        {
            var user = ValidateUserCredentials(authenticationRequestBody.UserName,
                authenticationRequestBody.Password);

            if (user == null)
            {
               return Unauthorized();
            }

            var securitykey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:SecretForKey"])
                );
            var signingCredentials = new SigningCredentials(
              securitykey, SecurityAlgorithms.HmacSha256
                );
            var ClaimsFortoken = new List<Claim>();
            ClaimsFortoken.Add(new Claim("UserId", user.UserId.ToString()));
            ClaimsFortoken.Add(new Claim("NameKarbari", user.FirstName.ToString()));
            ClaimsFortoken.Add(new Claim(ClaimTypes.Email, user.City.ToString()));

            var jwtSecurityToken = new JwtSecurityToken(
               _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                ClaimsFortoken,
                DateTime.UtcNow,
                  DateTime.UtcNow.AddHours(1),
                  signingCredentials
                );
            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);
            return Ok(tokenToReturn);


        }

        private CityInfoUser ValidateUserCredentials(string? userName,
            string? password)
        {
            return new CityInfoUser(1, userName ?? "",
                "Ali",
                "Fani",
                "Ahwaz");
        }
    }

}

