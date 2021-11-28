using System;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    /// <summary>
    /// 강의 세부 정보를 불러올 때 사용합니다.
    /// </summary>
    public class LectureInfoDetailDto
    {
        public LectureInfoDetailDto(LectureInfo lectureInfo)
        {
            LectureInfoId = lectureInfo.LectureInfoId;
            ProfessorId = lectureInfo.ProfessorId;
            Title = lectureInfo.Title;
            BookName = lectureInfo.BookName;
            BookLink = lectureInfo.BookLink;
            Semesters = lectureInfo.Semesters;
        }

        public int LectureInfoId { get; set; }
        public int ProfessorId { get; set; }
        public string Title { get; set; }
        public string BookName { get; set; }
        public string BookLink { get; set; }
        public string Semesters { get; set; }
    }
}
