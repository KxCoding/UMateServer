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
    [Route("bi/boardPost")]
    [ApiController]
    public class PostBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }


        // 게시물 벌크 인서트
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PostPostResponse>> PostPost(PostPostData post)
        {
            // Post는 중복을 제한하지 말아야한다. 사용자가 작성하는 건 다 올라가야 하므로
            // 존재 유무를 확인하지 않고 모두 서버에 저장
            var newPost = new Post
            {
                UserId = post.UserId,
                BoardId = post.BoardId,
                Title = post.Title,
                Content = post.Content,
                LikeCnt = post.LikeCnt,
                CommentCnt = post.CommentCnt,
                ScrapCnt = post.ScrapCnt,
                CategoryNumber = post.CategoryNumber,

                CreatedAt = post.CreatedAt
            };

            _context.Post.Add(newPost);
            await _context.SaveChangesAsync();

            return Ok(new PostPostResponse
            {
                Code = ResultCode.Ok,
                Post = newPost
            });
        }

    }
}
