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
    [Route("bi/lectureInfo")]
    [ApiController]
    public class LectureInfoBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LectureInfoBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }
        

        // 강의 정보 벌크 인서트
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LectureInfoPostResponse>> PostLectureInfo(LectureInfoPostData lectureInfo)
        {
            // 메모리에 같은 교수명이 존재하는지 확인
            // ProfessorBulkInsertController가 있지만 다시 확인하는 작업입니다.
            var existingProfessor = await _context.Professor
                .Where(p => p.Name == lectureInfo.Professor)
                .FirstOrDefaultAsync();

            // 교수명이 존재하지 않는다면
            if (existingProfessor == null)
            {
                // 강의 정보에 해당하는 교수명을 새로 서버에 저장
                existingProfessor = new Professor
                {
                    Name = lectureInfo.Professor
                };

                _context.Professor.Add(existingProfessor);
                await _context.SaveChangesAsync();
            }

            // 같은 강의정보가 존재하는지 확인
            var existingLectureInfo = await _context.LectureInfo
                .Where(l => l.Title == lectureInfo.Title)
                .FirstOrDefaultAsync();

            // 강의 정보가 이미 저장되있다면
            if (existingLectureInfo != null)
            {
                // 강의 정보와 교수명을 저장해서 연결해 줍니다.
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

       
    }
}
