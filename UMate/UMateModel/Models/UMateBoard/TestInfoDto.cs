using System;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    /// <summary>
    /// 시험 정보 저장 응답 데이터입니다.
    /// </summary>
    public class TestInfoDto
    {
        public TestInfoDto(TestInfo testInfo)
        {
            TestInfoId = testInfo.TestInfoId;
            UserId = testInfo.UserId;
            LectureInfoId = testInfo.LectureInfoId;
            Semester = testInfo.Semester;
            TestType = testInfo.TestType;
            TestStrategy = testInfo.TestStrategy;
            QuestionTypes = testInfo.QuestionTypes;
            CreatedAt = testInfo.CreatedAt;
        }
        public int TestInfoId { get; set; }
        public string UserId { get; set; }
        public int LectureInfoId { get; set; }
        public string Semester { get; set; }
        public string TestType { get; set; }
        public string TestStrategy { get; set; }
        public string QuestionTypes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
