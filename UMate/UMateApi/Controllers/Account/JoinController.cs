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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UMateModel.Contexts;
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
        private readonly ApplicationDbContext _context;

        public JoinController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
            _context = context;
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
                    expires: DateTime.UtcNow.AddYears(1),
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
                RealName = data.RealName,
                NickName = data.NickName,
                UniversityId = data.UniversityId,
                YearOfAdmission = data.YearOfAdmission,
                EmailConfirmed = true,
                UpdateDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, data.Password);

            var targetUniversity = await _context.University
                .Where(u => u.UniversityId == data.UniversityId)
                .FirstOrDefaultAsync();
            if (targetUniversity == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "일치하는 대학교 정보를 찾을 수 없습니다."
                });
            }

            if (result.Succeeded)
            {
                return Ok(new JoinResponse
                {
                    Code = ResultCode.Ok,
                    UserId = user.Id,
                    Email = user.Email,
                    Token = GetApiToken(user),
                    RealName = data.RealName,
                    NickName = data.NickName,
                    UniversityName = targetUniversity.Name,
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