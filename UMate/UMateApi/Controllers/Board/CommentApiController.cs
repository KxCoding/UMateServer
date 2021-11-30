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
    [Route("api/comment")]
    [ApiController]
    public class CommentApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IConfiguration Configuration { get; }

        public CommentApiController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            Configuration = configuration;
            _context = context;
        }

        // 댓글 목록을 불러옵니다.
        [HttpGet]
        public async Task<ActionResult<CommentListResponse<CommentDto>>> GetCommentList(int postId)
        {
            var comments = await _context.Comment
                .Where(c => c.PostId == postId)
                .Select(c => new CommentDto(c))
                .ToListAsync();

            foreach (CommentDto comment in comments)
            {
                var user = await _context.Users
                    .Where(u => u.Id == comment.UserId)
                    .FirstOrDefaultAsync();

                comment.UserName = user.UserName;
                comment.ProfileUrl = user.SelectedProfileImage;
            }

            var lastId = await _context.Comment
                .Select(c => c.CommentId)
                .MaxAsync();

            return Ok(new CommentListResponse
            {
                LastId = lastId,
                Code = ResultCode.Ok,
                List = comments
            });
        }


        // 댓글을 저장합니다.
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommentPostResponse>> PostComment(CommentPostData comment)
        {
            var loginedUser = await _userManager.GetUserAsync(User);
            var newComment = new Comment
            {
                UserId = loginedUser.Id,
                PostId = comment.PostId,
                Content = comment.Content,
                OriginalCommentId = comment.OriginalCommentId,
                IsReComment = comment.IsReComment,
                CreatedAt = comment.CreatedAt
            };

            var post = await _context.Post
                .Where(p => p.PostId == comment.PostId)
                .FirstOrDefaultAsync();

            post.CommentCnt += 1;

            _context.Comment.Add(newComment);
            await _context.SaveChangesAsync();

            if (newComment.IsReComment == false)
            {
                newComment.OriginalCommentId = newComment.CommentId;
            }

            
            await _context.SaveChangesAsync();

            var userComment = new CommentDto(newComment);
            userComment.UserName = loginedUser.UserName;
            userComment.ProfileUrl = loginedUser.SelectedProfileImage;

            return Ok(new CommentPostResponse
            {
                Code = ResultCode.Ok,
                Comment = userComment
            });
        }


        // 댓글을 삭제합니다.
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommonResponse>> DeleteComment(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "존재하지 않는 댓글"
                });
            }

            var post = await _context.Post
                .Where(p => p.PostId == comment.PostId)
                .FirstOrDefaultAsync();

            post.CommentCnt -= 1;

            var likeComments = await _context.LikeComment
                .Where(l => l.CommentId == id)
                .ToListAsync();

            foreach (LikeComment likeComment in likeComments)
            {
                _context.LikeComment.Remove(likeComment);
            }
            

            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok,
                Message = "삭제 성공"
            });
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }
    }
}