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
    [Route("bi/professor")]
    [ApiController]
    public class ProfessorBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfessorBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 교수명 벌크 인서트
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Professor>> PostProfessor(Professor professor)
        {
            // 이미 존재하는 교수명이 있는지 확인한다.
            var existingProfessor = await _context.Professor
                .Where(p => p.Name == professor.Name)
                .FirstOrDefaultAsync();

            // 이미 존재한다면 저장하지 않는다.
            if (existingProfessor != null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.ProfessorExists
                });
            }

            // 존재하지 않는다면 저장한다.
            _context.Professor.Add(professor);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.ProfessorExists
            });
        }

        
    }
}
