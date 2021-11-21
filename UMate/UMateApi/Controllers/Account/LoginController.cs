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
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IConfiguration Configuration { get; }

        public LoginController(
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

        [HttpPost("email")]
        public async Task<IActionResult> PostEmail(EmailLoginPostData data)
        {
            var result = await _signInManager.PasswordSignInAsync(data.Email, data.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(data.Email);

                if (user != null)
                {
                    var token = GetApiToken(user);
                    if (!token.Contains("fail"))
                    {
                        return Ok(new LoginResponse
                        {
                            Code = ResultCode.Ok,
                            UserId = user.Id,
                            Email = user.Email,
                            Token = token,
                            UserName = user.UserName,
                            NickName = user.NickName,
                            YearOfAdmission = user.YearOfAdmission
                        });
                    } else
                    {
                        return Ok(new LoginResponse
                        {
                            Code = ResultCode.Fail,
                            Message = "토큰 생성 실패"
                        });
                    }
                } else
                {
                    return Ok(new LoginResponse
                    {
                        Code = ResultCode.Fail,
                        Message = "사용자를 찾을 수 없습니다."
                    });
                }
            }

            return Ok(new LoginResponse
            {
                Code = ResultCode.Fail,
                Message = "로그인 정보를 확인해주세요."
            });
        }
    }
}