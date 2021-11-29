using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMateModel.Models;
using UMateModel.Contexts;
using UMateModel.Models.UMateBoard;
using UMateModel.Entities.UMateBoard;

namespace BoardBulkInsert.Controllers
{
    [Route("bi/board")]
    [ApiController]
    public class BoardBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BoardBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }


        // 게시판 벌크 인서트
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BoardPostResponse>> PostBoard(BoardPostData board)
        {
            // 이미 존재하는 게시판인지 확인
            var existingBoard = await _context.Board
                .Where(b => b.Name == board.Name)
                .FirstOrDefaultAsync();

            // 게시판이 이미 존재한다면
            if (existingBoard != null)
            {
                // 게시판에 해당하는 카테고리를 저장
                var categories = await _context.Category
                    .Where(b => b.BoardId == board.BoardId)
                    .ToListAsync();

                // 게시판에 해당하는 게시물을 저장
                var posts = await _context.Post
                    .Where(p => p.BoardId == board.BoardId)
                    .ToListAsync();

                existingBoard.Categories = categories;
                existingBoard.Posts = posts;

                _context.SaveChanges();

                return Ok(new BoardPostResponse
                {
                    Code = ResultCode.BoardExists,
                    Message = "이미 존재하는 게시판입니다."
                });
            }

            // 존재하지 않는 게시판이라면 새로 추가
            var newBoard = new Board
            {
                Section = board.Section,
                Name = board.Name
            };

            _context.Board.Add(newBoard);
            await _context.SaveChangesAsync();

            return Ok(new BoardPostResponse
            {
                Code = ResultCode.Ok,
                Board = newBoard
            });
        }

    }
}

