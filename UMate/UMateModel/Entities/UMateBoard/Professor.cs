using System;
using System.Collections.Generic;

namespace UMateModel.Entities.UMateBoard
{
    public class Professor
    {
        public int ProfessorId { get; set; }

        public string Name { get; set; }

        public List<LectureInfo> LectureInfos { get; set; }
    }
}
