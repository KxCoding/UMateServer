using System;
using System.Collections.Generic;

namespace UMateModel.Entities.UMateBoard
{
    public class Board
    {
        public int BoardId { get; set; }

        public int Section { get; set; }
        public string Name { get; set; }

        public List<Category> Categories { get; set; }
        public List<Post> Posts { get; set; }
    }
}
