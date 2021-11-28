using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UMateModel.Contexts;
using UMateModel.Entities.UMateBoard;
using UMateModel.Models;
using UMateModel.Models.UMateBoard;

namespace BoardApi.Controllers
{
    [Authorize]
    [Route("api/lectureInfo")]
    [ApiController]
    public class LectureInfoApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LectureInfoApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 강의 목록을 리턴합니다.
        [HttpGet]
        public async Task<ActionResult<LectureInfoListResponse<LectureInfoListDto>>> GetLectureInfo(int page = 1, int pageSize = 12)
        {
            // 최근 강의평 목록
            var list = await _context.LectureInfo
                .Include(l => l.Professor)
                .Include(l => l.LectureReviews)
                .Select(l => new LectureInfoListDto(l))
                .ToListAsync();

            var sortedList = list.OrderByDescending(l => l.ReviewId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
                
            var total = await _context.LectureInfo
                .CountAsync();

            return Ok(new LectureInfoListResponse
            {
                TotalCount = total,
                Code = ResultCode.Ok,
                List = sortedList
            });
        }

        // 세부 강의 정보 화면에 필요한 일부 정보를 리턴합니다.
        [HttpGet("{id}")]
        public async Task<ActionResult<LectureInfoDetailDto>> GetLectureInfo(int id)
        {
            var testInfoList = await _context.TestInfo
                .Where(t => t.LectureInfoId == id)
                .Include(t => t.Examples)
                .ToListAsync();

            var lectureInfo = await _context.LectureInfo
                .Where(l => l.LectureInfoId == id)
                .Include(l => l.Professor)
                .Include(l => l.LectureReviews)
                .FirstOrDefaultAsync();

            lectureInfo.TestInfos = testInfoList;

            if (lectureInfo == null)
            {
                return Ok(new LectureInfoDetailResponse
                {
                    Code = ResultCode.NotFound
                });
            }

            return Ok(new LectureInfoDetailResponse
            {
                Code = ResultCode.Ok,
                LectureInfo = new LectureInfoDetailDto(lectureInfo)
            });
        }


        // 강의 목록을 삭제합니다.
        [HttpDelete("{id}")]
        public async Task<ActionResult<LectureInfo>> DeleteLectureInfo(int id)
        {
            var lectureInfo = await _context.LectureInfo.FindAsync(id);
            if (lectureInfo == null)
            {
                return NotFound();
            }

            _context.LectureInfo.Remove(lectureInfo);
            await _context.SaveChangesAsync();

            return lectureInfo;
        }

        private bool LectureInfoExists(int id)
        {
            return _context.LectureInfo.Any(e => e.LectureInfoId == id);
        }
    }
}
