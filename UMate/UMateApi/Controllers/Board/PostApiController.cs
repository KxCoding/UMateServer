﻿using System;
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
    [Route("api/boardPost")]
    [ApiController]
    public class PostApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 게시글 목록
        // GET: api/PostApi
        [HttpGet]
        public async Task<ActionResult<PostListResponse<PostListDto>>> GetPostList(int boardId, string userId)
        {

            if (boardId == 1)
            {
                var myPostList = await _context.Post
                    .Where(p => p.UserId == userId)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new PostListDto(p))
                    .ToListAsync();

                return Ok(new MyPostListResponse
                {
                    TotalCount = myPostList.Count,
                    Code = ResultCode.Ok,
                    List = myPostList
                });
            }

            if (boardId == 2)
            {
                var myCommentPostList = await _context.Comment
                              .Where(c => c.UserId == userId)
                              .OrderByDescending(c => c.CreatedAt)
                              .Include(c => c.Post)
                              .Select(c => new PostListDto(c.Post))
                              .Distinct()
                              .ToListAsync();

                return Ok(new MyCommentListResponse
                {
                    TotalCount = myCommentPostList.Count,
                    Code = ResultCode.Ok,
                    List = myCommentPostList
                });
            }

            if (boardId == 3)
            {
                var scrapList = await _context.ScrapPost
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .Include(s => s.Post)
                .Select(s => new PostListDto(s.Post))
                .Distinct()
                .ToListAsync();

                return Ok(new ScrapPostListResponse
                {
                    TotalCount = scrapList.Count,
                    Code = ResultCode.Ok,
                    List = scrapList
                });
            }

            if (boardId == 4)
            {
                var popularPostList = await _context.Post
                    .Where(p => p.LikeCnt >= 20 || p.ScrapCnt >= 20)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new PostListDto(p))
                    .Distinct()
                    .ToListAsync();

                return Ok(new ScrapPostListResponse
                {
                    TotalCount = popularPostList.Count,
                    Code = ResultCode.Ok,
                    List = popularPostList
                });
            }

            var posts = await _context.Post
                .Where(p => p.BoardId == boardId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostListDto(p))
                .ToListAsync();

            return Ok(new PostListResponse
            {
                TotalCount = posts.Count,
                Code = ResultCode.Ok,
                List = posts
            });
        }


        // GET: api/PostApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id, string userId)
        {
            var likePost = await _context.LikePost
                .Where(l => l.PostId == id)
                .Where(l => l.UserId == userId)
                .Distinct()
                .FirstOrDefaultAsync();

            var isLiked = likePost != null;

            var scrapPost = await _context.ScrapPost
                .Where(s => s.PostId == id)
                .Where(s => s.UserId == userId)
                .Distinct()
                .FirstOrDefaultAsync();

            var isScrapped = scrapPost != null;
            var scrapPostId = 0;
            if (isScrapped)
            {
                scrapPostId = scrapPost.ScrapPostId;
            }

            var post = await _context.Post
                .Where(p => p.PostId == id)
                .Include(p => p.PostImages)
                .Select(p => new PostDto(p))
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return Ok(new PostResponse
                {
                    Code = ResultCode.NotFound
                });
            }

            return Ok(new PostResponse
            {
                Code = ResultCode.Ok,
                Post = post,
                isLiked = isLiked,
                isScrapped = isScrapped,
                scrapPostId = scrapPostId
            });
        }


        // POST: api/PostApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SavePostResponse>> PostPost(SavePostData post)
        {
            var existingBoard = await _context.Board
                .Where(b => b.BoardId == post.BoardId)
                .FirstOrDefaultAsync();


            if (existingBoard != null)
            {
                var newPost = new Post
                {
                    UserId = post.UserId,
                    BoardId = existingBoard.BoardId,
                    Title = post.Title,
                    Content = post.Content,
                    LikeCnt = 0,
                    CommentCnt = 0,
                    ScrapCnt = 0,
                    CategoryNumber = post.CategoryNumber,

                    CreatedAt = post.CreatedAt
                };

                _context.Post.Add(newPost);
                await _context.SaveChangesAsync();


                foreach (string urlStr in post.urlStrings)
                {
                    var newImage = new PostImage
                    {
                        PostId = newPost.PostId,
                        UrlString = urlStr
                    };

                    _context.PostImage.Add(newImage);
                    await _context.SaveChangesAsync();
                }
                

                return Ok(new SavePostResponse
                {
                    Code = ResultCode.Ok,
                    Post = new PostDto(newPost)
                });
            }

            return Ok(new SavePostResponse
            {
                Code = ResultCode.Fail,
                Message = "존재하지 않는 게시판이기에 게시물 작성이 불가합니다."
            });

        }

        // DELETE: api/PostApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommonResponse>> DeletePost(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "존재하지 않는 게시물"
                });
            }

            var images = await _context.PostImage
                .Where(p => p.PostId == id)
                .ToListAsync();

            foreach (PostImage image in images)
            {
                _context.PostImage.Remove(image);
            }

            var comments = await _context.Comment
                .Where(c => c.PostId == id)
                .ToListAsync();

            foreach (Comment comment in comments)
            {
                
                var likeComments = await _context.LikeComment
                    .Where(l => l.CommentId == comment.CommentId)
                    .ToListAsync();

                foreach (LikeComment likeComment in likeComments)
                {
                    _context.LikeComment.Remove(likeComment);
                }
                
                _context.Comment.Remove(comment);
            }

            var scraps = await _context.ScrapPost
                .Where(s => s.PostId == id)
                .ToListAsync();

            foreach (ScrapPost scrapPost in scraps)
            {
                _context.ScrapPost.Remove(scrapPost);
            }

            var likes = await _context.LikePost
                .Where(l => l.PostId == id)
                .ToListAsync();

            foreach (LikePost likePost in likes)
            {
                _context.LikePost.Remove(likePost);
            }


                _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok,
                Message = "삭제 성공"
            });
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
        }
    }
}