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
    [Route("api/boardPost")]
    [ApiController]
    public class PostApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IConfiguration Configuration { get; }

        public PostApiController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            Configuration = configuration;
            _context = context;
        }

        // 게시글 목록을 리턴합니다.
        [HttpGet]
        public async Task<ActionResult<PostListResponse<PostListDto>>> GetPostList(int boardId)
        {
            var loginedUser = await _userManager.GetUserAsync(User);
            // '내가 쓴 글' 게시판
            if (boardId == 1)
            {
                var myPostList = await _context.Post
                    .Where(p => p.UserId == loginedUser.Id)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new PostListDto(p))
                    .ToListAsync();

                foreach (PostListDto post in myPostList)
                {
                    var user = await _context.Users
                        .Where(u => u.Id == post.UserId)
                        .FirstOrDefaultAsync();

                    post.UserName = user.NickName;
                }


                return Ok(new MyPostListResponse
                {
                    TotalCount = myPostList.Count,
                    Code = ResultCode.Ok,
                    List = myPostList
                });
            }

            // '댓글 단 글' 게시판
            if (boardId == 2)
            {
                var myCommentPostList = await _context.Comment
                              .Where(c => c.UserId == loginedUser.Id)
                              .OrderByDescending(c => c.CreatedAt)
                              .Include(c => c.Post)
                              .Select(c => new PostListDto(c.Post))
                              .Distinct()
                              .ToListAsync();

                foreach (PostListDto post in myCommentPostList)
                {
                    var user = await _context.Users
                        .Where(u => u.Id == post.UserId)
                        .FirstOrDefaultAsync();

                    post.UserName = user.NickName;
                }

                return Ok(new MyCommentListResponse
                {
                    TotalCount = myCommentPostList.Count,
                    Code = ResultCode.Ok,
                    List = myCommentPostList
                });
            }

            // 스크랩 게시판
            if (boardId == 3)
            {
                var scrapList = await _context.ScrapPost
                .Where(s => s.UserId == loginedUser.Id)
                .OrderByDescending(s => s.CreatedAt)
                .Include(s => s.Post)
                .Select(s => new PostListDto(s.Post))
                .ToListAsync();

                foreach (PostListDto post in scrapList)
                {
                    var user = await _context.Users
                        .Where(u => u.Id == post.UserId)
                        .FirstOrDefaultAsync();

                    post.UserName = user.NickName;
                }

                return Ok(new ScrapPostListResponse
                {
                    TotalCount = scrapList.Count,
                    Code = ResultCode.Ok,
                    List = scrapList
                });
            }

            // 인기 게시판
            if (boardId == 4)
            {
                var popularPostList = await _context.Post
                    .Where(p => p.LikeCnt >= 20 || p.ScrapCnt >= 20)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new PostListDto(p))
                    .ToListAsync();

                foreach (PostListDto post in popularPostList)
                {
                    var user = await _context.Users
                        .Where(u => u.Id == post.UserId)
                        .FirstOrDefaultAsync();

                    post.UserName = user.NickName;
                }

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

            foreach (PostListDto post in posts)
            {
                var user = await _context.Users
                    .Where(u => u.Id == post.UserId)
                    .FirstOrDefaultAsync();

                post.UserName = user.NickName;
            }

            return Ok(new PostListResponse
            {
                TotalCount = posts.Count,
                Code = ResultCode.Ok,
                List = posts
            });
        }


        // 게시글 상세정보를 리턴합니다.
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDetailDto>> GetPost(int id)
        {
            var loginedUser = await _userManager.GetUserAsync(User);
            var likePost = await _context.LikePost
                .Where(l => l.PostId == id)
                .Where(l => l.UserId == loginedUser.Id)
                .FirstOrDefaultAsync();

            var isLiked = likePost != null;

            var scrapPost = await _context.ScrapPost
                .Where(s => s.PostId == id)
                .Where(s => s.UserId == loginedUser.Id)
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
                .Select(p => new PostDetailDto(p))
                .FirstOrDefaultAsync();

            var user = await _context.Users
                .Where(u => u.Id == post.UserId)
                .FirstOrDefaultAsync();

            post.UserName = user.NickName;
            post.ProfileUrl = user.SelectedProfileImage;

            if (post == null)
            {
                return Ok(new PostDetailResponse
                {
                    Code = ResultCode.NotFound
                });
            }

            return Ok(new PostDetailResponse
            {
                Code = ResultCode.Ok,
                Post = post,
                isLiked = isLiked,
                isScrapped = isScrapped,
                scrapPostId = scrapPostId
            });
        }


        // 게시글을 저장합니다.
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SavePostResponse>> PostPost(SavePostData post)
        {
            var loginedUser = await _userManager.GetUserAsync(User);
            var existingBoard = await _context.Board
                .Where(b => b.BoardId == post.BoardId)
                .FirstOrDefaultAsync();

            if (existingBoard != null)
            {
                var newPost = new Post
                {
                    UserId = loginedUser.Id,
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
                    Post = new PostDetailDto(newPost)
                });
            }

            return Ok(new SavePostResponse
            {
                Code = ResultCode.Fail,
                Message = "존재하지 않는 게시판이기에 게시물 작성이 불가합니다."
            });

        }

        // 게시글을 삭제합니다.
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
