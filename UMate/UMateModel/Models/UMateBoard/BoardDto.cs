using System;
using System.Collections.Generic;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    public class BoardDto
    {
        public BoardDto(Board board)
        {
            BoardId = board.BoardId;
            Section = board.Section;
            Name = board.Name;
            Categories = board.Categories;
        }

        public int BoardId { get; set; }
        public int Section { get; set; }
        public string Name { get; set; }
        public List<Category> Categories { get; set; }
    }
}
