using System;
namespace UMateModel.Models.UMatePlace
{
    public class PlaceReviewPostData
    {
        public string Place { get; set; }
        public string Taste { get; set; }
        public string Service { get; set; }
        public string Mood { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public int StarRating { get; set; }
        public string ReviewText { get; set; }
        public int Recommendation { get; set; }
    }

    public class PlaceReviewPutData : PlaceReviewPostData
    {
        public int PlaceReviewId { get; set; }
    }
}
