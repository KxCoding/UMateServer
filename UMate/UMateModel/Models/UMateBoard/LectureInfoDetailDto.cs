﻿using System;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
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
