using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.UMateBoard
{
    public class Comment
    {
        public int CommentId { get; set; }


        public string UserId { get; set; }


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
