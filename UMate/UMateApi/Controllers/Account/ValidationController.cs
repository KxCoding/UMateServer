using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UMateApi.Controllers.Account
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        /// <summary>
        /// 토큰이 유효한지 확인합니다.
        /// </summary>
        /// <returns> 서버 응답 코드 </returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}