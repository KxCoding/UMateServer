using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.Place
{
    public class PlaceBookmark
    {
        public int PlaceBookmarkId { get; set; }

        [Required]
        public string UserId { get; set; }

        public int PlaceId { get; set; }

        [JsonIgnore]
        public Place Place { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
