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
    [Route("bi/example")]
    [ApiController]
    public class ExampleBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExampleBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }


        // 강의정보에서 > 시험 정보에서 > 문제 예시 벌크 인서트
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ExamplePostResponse>> PostExample(ExamplePostData example)
        {
            // 시험 정보가 존재하는지 확인
            var existingTestInfo = await _context.TestInfo
                .Where(t => t.TestInfoId == example.TestInfoId)
                .FirstOrDefaultAsync();

            // 존재하는 시험정보가 없다면
            if (existingTestInfo == null)
            {
                // 문제 예시를 저장할 수 없다.
                return Ok(new ExamplePostResponse
                {
                    Code = ResultCode.Fail,
                    Message = "존재하지 않는 시험정보입니다."
                });
            }

            // 문제 예시와 관련된 시험 정보가 존재한다면 저장
            var newExample = new Example
            {
                TestInfoId = example.TestInfoId,
                Content = example.Content
            };

            _context.Example.Add(newExample);
            await _context.SaveChangesAsync();

            return Ok(new ExamplePostResponse
            {
                Code = ResultCode.Ok,
                Example = newExample
            });
        }
    }
}