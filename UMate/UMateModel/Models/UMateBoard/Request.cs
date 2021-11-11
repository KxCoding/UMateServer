using System;
using System.Collections.Generic;

namespace UMateModel.Models.UMateBoard
{
    // BULK INSERT

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



    // Api Post
    // 게시글 작성
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





    //
    //
    // 강의 정보

    public class LectureInfoPostData
    {
        public int LectureInfoId { get; set; }
        public string Title { get; set; }
        public string Professor { get; set; }
        public string BookName { get; set; }
        public string BookLink { get; set; }
        public string Semesters { get; set; }
    }

    public class LectureReviewPostData
    {
        public int LectureReviewId { get; set; }

        public string UserId { get; set; }// context.User

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

    public class ExamplePostData
    {
        public int ExampleId { get; set; }
        public int TestInfoId { get; set; }
        public string Content { get; set; }
    }

    //벌크 인서트
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



    // post에 사용
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
