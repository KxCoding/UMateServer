using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities;
using UMateModel.Entities.Place;
using UMateModel.Models;
using UMateModel.Models.UMatePlace;
using UMateModel.Models.UMateUniversity;

namespace UMateApi.Controllers
{
    [Authorize]
    [Route("api/university")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UniversityController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: University
        // 전체 대학교 리스트 리턴
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UniversityListResponse>>> GetUniversityList()
        {
            var list = await _context.University
                .OrderBy(u => u.Name)
                .Select(u => new UniversityDto(u))
                .ToListAsync();

            var total = await _context.Place.CountAsync();

            return Ok(new UniversityListResponse
            {
                Code = ResultCode.Ok,
                TotalCount = total,
                Universities = list,
            });
        }

        // GET: University/5
        // 전달된 대학 id와 일치하는 대학 정보 리턴
        [HttpGet("{id}")]
        public async Task<ActionResult<UniversityResponse>> GetUniversity(int id)
        {
            var university = await _context.University
                //.Include(u => u.Places)
                .Where(u => u.UniversityId == id)
                .FirstOrDefaultAsync();

            if (university == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "can't find university"
                });
            }

            var data = new UniversityDto(university);

            return Ok(new UniversityResponse
            {
                Code = ResultCode.Ok,
                University = data
            });
        }

       
    }
}
