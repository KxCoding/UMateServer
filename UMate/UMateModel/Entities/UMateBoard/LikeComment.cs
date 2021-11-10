using System;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.UMateBoard
{
    public class LikeComment
    {
        public int LikeCommentId { get; set; }

        public string UserId { get; set; }


        public int CommentId { get; set; }
        [JsonIgnore]
        public Comment Comment { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
