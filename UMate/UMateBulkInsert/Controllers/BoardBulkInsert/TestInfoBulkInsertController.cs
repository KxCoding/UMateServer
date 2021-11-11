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
    [Route("bi/testInfo")]
    [ApiController]
    public class TestInfoBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestInfoBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }


        // POST: api/TestInfoApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TestInfoPostResponse>> PostTestInfo(TestInfoPostData testInfo)
        {
            // 이미 시험정보를 등록한 강의인지 확인
            var existingTestInfo = await _context.TestInfo
                .Where(l => l.UserId == testInfo.UserId)// 강의평 중에 같은 사용자가 남긴 강의평 확인
                .Where(l => l.LectureInfoId == testInfo.LectureInfoId)// 사용자가 남긴 강의평중에 같은 강의정보가 있는지 확인
                .FirstOrDefaultAsync();


            // 같은 userId를 가진 시험정보가 이미 존재한다면 저장 안 되도록
            if (existingTestInfo != null)
            {
                return Ok(new LectureReviewPostResponse
                {
                    Code = ResultCode.LectureReviewExists,
                    Message = "이미 시험정보를 작성한 강의입니다."
                });
            }


            // 사용자가 시험정보를 남기지 않은 강의라면 서버에 저장
            var newTestInfo = new TestInfo
            {
                UserId = testInfo.UserId,
                LectureInfoId = testInfo.LectureInfoId,

                Semester = testInfo.Semester,
                TestType = testInfo.TestType,
                TestStrategy = testInfo.TestStrategy,
                QuestionTypes = testInfo.QuestionTypes
            };

            _context.TestInfo.Add(newTestInfo);
            await _context.SaveChangesAsync();


            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok
            });
        }

    }
}
