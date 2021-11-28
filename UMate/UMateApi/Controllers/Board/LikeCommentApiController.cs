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
    [Route("api/likeComment")]
    [ApiController]
    public class LikeCommentApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IConfiguration Configuration { get; }

        public LikeCommentApiController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            Configuration = configuration;
            _context = context;
        }

        // 좋아요한 댓글 목록을 리턴합니다.
        [HttpGet]
        public async Task<ActionResult<LikeCommentListResponse<LikeComment>>> GetLikeComment()
        {
            var loginedUser = await _userManager.GetUserAsync(User);
            var list = await _context.LikeComment
                .Where(l => l.UserId == loginedUser.Id)
                .ToListAsync();

            return Ok(new LikeCommentListResponse
            {
                Code = ResultCode.Ok,
                List = list
            });
        }

        // 댓글 좋아요를 저장합니다.
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LikeComment>> PostLikeComment(LikeComment likeComment)
        {
            var loginedUser = await _userManager.GetUserAsync(User);
            var existingLikeComment = await _context.LikeComment
                .Where(l => l.CommentId == likeComment.CommentId)
                .FirstOrDefaultAsync();

            if (existingLikeComment != null)
            {
                return Ok(new SaveLikeCommentResponse
                {
                    Code = ResultCode.ExistsAlready,
                    Message = "이미 존재합니다."
                });
            }

            var comment = await _context.Comment
                .Where(c => c.CommentId == likeComment.CommentId)
                .FirstOrDefaultAsync();

            comment.LikeCnt += 1;

            likeComment.UserId = loginedUser.Id;
            _context.LikeComment.Add(likeComment);
            await _context.SaveChangesAsync();

            return Ok(new SaveLikeCommentResponse
            {
                LikeComment = likeComment,
                Code = ResultCode.Ok,
                Message = "추가 성공"
            });
        }

        // 댓글 좋아요를 삭제합니다.
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

