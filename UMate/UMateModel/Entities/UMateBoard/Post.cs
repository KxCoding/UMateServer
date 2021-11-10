using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace UMateModel.Entities.UMateBoard
{
    public class Post
    {
        public int PostId { get; set; }


        public string UserId { get; set; }


        public int BoardId { get; set; }
        [JsonIgnore]
        public Board Board { get; set; }


        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

        public int LikeCnt { get; set; }
        public int CommentCnt { get; set; }
        public int ScrapCnt { get; set; }

        public int CategoryNumber { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<PostImage> PostImages { get; set; }

        public List<Comment> Comments { get; set; }
        public List<LikePost> LikePosts { get; set; }
        public List<ScrapPost> ScrapPosts { get; set; }
    }
}
