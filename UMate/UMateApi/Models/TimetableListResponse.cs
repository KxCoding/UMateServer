using System;
using System.Collections.Generic;
using UMateModel.Entities.Timetable;
using UMateModel.Models;

namespace UMateApi.Models
{
    public class TimetableListResponse: CommonResponse
    {
        public List<TimetableDto> List { get; set; }   
    }

    public class TimetableDto
    {
        public TimetableDto(Timetable timetable)
        {
            TimetableId = timetable.TimetableId;
            CourseId = timetable.CourseId;
            CourseName = timetable.CourseName;
            RoomName = timetable.RoomName;
            ProfessorName = timetable.ProfessorName;
            CourseDay = timetable.CourseDay;
            StartTime = timetable.StartTime;
            EndTime = timetable.EndTime;
            BackgroundColor = timetable.BackgroundColor;
            TextColor = timetable.TextColor;
        }

        public int TimetableId { get; set; }

        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public string RoomName { get; set; }

        public string ProfessorName { get; set; }

        public string CourseDay { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string BackgroundColor { get; set; }

        public string TextColor { get; set; }
    }

}
