using System;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.LectureInfo
{
    public class LectureReview
    {
        public int LectureReviewId { get; set; }


        public string UserId { get; set; }

        public int LectureInfoId { get; set; }
        [JsonIgnore]
        public LectureInfo LectureInfo { get; set; }


        public int Assignment { get; set; }

        public int GroupMeeting { get; set; }

        public int Evaluation { get; set; }

        public int Attendance { get; set; }

        public int TestNumber { get; set; }

        public int Rating { get; set; }

        [Required]
        public string Semester { get; set; }
        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
}
