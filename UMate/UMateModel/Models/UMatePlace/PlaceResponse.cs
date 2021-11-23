using System;
using System.Collections.Generic;
using UMateModel.Entities.Common;
using UMateModel.Models;

namespace UMateModel.Models.UMatePlace
{
    public class PlaceCommonResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string ClientAlertMessage { get; set; }
    }

    public class PlacePostResponse : PlaceCommonResponse
    {
        public PlacePostData Place { get; set; }
    }

    public class PlaceListResponse : PlaceCommonResponse
    {
        public UniversityPlaceMainDto University { get; set; }

        public int TotalCount { get; set; }

        public List<PlaceSimpleDto> Places { get; set; }
    }

    public class PlaceResponse : PlaceCommonResponse
    {
        public PlaceDto Place { get; set; }
    }

    public class PlaceBookmarkListResponse : PlaceCommonResponse
    {
        public string UserId { get; set; }

        public int TotalCount { get; set; }

        public List<PlaceSimpleDto> Places { get; set; }
    }

    public class PlaceBookmarkCheckResponse : PlaceCommonResponse
    {
        public string UserId { get; set; }
        public string PlaceName { get; set; }
        public bool IsBookmarked { get; set; }
    }

}