using System;
using System.Collections.Generic;
using UMateModel.Entities.Place;

namespace UMateModel.Models.UMatePlace
{
    public class PlaceReviewListResponse : CommonResponse
    {
        public int TotalCount { get; set; }
        public List<PlaceReview> List { get; set; }
    }

    public class PlaceReviewResponse : CommonResponse
    {
        public PlaceReviewDto PlaceReview { get; set; }
    }

    public class PlaceReviewPutResponse : CommonResponse
    {
    }
}
