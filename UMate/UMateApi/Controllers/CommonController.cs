using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UMateModel.Models;

namespace UMateApi.Controllers
{
    public class CommonController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public CommonController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 새로운 Api 토큰을 생성합니다.
        /// </summary>
        /// <param name="user"> 유저 정보를 담은 객체 </param>
        /// <returns> serialize된 토큰 </returns>
        public string GetApiToken(ApplicationUser user)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    Configuration["JwtIssuer"],
                    Configuration["JwtAudience"],
                    claims,
                    expires: DateTime.UtcNow.AddMonths(2),
                    signingCredentials: creds
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return "fail";
            }
        }
    }
}