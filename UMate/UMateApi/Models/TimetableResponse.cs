using System;
using UMateModel.Models;

namespace UMateApi.Models
{
    public class TimetableResponse: CommonResponse
    {
        public TimetableDto Timetable { get; set; }
    }
}
