using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UMateModel.Entities.Timetable
{
    public class Timetable
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int TimetableId { get; set; }

        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public string RoomName { get; set; }

        public string ProfessorName { get; set; }

        public string CourseDay { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string BackgroundColor { get; set; }
    }
}
