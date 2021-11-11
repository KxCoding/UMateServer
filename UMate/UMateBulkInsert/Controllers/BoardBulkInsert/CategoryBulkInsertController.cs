using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoardModel.Models;
using BoardModel.Contexts;

namespace BoardBulkInsert.Controllers
{
    [Route("bi/category")]
    [ApiController]
    public class CategoryBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        // POST: api/CategoryApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CategoryPostResponse>> PostCategory(CategoryPostData category)
        {
            var existingCategory = await _context.Category
                .Where(c => c.Name == category.Name)
                .FirstOrDefaultAsync();

            if (existingCategory != null)
            {
                return Ok(new CategoryPostResponse
                {
                    ResultCode = ResultCode.CategoryExists,
                    Message = "이미 존재하는 카테고리입니다."
                });
            }

            var newCategory = new Category
            {
                BoardId = category.BoardId,
                Name = category.Name
            };

            _context.Category.Add(newCategory);
            await _context.SaveChangesAsync();

            return Ok(new CategoryPostResponse
            {
                ResultCode = ResultCode.Ok,
                Category = newCategory
            });
        }

      
    }
}
