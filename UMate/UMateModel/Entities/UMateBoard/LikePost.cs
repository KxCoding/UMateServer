using System;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.UMateBoard
{
    public class LikePost
    {
        public int LikePostId { get; set; }


        public string UserId { get; set; }


        public int PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
