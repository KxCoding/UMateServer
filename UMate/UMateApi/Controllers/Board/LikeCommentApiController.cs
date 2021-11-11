using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities.UMateBoard;
using UMateModel.Models;
using UMateModel.Models.UMateBoard;

namespace BoardApi.Controllers
{
    [Route("api/likeComment")]
    [ApiController]
    public class LikeCommentApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LikeCommentApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LikeCommentApi
        [HttpGet]
        public async Task<ActionResult<LikeCommentListResponse<LikeComment>>> GetLikeComment(string userId)
        {
            var list = await _context.LikeComment
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return Ok(new LikeCommentListResponse
            {
                Code = ResultCode.Ok,
                List = list
            });
        }

        // POST: api/LikeCommentApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LikeComment>> PostLikeComment(LikeComment likeComment)
        {
            var comment = await _context.Comment
                .Where(c => c.CommentId == likeComment.CommentId)
                .FirstOrDefaultAsync();

            comment.LikeCnt += 1;

            _context.LikeComment.Add(likeComment);
            await _context.SaveChangesAsync();

            return Ok(new SaveLikeCommentResponse
            {
                LikeComment = likeComment,
                Code = ResultCode.Ok,
                Message = "추가 성공"
            });
        }

        // DELETE: api/LikeCommentApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LikeComment>> DeleteLikeComment(int id)
        {
            var likeComment = await _context.LikeComment.FindAsync(id);
            if (likeComment == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.NotFound
                });
            }

            var comment = await _context.Comment
               .Where(c => c.CommentId == likeComment.CommentId)
               .FirstOrDefaultAsync();

            comment.LikeCnt -= 1;
            _context.LikeComment.Remove(likeComment);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok
            });
        }

        private bool LikeCommentExists(int id)
        {
            return _context.LikeComment.Any(e => e.LikeCommentId == id);
        }
    }
}

