using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UMateModel.Models;

namespace UMateModel.Models.UMatePlace
{
    public class PlacePostData
    {
        public string Name { get; set; }

        public string UniversityName { get; set; }

        public string District { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Type { get; set; }
        public string Keywords { get; set; }

        public string Tel { get; set; }
        public string InstagramId { get; set; }

        public string WebsiteUrl { get; set; }
    }

    public class PlaceBookmarkPostData
    {
        public int PlaceId { get; set; }
    }
}