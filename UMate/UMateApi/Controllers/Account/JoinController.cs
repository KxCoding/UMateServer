using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UMateModel.Models;

namespace UMateApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JoinController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IConfiguration Configuration { get; }

        public JoinController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
        }

        private string GetApiToken(ApplicationUser user)
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
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: creds
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return "fail";
            }
        }

        [HttpPost("email")]
        public async Task<IActionResult> PostEmail(EmailJoinPostData data)
        {
            var user = new ApplicationUser
            {
                UserName = data.Email,
                Email = data.Email,
                NickName = data.NickName,
                YearOfAdmission = data.YearOfAdmission,
                EmailConfirmed = true,
                UpdateDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
            {
                return Ok(new JoinResponse
                {
                    Code = ResultCode.Ok,
                    UserId = user.Id,
                    Email = user.Email,
                    Token = GetApiToken(user),
                    UserName = data.UserName,
                    NickName = data.NickName,
                    YearOfAdmission = data.YearOfAdmission
                });
            }

            return Ok(new JoinResponse
            {
                Code = ResultCode.Fail,
                Message = result.Errors.FirstOrDefault()?.Description
            });
        }
    }
}