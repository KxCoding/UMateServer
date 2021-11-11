using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMateModel.Models;
using UMateModel.Contexts;
using UMateModel.Models.UMateBoard;
using UMateModel.Entities.UMateBoard;

namespace BoardBulkInsert.Controllers
{
    [Route("bi/lectureReview")]
    [ApiController]
    public class LectureReviewBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LectureReviewBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
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

                CreatedAt = DateTime.UtcNow// 그냥 Now는 컴퓨터 위치시간으로 저장됨. 출력할 때는 .utf8??로 스트링??
            };

            _context.LectureReview.Add(newLectureReview);
            await _context.SaveChangesAsync();

            return Ok(new LectureReviewPostResponse
            {
                Code = ResultCode.Ok,
                LectureReview = newLectureReview
            });
        }
   
    }
}

