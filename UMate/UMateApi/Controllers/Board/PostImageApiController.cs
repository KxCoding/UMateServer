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
    [Route("api/image")]
    [ApiController]
    public class PostImageApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostImageApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PostImageApi
        [HttpGet]
        public async Task<ActionResult<ImageListResponse<PostImage>>> GetPostImages (int postId)
        {
            var list = await _context.PostImage
                .Where(p => p.PostId == postId)
                .ToListAsync();

            return Ok(new ImageListResponse
            {
                Code = ResultCode.Ok,
                List = list
            });
        }

        // GET: api/PostImageApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostImage>> GetPostImage(int id)
        {
            var postImage = await _context.PostImage.FindAsync(id);

            if (postImage == null)
            {
                return NotFound();
            }

            return postImage;
        }

        // PUT: api/PostImageApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostImage(int id, PostImage postImage)
        {
            if (id != postImage.PostImageId)
            {
                return BadRequest();
            }

            _context.Entry(postImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }





        // POST: api/PostImageApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SaveImageResponse>> PostPostImage(PostImage postImage)
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





        // DELETE: api/PostImageApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommonResponse>> DeletePostImage(int id)
        {
            var postImage = await _context.PostImage.FindAsync(id);
            if (postImage == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "존재하지 않는 이미지"
                });
            }

            _context.PostImage.Remove(postImage);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok,
                Message = "삭제 성공"
            });
        }

        private bool PostImageExists(int id)
        {
            return _context.PostImage.Any(e => e.PostImageId == id);
        }
    }
}
