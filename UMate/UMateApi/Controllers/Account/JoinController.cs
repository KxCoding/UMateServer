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
    public class JoinController : CommonController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public JoinController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IConfiguration configuration) : base(configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
                JoinDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
            {
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