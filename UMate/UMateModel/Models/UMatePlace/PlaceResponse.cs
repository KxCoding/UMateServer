using System;
using System.Collections.Generic;
using UMateModel.Entities.Common;
using UMateModel.Models;

namespace UMateModel.Models.UMatePlace
{
    // post response

    public class PlacePostResponse : CommonResponse
    {
        public PlacePostData Place { get; set; }
    }

    public class UniversityPostResponse : CommonResponse
    {
        public University University { get; set; }
    }

    // get response

    public class PlaceListResponse : CommonResponse
    {
        public string UniversityName { get; set; }

        public int TotalCount { get; set; }

        public List<PlaceSimpleDto> Places { get; set; }
    }

    public class PlaceResponse : CommonResponse
    {
        public PlaceDto Place { get; set; }
    }

    public class BookmarkListResponse : CommonResponse
    {
        public string UserId { get; set; }

        public int TotalCount { get; set; }

        public List<PlaceSimpleDto> Places { get; set; }
    }

    public class BookmarkCheckResponse : CommonResponse
    {
        public string UserId { get; set; }
        public string PlaceName { get; set; }
        public bool IsBookmarked { get; set; }
    }
}