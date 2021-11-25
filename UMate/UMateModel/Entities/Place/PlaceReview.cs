using System;
using System.ComponentModel.DataAnnotations;

namespace UMateModel.Entities.Place
{
    /// <summary>
    /// 상점 리뷰
    /// </summary>
    public class PlaceReview
    {
        /// <summary>
        /// 상점 리뷰 아이디
        /// </summary>
        public int PlaceReviewId { get; set; }

        /// <summary>
        /// 유저 아이디
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 상점 아이디
        /// </summary>
        public int? PlaceId { get; set; }

        /// <summary>
        /// 상점 정보를 가진 속성
        /// </summary>
        public Place Place { get; set; }

        /// <summary>
        /// 맛
        /// </summary>
        [Required]
        public string Taste { get; set; }

        /// <summary>
        /// 서비스
        /// </summary>
        [Required]
        public string Service { get; set; }

        /// <summary>
        /// 분위기
        /// </summary>
        [Required]
        public string Mood { get; set; }

        /// <summary>
        /// 가격
        /// </summary>
        [Required]
        public string Price { get; set; }

        /// <summary>
        /// 음식양
        /// </summary>
        [Required]
        public string Amount { get; set; }

        /// <summary>
        /// 평점
        /// </summary>
        [Required]
        public double StarRating { get; set; }

        /// <summary>
        /// 리뷰 텍스트
        /// </summary>
        [Required]
        public string ReviewText { get; set; }

        /// <summary>
        /// 추천수
        /// </summary>
        public int RecommendationCount { get; set; }

        /// <summary>
        /// 리뷰 작성 날짜
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 리뷰 수정 날짜
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}
