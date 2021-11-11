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

        // POST: api/ProfessorApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Professor>> PostProfessor(Professor professor)
        {
            var existingProfessor = await _context.Professor
                .Where(p => p.Name == professor.Name)
                .FirstOrDefaultAsync();

            if (existingProfessor != null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.ProfessorExists
                });
            }

            _context.Professor.Add(professor);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.ProfessorExists
            });
        }

        
    }
}
