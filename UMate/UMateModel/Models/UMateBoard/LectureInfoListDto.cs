using System;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    /// <summary>
    /// 강의 목록을 불러올 때 사용합니다.
    /// </summary>
    public class LectureInfoListDto
    {
        public LectureInfoListDto(LectureInfo lectureInfo)
        {
            LectureInfoId = lectureInfo.LectureInfoId;
            Title = lectureInfo.Title;
            Professor = lectureInfo.Professor.Name;

            if (lectureInfo.LectureReviews.Count > 0)
            {
                var recentReview = lectureInfo.LectureReviews[lectureInfo.LectureReviews.Count - 1];

                ReviewId = recentReview.LectureReviewId;
                Semester = recentReview.Semester;
                Rating = recentReview.Rating;
                Content = recentReview.Content;
            }
        }

        public int LectureInfoId { get; set; }
        public string Title { get; set; }
        public string Professor { get; set; }

        public int ReviewId { get; set; }
        public string Semester { get; set; }
        public int? Rating { get; set; }
        public string Content { get; set; }
    }
}
