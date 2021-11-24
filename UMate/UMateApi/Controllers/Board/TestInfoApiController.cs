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
    [Route("api/testInfo")]
    [ApiController]
    public class TestInfoApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestInfoApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TestInfoApi
        [HttpGet]
        public async Task<ActionResult<TestInfoListResponse<TestInfo>>> GetTestInfo(int lectureInfoId)
        {
            var existingLectureInfo = await _context.LectureInfo
                            .Where(l => l.LectureInfoId == lectureInfoId)
                            .FirstOrDefaultAsync();

            if (existingLectureInfo == null)
            {
                return Ok(new TestInfoListResponse
                {
                    Code = ResultCode.Fail,
                    Message = "등록된 강의정보가 없습니다."
                });
            }

            var testInfos = await _context.TestInfo
                .Include(t => t.Examples)
                .Where(t => t.LectureInfoId == lectureInfoId)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();

            return Ok(new TestInfoListResponse
            {
                TestInfos = testInfos,
                Code = ResultCode.Ok
            });
        }


        // POST: api/TestInfoApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TestInfoPostResponse>> PostTestInfo(SaveTestInfoData testInfo)
        {
            // 이미 시험정보를 등록한 강의인지 확인
            var existingTestInfo = await _context.TestInfo
                .Include(l => l.Examples)
                .Where(l => l.UserId == testInfo.UserId)// 강의평 중에 같은 사용자가 남긴 강의평 확인
                .Where(l => l.LectureInfoId == testInfo.LectureInfoId)// 사용자가 남긴 강의평중에 같은 강의정보가 있는지 확인
                .FirstOrDefaultAsync();


            // 같은 userId를 가진 시험정보가 이미 존재한다면 저장 안 되도록
            if (existingTestInfo != null)
            {
                return Ok(new TestInfoPostResponse
                {
                    Code = ResultCode.testInfoExists,
                    TestInfo = new TestInfoDto(existingTestInfo),
                    Examples = new List<Example>(),
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
                QuestionTypes = testInfo.QuestionTypes,

                CreatedAt = testInfo.CreatedAt
            };

            _context.TestInfo.Add(newTestInfo);
            await _context.SaveChangesAsync();

            var examples = new List<Example>();
            foreach (string example in testInfo.Examples)
            {
                var newExample = new Example
                {
                    TestInfoId = newTestInfo.TestInfoId,
                    Content = example
                };

                _context.Example.Add(newExample);
                await _context.SaveChangesAsync();

                examples.Add(newExample);
            }

            return Ok(new TestInfoPostResponse
            {
                Code = ResultCode.Ok,
                TestInfo = new TestInfoDto(newTestInfo),
                Examples = examples
            });
        }

        // DELETE: api/TestInfoApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommonResponse>> DeleteTestInfo(int id)
        {
            var testInfo = await _context.TestInfo.FindAsync(id);
            if (testInfo == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "존재하지 않는 시험정보"
                });
            }

            var examples = await _context.Example
                .Where(e => e.TestInfoId == id)
                .ToListAsync();

            foreach (Example example in examples)
            {
                _context.Example.Remove(example);
            }

            _context.TestInfo.Remove(testInfo);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok,
                Message = "삭제 성공"
            });
        }

        private bool TestInfoExists(int id)
        {
            return _context.TestInfo.Any(e => e.TestInfoId == id);
        }
    }
}
