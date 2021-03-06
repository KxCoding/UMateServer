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
    public class LoginController : CommonController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public LoginController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IConfiguration configuration) : base(configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        /// 이메일과 비밀번호로 로그인합니다.
        /// </summary>
        /// <param name="data"> 로그인 정보를 담은 객체 </param>
        /// <returns> 로그인 응답 객체 </returns>
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
                        var targetUniversity = await _context.University
                            .Where(u => u.UniversityId == user.UniversityId)
                            .FirstOrDefaultAsync();
                        if (targetUniversity == null)
                        {
                            return Ok(new CommonResponse
                            {
                                Code = ResultCode.Fail,
                                Message = "일치하는 대학교 정보를 찾을 수 없습니다."
                            });
                        }

                        return Ok(new LoginResponse
                        {
                            Code = ResultCode.Ok,
                            UserId = user.Id,
                            Email = user.Email,
                            Token = token,
                            RealName = user.RealName,
                            NickName = user.NickName,
                            UniversityName = targetUniversity.Name,
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