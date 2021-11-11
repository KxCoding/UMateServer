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
    [Route("api/lectureReview")]
    [ApiController]
    public class LectureReviewApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LectureReviewApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LectureReviewApi
        [HttpGet]
        public async Task<ActionResult<LectureReviewListResponse<LectureReview>>> GetLectureReview(int lectureInfoId)
        {
            var existingLectureInfo = await _context.LectureInfo
                .Where(l => l.LectureInfoId == lectureInfoId)
                .FirstOrDefaultAsync();

            if (existingLectureInfo == null)
            {
                return Ok(new LectureReviewListResponse
                {
                    Code = ResultCode.Fail,
                    Message = "등록된 강의정보가 없습니다."
                });
            }

            var reviews = await _context.LectureReview
                .Where(l => l.LectureInfoId == lectureInfoId)
                .OrderBy(l => l.CreatedAt)
                .ToListAsync();

            return Ok(new LectureReviewListResponse
            {
                LectureReviews = reviews,
                Code = ResultCode.Ok
            });
        }





        // POST: api/LectureReviewApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LectureReviewPostResponse>> PostLectureReview(LectureReviewPostData lectureReview)
        {
            // 이미 강의평을 등록한 강의인지 확인
            var existingReview = await _context.LectureReview
                .Where(l => l.UserId == lectureReview.UserId)// 강의평 중에 같은 사용자가 남긴 강의평 확인
                .Where(l => l.LectureInfoId == lectureReview.LectureInfoId)// 사용자가 남긴 강의평중에 같은 강의정보가 있는지 확인
                .FirstOrDefaultAsync();


            // 같은 userId를 가진 강의평이 이미 존재한다면 저장 안 되도록
            if (existingReview != null)
            {
                return Ok(new LectureReviewPostResponse
                {
                    Code = ResultCode.LectureReviewExists,
                    LectureReview = existingReview,
                    Message = "이미 리뷰를 작성한 강의입니다."
                });
            }

            // 사용자가 강의평을 남기지 않은 강의라면 서버에 저장
            var newLectureReview = new LectureReview
            {
                UserId = lectureReview.UserId,
                LectureInfoId = lectureReview.LectureInfoId,
                Assignment = lectureReview.Assignment,
                GroupMeeting = lectureReview.GroupMeeting,
                Evaluation = lectureReview.Evaluation,
                Attendance = lectureReview.Attendance,
                TestNumber = lectureReview.TestNumber,
                Rating = lectureReview.Rating,

                Semester = lectureReview.Semester,
                Content = lectureReview.Content,

                CreatedAt = lectureReview.CreatedAt
            };

            _context.LectureReview.Add(newLectureReview);
            await _context.SaveChangesAsync();

            return Ok(new LectureReviewPostResponse
            {
                Code = ResultCode.Ok,
                LectureReview = newLectureReview
            });
        }




        // DELETE: api/LectureReviewApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LectureReview>> DeleteLectureReview(int id)
        {
            var lectureReview = await _context.LectureReview.FindAsync(id);
            if (lectureReview == null)
            {
                return NotFound();
            }

            _context.LectureReview.Remove(lectureReview);
            await _context.SaveChangesAsync();

            return lectureReview;
        }

        private bool LectureReviewExists(int id)
        {
            return _context.LectureReview.Any(e => e.LectureReviewId == id);
        }
    }
}
