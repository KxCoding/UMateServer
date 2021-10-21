using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.LectureInfo
{
    public class Example
    {
        public int ExampleId { get; set; }


        public int TestInfoId { get; set; }
        [JsonIgnore]
        public TestInfo TestInfo { get; set; }


        [Required]
        public string Content { get; set; }
    }
}
