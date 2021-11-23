using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities;
using UMateModel.Entities.Place;
using UMateModel.Models;
using UMateModel.Models.UMatePlace;

namespace UMateApi.Controllers
{
    [Authorize]
    [Route("api/place")]
    [ApiController]
    public class PlaceController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlaceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/place
        // 저장된 전체 상점 리스트 리턴
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaceListResponse>>> GetPlaceList()
        {
            var list = await _context.Place
                .Include(p => p.University)
                .OrderBy(p => p.Type)
                .ThenBy(p => p.Longitude)
                .Select(p => new PlaceSimpleDto(p))
                .ToListAsync();

            var total = await _context.Place.CountAsync();

            return Ok(new PlaceListResponse
            {
                Code = ResultCode.Ok,
                TotalCount = total,
                Places = list
            });
        }

        // GET: api/place/university/1
        // 대학교 id 전달하면 해당 학교 주변 상점 리스트 리턴
        [HttpGet("university/{universityId}")]
        public async Task<ActionResult<IEnumerable<PlaceListResponse>>> GetPlaceList(int universityId)
        {
            var university = await _context.University
                .Where(u => u.UniversityId == universityId)
                .FirstOrDefaultAsync();

            if (university == null)
            {
                return Ok(new PlaceListResponse
                {
                    Code = ResultCode.NotFound,
                    Message = $"cannot find university with passed id {universityId}",
                    ClientAlertMessage = "선택한 대학을 찾을 수 없습니다."
                });
            }

            var list = await _context.Place
                .Include(p => p.University)
                .Where(p => p.UniversityId == universityId)
                .OrderBy(p => p.Type)
                .ThenBy(p => p.Longitude)
                .Select(p => new PlaceSimpleDto(p))
                .ToListAsync();

            var total = await _context.Place.CountAsync();

            return Ok(new PlaceListResponse
            {
                Code = ResultCode.Ok,
                University = new UniversityPlaceMainDto(university),
                TotalCount = total,
                Places = list
            });
        }

        // GET: api/Place/5
        // 전달된 상점 id와 일치하는 상점 정보 리턴
        [HttpGet("{placeId}")]
        public async Task<ActionResult<PlaceResponse>> GetPlace(int placeId)
        {
            var place = await _context.Place
                .Include(p => p.University)
                .Where(s => s.PlaceId == placeId)
                .FirstOrDefaultAsync();

            if (place == null)
            {
                return Ok(new PlaceResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "can't find place",
                    ClientAlertMessage = "상점을 찾을 수 없습니다."
                });
            }

            var placeData = new PlaceDto(place);

            return Ok(new PlaceResponse
            {
                Code = ResultCode.Ok,
                Place = placeData
            });
        }

        // PUT: api/Place/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlace(int id, Place place)
        {
            if (id != place.PlaceId)
            {
                return BadRequest();
            }

            _context.Entry(place).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(id))
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

        // POST: api/place
        [HttpPost]
        public async Task<ActionResult<CommonResponse>> PostPost(PlacePostData place)
        {
            var existingUniversity = _context.University
                .Where(u => u.Name == place.UniversityName)
                .FirstOrDefault();

            var universityId = 0;

            if (existingUniversity != null)
            {
                universityId = existingUniversity.UniversityId;
            }

            var existingPlace = _context.Place
                .Where(p => p.Name == place.Name)
                .FirstOrDefault();

            if (existingPlace != null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.ExistsAlready,
                    Message = $"request data(place) already exists - {existingPlace.Name}"
                });
            }
            else
            {
                var newPlace = new Place
                {
                    Name = place.Name,
                    UniversityId = universityId,
                    District = place.District,
                    Latitude = place.Latitude,
                    Longitude = place.Longitude,
                    Type = place.Type,
                    Keywords = place.Keywords,
                    Tel = place.Tel,
                    InstagramId = place.InstagramId,
                    WebsiteUrl = place.WebsiteUrl
                };

                _context.Place.Add(newPlace);
                await _context.SaveChangesAsync();

                return Ok(new CommonResponse
                {
                    Code = ResultCode.Ok,
                    Message = $"success inserting place - {newPlace.Name}"
                });
            }
        }

        // DELETE: api/place/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommonResponse>> DeletePlace(int id)
        {
            var place = await _context.Place.FindAsync(id);

            if (place == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Ok,
                    Message = "can't find place"
                });
            }

            _context.Place.Remove(place);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok,
                Message = $"place deleted successfully - {place.Name}"
            });
        }

        private bool PlaceExists(int id)
        {
            return _context.Place.Any(e => e.PlaceId == id);
        }
    }
}
