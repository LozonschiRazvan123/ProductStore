using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductStore.Framework.Configuration;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class CreateJWT
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;

        public CreateJWT(UserManager<User> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public string CreateJwt(User user)
        {
            /*var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = _configuration.GetSection("JwtSettings:Token").Value;

            var encodedJWTKey = Encoding.UTF8.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedJWTKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var roles = _userManager.GetRolesAsync(user).Result.ToList();

            roles.ForEach(role =>
            {
                tokenDescriptor.Subject.AddClaim(new Claim("roles", role));
            });

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var writtenToken = tokenHandler.WriteToken(token);

            return writtenToken;*/
            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Token));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}
