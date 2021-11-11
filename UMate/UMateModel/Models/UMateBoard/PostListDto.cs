using System;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    public class PostListDto
    {
        // 게시글 목록 get list에 사용
        public PostListDto(Post post)
        {
            PostId = post.PostId;
            Title = post.Title;
            Content = post.Content;
            CreatedAt = post.CreatedAt;
            // Id로 바꾸 
            UserName = post.UserId;
            LikeCnt = post.LikeCnt;
            CommentCnt = post.CommentCnt;
            ScrapCnt = post.ScrapCnt;
            CategoryNumber = post.CategoryNumber;
        }

        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public int LikeCnt { get; set; }
        public int CommentCnt { get; set; }
        public int ScrapCnt { get; set; }
        public int CategoryNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
