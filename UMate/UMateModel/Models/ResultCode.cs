using System;
namespace UMateModel.Models
{
    public class ResultCode
    {
        public static int Ok = 200;
        public static int Fail = -999;
        public static int NotFound = 404;

        public static int BoardExists = 1000;
        public static int CategoryExists = 1001;
        public static int PostExists = 1002;

        public static int ProfessorExists = 2000;
        public static int LectureInfoExists = 2001;
        public static int LectureReviewExists = 2002;
        public static int testInfoExists = 2003;
    }
}
