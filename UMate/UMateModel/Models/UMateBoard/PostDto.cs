using System;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    /// <summary>
    /// 게시글 상세화면 정보를 불러올 때 사용합니다.
    /// </summary>
    public class PostDetailDto
    {
        public PostDetailDto(Post post)
        {
            PostId = post.PostId;
            UserId = post.UserId;
            UserName = "";
            ProfileUrl = "";
            BoardId = post.BoardId;
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
        public string ProfileUrl { get; set; }

        public int BoardId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int LikeCnt { get; set; }
        public int CommentCnt { get; set; }
        public int ScrapCnt { get; set; }

        public int CategoryNumber { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
