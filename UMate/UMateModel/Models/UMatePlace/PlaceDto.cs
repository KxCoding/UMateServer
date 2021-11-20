using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UMateModel.Models;
using UMateModel.Entities.Place;

namespace UMateModel.Models.UMatePlace
{
    public class PlaceSimpleDto
    {
        public int PlaceId { get; set; }
        public string Name { get; set; }

        public string District { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Type { get; set; }
        public string Keywords { get; set; }
        public string ThumbnailImageUrl { get; set; }

        public PlaceSimpleDto(Place place)
        {
            PlaceId = place.PlaceId;
            Name = place.Name;

            District = place.District;
            Latitude = place.Latitude;
            Longitude = place.Longitude;

            Type = place.Type;
            Keywords = place.Keywords;

            ThumbnailImageUrl = place.ThumbnailImageUrl;
        }
    }

    public class PlaceDto
    {
        public int PlaceId { get; set; }
        public string Name { get; set; }

        public string District { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

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

        public PlaceDto(Place place)
        {
            PlaceId = place.PlaceId;
            Name = place.Name;

            District = place.District;
            Latitude = place.Latitude;
            Longitude = place.Longitude;

            Type = place.Type;
            Keywords = place.Keywords;

            Tel = place.Tel;
            InstagramId = place.InstagramId;
            WebsiteUrl = place.WebsiteUrl;

            ThumbnailImageUrl = place.ThumbnailImageUrl;
            PlaceImageUrl0 = place.PlaceImageUrl0;
            PlaceImageUrl1 = place.PlaceImageUrl1;
            PlaceImageUrl2 = place.PlaceImageUrl2;
            PlaceImageUrl3 = place.PlaceImageUrl3;
            PlaceImageUrl4 = place.PlaceImageUrl4;
            PlaceImageUrl5 = place.PlaceImageUrl5;
        }
    }
}