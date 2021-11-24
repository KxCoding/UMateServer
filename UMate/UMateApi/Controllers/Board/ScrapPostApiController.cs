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
    [Route("api/scrapPost")]
    [ApiController]
    public class ScrapPostApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ScrapPostApiController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/ScrapPostApi
        [HttpGet]
        public async Task<ActionResult<ScrapPostListResponse<PostDto>>> GetScrapPost(string userId)
        {
            var list = await _context.ScrapPost
                .Where(s => s.UserId == userId)
                .Include(s => s.Post)
                .Select(s => new PostListDto(s.Post))
                .ToListAsync();

            return Ok(new ScrapPostListResponse
            {
                Code = ResultCode.Ok,
                List = list
            });
        }

        // POST: api/ScrapPostApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ScrapPost>> PostScrapPost(ScrapPost scrapPost)
        {
            var existingScrapPost = await _context.ScrapPost
                .Where(l => l.UserId == scrapPost.UserId)
                .Where(l => l.PostId == scrapPost.PostId)
                .FirstOrDefaultAsync();

            if (existingScrapPost != null)
            {
                return Ok(new SaveScrapPostResponse
                {
                    Code = ResultCode.Fail,
                    Message = "이미 스크랩한 글입니다."
                });
            }

            var post = await _context.Post
                .Where(c => c.PostId == scrapPost.PostId)
                .FirstOrDefaultAsync();

            post.ScrapCnt += 1;

            _context.ScrapPost.Add(scrapPost);
            await _context.SaveChangesAsync();

            return Ok(new SaveScrapPostResponse
            {
                scrapPost = scrapPost,
                Code = ResultCode.Ok,
                Message = "추가 성공"
            });
        }

        // DELETE: api/ScrapPostApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ScrapPost>> DeleteScrapPost(int id)
        {
            var scrapPost = await _context.ScrapPost.FindAsync(id);
            if (scrapPost == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.NotFound
                });
            }

            var post = await _context.Post
               .Where(p => p.PostId == scrapPost.PostId)
               .FirstOrDefaultAsync();

            post.ScrapCnt -= 1;

            _context.ScrapPost.Remove(scrapPost);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok
            });
        }

        private bool ScrapPostExists(int id)
        {
            return _context.ScrapPost.Any(e => e.ScrapPostId == id);
        }
    }
}
