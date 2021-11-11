using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities.UMateBoard;
using UMateModel.Models;
using UMateModel.Models.UMateBoard;

namespace BoardApi.Controllers
{
    [Route("api/lectureInfo")]
    [ApiController]
    public class LectureInfoApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LectureInfoApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LectureInfoApi
        [HttpGet]
        public async Task<ActionResult<LectureInfoListResponse<LectureInfoListDto>>> GetLectureInfo(int page = 1, int pageSize = 12)
        {
            // 최근 강의평 목록
            var list = await _context.LectureInfo
                .Include(l => l.Professor)
                .Include(l => l.LectureReviews)
                .Select(l => new LectureInfoListDto(l))// 여러개를 불러올 때는 Dto사용
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

        // GET: api/LectureInfoApi/5
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

        // PUT: api/LectureInfoApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLectureInfo(int id, LectureInfo lectureInfo)
        {
            if (id != lectureInfo.LectureInfoId)
            {
                return BadRequest();
            }

            _context.Entry(lectureInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LectureInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LectureInfoApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LectureInfoPostResponse>> PostLectureInfo(LectureInfoPostData lectureInfo)
        {
            // 메모리에 같은 교수명이 존재하는지 확인
            var existingProfessor = await _context.Professor
                .Where(p => p.Name == lectureInfo.Professor)
                .FirstOrDefaultAsync();

            // 존재하지 않는다면
            if (existingProfessor == null)
            {
                existingProfessor = new Professor
                {
                    Name = lectureInfo.Professor
                };

                // 서버에 새로운 교수명 저장
                _context.Professor.Add(existingProfessor);
                await _context.SaveChangesAsync();
            }

            // 메모리에 같은 강의정보가 존재하는지 확인
            var existingLectureInfo = await _context.LectureInfo
                .Where(l => l.Title == lectureInfo.Title)
                .FirstOrDefaultAsync();

            // 강의 정보가 존재한다면 이미 저장되있다면
            if (existingLectureInfo != null)
            {
                // 강의 정보에 교수Id 저장해주기
                existingLectureInfo.ProfessorId = existingProfessor.ProfessorId;
                await _context.SaveChangesAsync();

                return Ok(new LectureInfoPostResponse
                {
                    Code = ResultCode.LectureInfoExists,
                    Message = "강의가 이미 존재합니다."
                });
            }

            // 강의 정보가 메모리에 존재하지 않는다면 서버에 저장
            var newLectureInfo = new LectureInfo
            {
                Title = lectureInfo.Title,
                BookName = lectureInfo.BookName,
                BookLink = lectureInfo.BookLink,
                Semesters = lectureInfo.Semesters,
                ProfessorId = existingProfessor.ProfessorId
            };

            _context.LectureInfo.Add(newLectureInfo);
            await _context.SaveChangesAsync();

            return Ok(new LectureInfoPostResponse
            {
                Code = ResultCode.Ok,
                LectureInfo = newLectureInfo
            });
        }

        // DELETE: api/LectureInfoApi/5
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
