using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities.UMateBoard;
using UMateModel.Models;
using UMateModel.Models.UMateBoard;

namespace BoardApi.Controllers
{
    [Route("api/board")]
    [ApiController]
    public class BoardApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BoardApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 게시판 목록 화면
        // GET: api/BoardApi
        [HttpGet]
        public async Task<ActionResult<BoardListResponse<BoardDto>>> GetBoard()
        {
            var list = await _context.Board
                .OrderBy(b => b.BoardId)
                .Include(b => b.Categories)
                .Select(b => new BoardDto(b))
                .ToListAsync();

            return Ok(new BoardListResponse
            {
                Code = ResultCode.Ok,
                List = list
            });
        }

        // 사용처 미

        // GET: api/BoardApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDto>> GetBoard(int id)
        {
            var board = await _context.Board
                .Where(b => b.BoardId == id)
                .Include(b => b.Categories)
                .Select(b => new BoardDto(b))
                .FirstOrDefaultAsync();

            if (board == null)
            {
                return Ok(new BoardResponse
                {
                    Code = ResultCode.NotFound
                });
            }

            return Ok(new BoardResponse
            {
                Code = ResultCode.Ok,
                Board = board
            });
        }

        // PUT: api/BoardApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoard(int id, Board board)
        {
            if (id != board.BoardId)
            {
                return BadRequest();
            }

            _context.Entry(board).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BoardApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BoardPostResponse>> PostBoard(BoardPostData board)
        {
            // 이미 존재하는 게시판인지 확인
            var existingBoard = await _context.Board
                .Where(b => b.Name == board.Name)
                .FirstOrDefaultAsync();

            if (existingBoard != null)
            {
                // 저장이 안 된다!!!
                var categories = await _context.Category
                    .Where(b => b.BoardId == board.BoardId)
                    .ToListAsync();

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

        // DELETE: api/BoardApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Board>> DeleteBoard(int id)
        {
            var board = await _context.Board.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            _context.Board.Remove(board);
            await _context.SaveChangesAsync();

            return board;
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.BoardId == id);
        }
    }
}

