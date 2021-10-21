using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.LectureInfo
{
    public class LectureInfo
    {
        public int LectureInfoId { get; set; }

        public int ProfessorId { get; set; }
        [JsonIgnore]
        public Professor Professor { get; set; }

        [Required]
        public string Title { get; set; }
        public string BookName { get; set; }
        public string BookLink { get; set; }

        [Required]
        public string Semesters { get; set; } //2019-1,2019-2

        public List<LectureReview> LectureReviews { get; set; }
        public List<TestInfo> TestInfos { get; set; }
    }
}
