using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UMateModel.Entities;
using UMateModel.Contexts;
using UMateModel.Models;
using UMateModel.Entities.Common;
using UMateModel.Models.UMatePlace;

namespace UMateBulkInsert.Controllers
{
    [Route("bi/place/univ")]
    [ApiController]
    public class UniversityBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UniversityBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/PlaceApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommonResponse>> PostPost(University university)
        {
            // 대학교 검색
            var existingUniversity = _context.University
                .Where(u => u.Name == university.Name)
                .FirstOrDefault();

            // 이미 등록되어 있다면 return
            if (existingUniversity != null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.ExistsAlready,
                    Message = "request data(place) already exists"
                });
            }

            // 등록이 안되어 있는 대학이라면 새로 등록
            else
            {
                _context.University.Add(university);
                await _context.SaveChangesAsync();

                return Ok(new UniversityPostResponse
                {
                    Code = ResultCode.Ok,
                    Message = "success inserting university"
                });
            }
        }
    }
}
