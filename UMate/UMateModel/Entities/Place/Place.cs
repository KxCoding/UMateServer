using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace UMateModel.Entities.Place
{
    public class Place
    {
        public int PlaceId { get; set; }

        [Required]
        public string Name { get; set; }

        /*
        public int UniversityId { get; set; }
        [JsonIgnore]
        public University University { get; set; }
        */

        public string UniversityName { get; set; }

        public string District { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required]
        public string Type { get; set; }

        public string Keywords { get; set; } // 키워드를 연쇄시킨 하나의 문자열

        public string Tel { get; set; }

        public string InstagramId { get; set; }

        public string WebsiteUrl { get; set; }

        public string ThumbnailImageUrl { get; set; }
        public string PlaceImageUrl0 { get; set; }
        public string PlaceImageUrl1 { get; set; }
        public string PlaceImageUrl2 { get; set; }
        public string PlaceImageUrl3 { get; set; }
        public string PlaceImageUrl4 { get; set; }
        public string PlaceImageUrl5 { get; set; }

        [NotMapped]
        public IFormFile ThumbnailFile { get; set; }
        [NotMapped]
        public IFormFile PlaceImageFile0 { get; set; }
        [NotMapped]
        public IFormFile PlaceImageFile1 { get; set; }
        [NotMapped]
        public IFormFile PlaceImageFile2 { get; set; }
        [NotMapped]
        public IFormFile PlaceImageFile3 { get; set; }
        [NotMapped]
        public IFormFile PlaceImageFile4 { get; set; }
        [NotMapped]
        public IFormFile PlaceImageFile5 { get; set; }
    }
}
