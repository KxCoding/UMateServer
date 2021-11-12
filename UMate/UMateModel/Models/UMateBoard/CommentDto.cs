using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    public class CommentDto
    {
        public CommentDto(Comment comment)
        {
            CommentId = comment.CommentId;
            UserId = comment.UserId;
            UserName = "";
            ProfileUrl = "";
            PostId = comment.PostId;
            Content = comment.Content;
            LikeCnt = comment.LikeCnt;
            OriginalCommentId = comment.OriginalCommentId;
            IsReComment = comment.IsReComment;
            CreatedAt = comment.CreatedAt;
        }

        public int CommentId { get; set; }


        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfileUrl { get; set; }


        public int PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }


        [Required]
        public string Content { get; set; }
        public int LikeCnt { get; set; }

        public int OriginalCommentId { get; set; }
        public bool IsReComment { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
