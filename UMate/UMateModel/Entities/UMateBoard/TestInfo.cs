using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.UMateBoard
{
    public class TestInfo
    {
        public int TestInfoId { get; set; }


        public string UserId { get; set; }


        public int LectureInfoId { get; set; }
        [JsonIgnore]
        public LectureInfo LectureInfo { get; set; }


        [Required]
        public string Semester { get; set; }
        [Required]
        public string TestType { get; set; }
        [Required]
        public string TestStrategy { get; set; }

        [Required]
        public string QuestionTypes { get; set; }
        [Required]
        public List<Example> Examples { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
