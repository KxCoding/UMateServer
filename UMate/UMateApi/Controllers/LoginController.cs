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
                            Token = token
                        });
                    }
                }
            }

            return Ok(new LoginResponse
            {
                Code = ResultCode.Fail
            });
        }

        [HttpPost("sso")]
        public async Task<IActionResult> PostSSO(SocialLoginPostData data)
        {
            var result = await _signInManager.ExternalLoginSignInAsync(data.Provider, data.Id, false);
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
                            Token = token
                        });
                    }
                }
            }

            var newUser = new ApplicationUser
            {
                UserName = data.Email,
                Email = data.Email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (createResult.Succeeded)
            {
                var loginInfo = new UserLoginInfo(data.Provider, data.Id, data.Email);
                var addResult = await _userManager.AddLoginAsync(newUser, loginInfo);

                if (addResult.Succeeded)
                {
                    return Ok(new LoginResponse
                    {
                        Code = ResultCode.Ok,
                        Message = "join & login success",
                        UserId = newUser.Id,
                        Token = GetApiToken(newUser)
                    });
                }
            }

            return Ok(new LoginResponse
            {
                Code = ResultCode.Fail,
                Message = createResult.Errors.First().Description
            });
        }
    }
}