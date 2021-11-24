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

namespace BoardServer.Controllers
{
    [Authorize]
    [Route("api/likePost")]
    [ApiController]
    public class LikePostApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LikePostApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LikePostApi
        [HttpGet]
        public async Task<ActionResult<LikePostListResponse<LikePost>>> GetLikePost(string userId)
        {
            var list = await _context.LikePost
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return Ok(new LikePostListResponse
            {
                Code = ResultCode.Ok,
                List = list
            });
        }


        // POST: api/LikePostApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LikePost>> PostLikePost(LikePost likePost)
        {
            var existingLikePost = await _context.LikePost
                .Where(l => l.UserId == likePost.UserId)
                .Where(l => l.PostId == likePost.PostId)
                .FirstOrDefaultAsync();

            if (existingLikePost != null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "이미 좋아요를 누른 게p글입니다."
                });
            }

            var post = await _context.Post
                .Where(c => c.PostId == likePost.PostId)
                .FirstOrDefaultAsync();

            post.LikeCnt += 1;

            _context.LikePost.Add(likePost);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok,
                Message = "추가 성공"
            });
        }

        // DELETE: api/LikePostApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LikePost>> DeleteLikePost(int id)
        {
            var likePost = await _context.LikePost.FindAsync(id);
            if (likePost == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.NotFound
                });
            }

            var post = await _context.Post
               .Where(p => p.PostId == likePost.PostId)
               .FirstOrDefaultAsync();

            post.LikeCnt -= 1;
            _context.LikePost.Remove(likePost);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok
            });
        }

        private bool LikePostExists(int id)
        {
            return _context.LikePost.Any(e => e.LikePostId == id);
        }
    }
}
