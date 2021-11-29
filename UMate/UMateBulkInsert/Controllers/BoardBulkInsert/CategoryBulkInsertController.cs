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
    [Route("bi/category")]
    [ApiController]
    public class CategoryBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        // 게시판 카테고리 벌크 인서트
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CategoryPostResponse>> PostCategory(CategoryPostData category)
        {
            // 카테고리가 존재하는지 확인
            var existingCategory = await _context.Category
                .Where(c => c.Name == category.Name)
                .FirstOrDefaultAsync();

            // 카테고리가 이미 존재한다면
            if (existingCategory != null)
            {
                return Ok(new CategoryPostResponse
                {
                    Code = ResultCode.CategoryExists,
                    Message = "이미 존재하는 카테고리입니다."
                });
            }

            // 없다면 새로운 카테고리 정보 저장
            var newCategory = new Category
            {
                BoardId = category.BoardId,
                Name = category.Name
            };

            _context.Category.Add(newCategory);
            await _context.SaveChangesAsync();

            return Ok(new CategoryPostResponse
            {
                Code = ResultCode.Ok,
                Category = newCategory
            });
        }

      
    }
}
