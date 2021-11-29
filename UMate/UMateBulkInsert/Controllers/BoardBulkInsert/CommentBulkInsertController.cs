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
    [Route("bi/comment")]
    [ApiController]
    public class CommentBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }


        // 댓글 벌크 인서트
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommentPostResponse>> PostComment(CommentPostData comment)
        {
            // 새로운 댓글 
            var newComment = new Comment
            {
                UserId = comment.UserId,
                PostId = comment.PostId,
                Content = comment.Content,
                OriginalCommentId = comment.OriginalCommentId,
                IsReComment = comment.IsReComment,
                CreatedAt = comment.CreatedAt
            };

            _context.Comment.Add(newComment);
            await _context.SaveChangesAsync();

            return Ok(new CommentPostResponse
            {
                Code = ResultCode.Ok,
                Comment = new CommentDto(newComment)
            });
        }


    }
}

