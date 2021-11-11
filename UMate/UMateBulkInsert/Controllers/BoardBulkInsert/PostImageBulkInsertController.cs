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
    [Route("bi/image")]
    [ApiController]
    public class PostImageBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostImageBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/PostImageApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ImagePostResponse>> PostPostImage(PostImage postImage)
        {
            var existingPost = await _context.Post
                .Where(p => p.PostId == postImage.PostId)
                .FirstOrDefaultAsync();


            if (existingPost == null)
            {
                return Ok(new ImagePostResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "존재하는 게시글이 없습니다."
                });
            }


            _context.PostImage.Add(postImage);
            await _context.SaveChangesAsync();

            return Ok(new ImagePostResponse
            {
                Code = ResultCode.Ok,
                Image = postImage
            });
        }

    }
}
