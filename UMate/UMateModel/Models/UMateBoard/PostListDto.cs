using System;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    /// <summary>
    /// 게시글 목록을 불러올 때 사용합니다.
    /// </summary>
    public class PostListDto
    {
        public PostListDto(Post post)
        {
            PostId = post.PostId;
            UserId = post.UserId;
            UserName = "";

            Title = post.Title;
            Content = post.Content;
            
            LikeCnt = post.LikeCnt;
            CommentCnt = post.CommentCnt;
            ScrapCnt = post.ScrapCnt;
            CategoryNumber = post.CategoryNumber;
            CreatedAt = post.CreatedAt;
        }

        public int PostId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        
        public int LikeCnt { get; set; }
        public int CommentCnt { get; set; }
        public int ScrapCnt { get; set; }
        public int CategoryNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
