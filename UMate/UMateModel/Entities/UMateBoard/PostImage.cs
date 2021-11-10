using System;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.UMateBoard
{
    public class PostImage
    {
        public int PostImageId { get; set; }


        public int PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }


        public string UrlString { get; set; }
    }
}
