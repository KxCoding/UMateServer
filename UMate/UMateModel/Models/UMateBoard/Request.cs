using System;
using System.Collections.Generic;

namespace UMateModel.Models.UMateBoard
{
    /// <summary>
    /// BULK INSERT전용 POSTData
    ///
    /// 벌크 인서트 할 때 서버에서 받는 데이터
    /// </summary>
    public class BoardPostData
    {
        public int BoardId { get; set; }
        public int Section { get; set; }
        public string Name { get; set; }
    }

    public class CategoryPostData
    {
        public int CategoryId { get; set; }
        public int BoardId { get; set; }
        public string Name { get; set; }
    }


    public class PostPostData
    {
        public string UserId { get; set; }
        public int BoardId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int LikeCnt { get; set; }
        public int CommentCnt { get; set; }
        public int ScrapCnt { get; set; }
        public int CategoryNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }


    public class CommentPostData
    {
        public string UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public int OriginalCommentId { get; set; }
        public bool IsReComment { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class LectureInfoPostData
    {
        public int LectureInfoId { get; set; }
        public string Title { get; set; }
        public string Professor { get; set; }
        public string BookName { get; set; }
        public string BookLink { get; set; }
        public string Semesters { get; set; }
    }

    public class TestInfoPostData
    {
        public int TestInfoId { get; set; }
        public string UserId { get; set; }
        public int LectureInfoId { get; set; }
        public string Semester { get; set; }
        public string TestType { get; set; }
        public string TestStrategy { get; set; }
        public string QuestionTypes { get; set; }
    }


    /// <summary>
    /// Api POST
    /// 
    /// 게시글 작성 할 때 서버에서 받는 데이터
    /// </summary>
    /// 게시글 저장
    public class SavePostData
    {
        public int PostId { get; set; }

        public string UserId { get; set; }

        public int BoardId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int CategoryNumber { get; set; }

        public List<string> urlStrings { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    ///  강의평 저장
    public class LectureReviewPostData
    {
        public int LectureReviewId { get; set; }

        public string UserId { get; set; }

        public int LectureInfoId { get; set; }

        public int Assignment { get; set; }
        public int GroupMeeting { get; set; }
        public int Evaluation { get; set; }
        public int Attendance { get; set; }
        public int TestNumber { get; set; }
        public int Rating { get; set; }

        public string Semester { get; set; }
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    /// 시험 정보에서 문제 예시 저장
    public class ExamplePostData
    {
        public int ExampleId { get; set; }
        public int TestInfoId { get; set; }
        public string Content { get; set; }
    }

    // 시험정보 저
    public class SaveTestInfoData
    {
        public int TestInfoId { get; set; }
        public string UserId { get; set; }
        public int LectureInfoId { get; set; }
        public string Semester { get; set; }
        public string TestType { get; set; }
        public string TestStrategy { get; set; }
        public string QuestionTypes { get; set; }
        public List<string> Examples { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
