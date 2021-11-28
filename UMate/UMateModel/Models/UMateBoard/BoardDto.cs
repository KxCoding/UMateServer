using System;
using System.Collections.Generic;
using UMateModel.Entities.UMateBoard;

namespace UMateModel.Models.UMateBoard
{
    /// <summary>
    /// 게시판 목록을 불러올 때 사용합니다.
    /// </summary>
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
