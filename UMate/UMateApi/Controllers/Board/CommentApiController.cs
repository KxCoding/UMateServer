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
    [Route("api/comment")]
    [ApiController]
    public class CommentApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CommentApi
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


        // GET: api/CommentApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comment.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }


        // POST: api/CommentApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommentPostResponse>> PostComment(CommentPostData comment)
        {
            var newComment = new Comment
            {
                UserId = comment.UserId,
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

            var user = await _context.Users
                .Where(u => u.Id == comment.UserId)
                .FirstOrDefaultAsync();

            var userComment = new CommentDto(newComment);
            userComment.UserName = user.UserName;
            userComment.ProfileUrl = user.SelectedProfileImage;

            return Ok(new CommentPostResponse
            {
                Code = ResultCode.Ok,
                Comment = userComment
            });
        }



        // DELETE: api/CommentApi/5
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