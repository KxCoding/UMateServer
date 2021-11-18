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
using UMateApi.Models;
using UMateModel.Entities.Timetable;
using UMateModel.Models;

namespace UMateApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IConfiguration Configuration { get; }

        public TimetableController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
            _context = context;
        }

        // GET: api/Timetable
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timetable>>> GetTimetable()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "User Not Found"
                });
            }

            var timetableList = await _context.Timetable
                .Where(t => t.UserId == user.Id)
                .Select(t => new TimetableDto(t))
                .ToListAsync();


            return Ok(new TimetableListResponse
            {
                Code = ResultCode.Ok,
                List = timetableList,
                Message = "Get Timetable List OK"
            });
        }

        // GET: api/Timetable/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Timetable>> GetTimetable(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "User Not Found"
                });
            }

            var timetable = await _context.Timetable
                .Where(t => t.UserId == user.Id && t.TimetableId == id)
                .Select(t => new TimetableDto(t))
                .FirstOrDefaultAsync();


            if (timetable == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "Timetable Not Found"
                });
            }


            return Ok(new TimetableResponse
            { 
                Code = ResultCode.Ok,
                Timetable = timetable,
                Message = "Get 개별 Timetable OK"
            });
        }

        // PUT: api/Timetable/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimetable(int id, Timetable timetable)
        {
            if (id != timetable.TimetableId)
            {
                return BadRequest();
            }

            _context.Entry(timetable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimetableExists(id))
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

        // POST: api/Timetable
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Timetable>> PostTimetable(Timetable timetable)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "User Not Found"
                });
            }

            var newTimetable = new Timetable
            {
                UserId = user.Id,
                TimetableId = timetable.TimetableId,
                CourseId = timetable.CourseId,
                CourseName = timetable.CourseName,
                RoomName = timetable.RoomName,
                ProfessorName = timetable.ProfessorName,
                CourseDay = timetable.CourseDay,
                StartTime = timetable.StartTime,
                EndTime = timetable.EndTime,
                BackgroundColor = timetable.BackgroundColor,
                TextColor = timetable.TextColor
            };

            _context.Timetable.Add(newTimetable);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTimetable", new { id = timetable.TimetableId }, timetable);
            return Ok(new TimetablePostResponse
            {
                TimetableId = newTimetable.TimetableId,
                Code = ResultCode.Ok,
                Message = "Timetable POST OK"
            });
        }

        // DELETE: api/Timetable/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Timetable>> DeleteTimetable(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "User Not Found"
                });
            }

            var timetable = await _context.Timetable
                .Where(t => t.UserId == user.Id && t.TimetableId == id)
                .FirstOrDefaultAsync();

            if (timetable == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "삭제할 시간표가 없습니다."
                });
            }

            _context.Timetable.Remove(timetable);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok,
                Message = "삭제 OK"
            });
        }

        private bool TimetableExists(int id)
        {
            return _context.Timetable.Any(e => e.TimetableId == id);
        }
    }
}
