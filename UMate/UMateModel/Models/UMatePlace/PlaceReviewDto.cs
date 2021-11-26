using System;
using UMateModel.Entities.Place;

namespace UMateModel.Models.UMatePlace
{
    public class PlaceReviewDto
    {
        public PlaceReviewDto(PlaceReview placeReview)
        {
            PlaceReviewId = placeReview.PlaceReviewId;
            PlaceName = placeReview.Place.Name;
            Taste = placeReview.Taste;
            Service = placeReview.Service;
            Mood = placeReview.Mood;
            Price = placeReview.Price;
            Amount = placeReview.Amount;
            StarRating = placeReview.StarRating;
            ReviewText = placeReview.ReviewText;
            UpdateDate = placeReview.UpdateDate;
        }

        public int PlaceReviewId { get; set; }
        public string PlaceName { get; set; }
        public string Taste { get; set; }
        public string Service { get; set; }
        public string Mood { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public double StarRating { get; set; }
        public string ReviewText { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
