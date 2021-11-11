using System;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    // 게시글 상세화면 get에서 사용
    public class PostDto
    {
        public PostDto(Post post)
        {
            PostId = post.PostId;
            UserId = post.UserId;
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
