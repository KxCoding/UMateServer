using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UMateModel.Contexts;
using UMateModel.Entities.UMateBoard;
using UMateModel.Models;
using UMateModel.Models.UMateBoard;

namespace BoardApi.Controllers
{
    [Authorize]
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

        // 해당 게시판 정보, 카테고리를 리턴합니다.
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
       
    }
}

