using HolbertonExample.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HolbertonExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private string? secretKey;
        public UserController(IConfiguration configuration)
        {
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        //
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LogInRequestDto logInRequestDTO)
        {
            var token = await GetAccessToken(logInRequestDTO);
            if (token==null)
            {
                return BadRequest("incorrect credentials");
            }

            return Ok(token);
        }

        [NonAction]
        private async Task<string>? GetAccessToken(LogInRequestDto logInRequestDTO)
        {
            // check user
            bool userExists = StaticDetails.userCredentials.ContainsKey(logInRequestDTO.UserName) && StaticDetails.userCredentials[logInRequestDTO.UserName] == logInRequestDTO.Password;

            if (!userExists)
            {
                return null;
            }


            // if user exists            
            var tokenHandler = new JwtSecurityTokenHandler();

            // this line converts secret key to bytes and we'll have that as byte array in the variable => key
            var key = Encoding.ASCII.GetBytes(secretKey);

            // tokenDescriptor basically contains everything like what are all the claims in a token
            // Claim - this will basically identify that this is name of the user, this is the role that you have,    
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, logInRequestDTO.UserName.ToString()),
                        new Claim(ClaimTypes.Role, StaticDetails.userRoles[logInRequestDTO.UserName]),
                    }),
                Expires = DateTime.UtcNow.AddDays(7),

                // finally we create symmetric security key with our key variable 
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            // generating token 
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor); // after adding identity I extracted role from token itself

            //the line below will generate the token that we'll finally use
            var token = tokenHandler.WriteToken(securityToken);

            return token;   
        }
    }
}
