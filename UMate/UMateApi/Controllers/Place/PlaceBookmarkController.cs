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
using UMateModel.Entities;
using UMateModel.Entities.Place;
using UMateModel.Models;
using UMateModel.Models.UMatePlace;
using PlaceBookmark = UMateModel.Entities.Place.PlaceBookmark;

namespace UMateApi.Controllers
{
    [Authorize]
    [Route("api/place/bookmark")]
    [ApiController]
    public class PlaceBookmarkController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IConfiguration Configuration { get; }

        public PlaceBookmarkController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
        }

        // GET: api/place/bookmark
        [HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaceBookmarkListResponse>>> GetBookmarkedPlaceList()
        {
            // 요청하고 있는 사용자 확인
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Ok(new PlaceBookmarkListResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "User Not Found",
                    ClientAlertMessage = "사용자를 찾을 수 없습니다."
                });
            }

            // 사용자 북마크 검색
            var places = await _context.PlaceBookmark
                .Include(b => b.Place)
                .Where(b => b.UserId == user.Id)
                .OrderBy(b => b.InsertDate)
                .Select(b => new PlaceSimpleDto(b.Place))
                .ToListAsync();

            //var places = new List<PlaceSimpleDto>();

            //foreach (int id in placeIds)
            //{
            //    var place = await _context.Place
            //    .Where(p => p.PlaceId == id)
            //    .Select(p => new PlaceSimpleDto(p))
            //    .FirstOrDefaultAsync();

            //    places.Append(place);
            //}

            return Ok(new PlaceBookmarkListResponse
            {
                Code = ResultCode.Ok,
                UserId = user.Id,
                TotalCount = places.Count(),
                Places = places
            });
        }

        // GET: api/place/bookmark/place/5
        [HttpGet("place/{placeId}")]
        public async Task<ActionResult<PlaceBookmarkCheckResponse>> CheckIfBookmarked(int placeId)
        {
            // 요청하고 있는 사용자 확인
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Ok(new PlaceBookmarkCheckResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "can't find user",
                    ClientAlertMessage = "사용자를 찾을 수 없습니다."
                });
            }

            // 전달된 id로 상점 검색
            var place = await _context.Place
                .Where(s => s.PlaceId == placeId)
                .FirstOrDefaultAsync();

            // 상점이 없으면 return
            if (place == null)
            {
                return Ok(new PlaceBookmarkCheckResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "can't find place",
                    ClientAlertMessage = "상점을 찾을 수 없습니다."
                });
            }

            // 사용자 id, 상점 id 모두 일치 하는 북마크 데이터 검색
            var bookmark = await _context.PlaceBookmark
                .Where(b => b.UserId == user.Id && b.PlaceId == placeId)
                .FirstOrDefaultAsync();

            if (bookmark == null)
            {
                return Ok(new PlaceBookmarkCheckResponse
                {
                    Code = ResultCode.Ok,
                    Message = "the place isn't bookmarked",
                    UserId = user.Id,
                    PlaceName = place.Name,
                    IsBookmarked = false
                });
            }

            return Ok(new PlaceBookmarkCheckResponse
            {
                Code = ResultCode.Ok,
                Message = "the place is bookmarked",
                UserId = user.Id,
                PlaceName = place.Name,
                IsBookmarked = true
            });
        }

        // POST: api/place/bookmark
        [HttpPost]
        public async Task<ActionResult<PlaceCommonResponse>> CreateBookmark(PlaceBookmarkPostData bookmarkPostData)
        {
            // 요청하고 있는 사용자 확인
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Ok(new PlaceCommonResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "can't find user",
                    ClientAlertMessage = "사용자를 찾을 수 없습니다."
                });
            }

            // 전달된 id로 상점 검색
            var place = await _context.Place
                .Where(s => s.PlaceId == bookmarkPostData.PlaceId)
                .FirstOrDefaultAsync();

            // 상점이 없으면 return
            if (place == null)
            {
                return Ok(new PlaceCommonResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "can't find place",
                    ClientAlertMessage = "상점이 존재하지 않습니다."
                });
            }

            // 사용자 id, 상점 id 모두 일치 하는 북마크 데이터 검색
            var bookmark = await _context.PlaceBookmark
                .Where(b => b.UserId == user.Id)
                .Where(b => b.PlaceId == bookmarkPostData.PlaceId) // 전달된 id로 검색
                .FirstOrDefaultAsync();

            if (bookmark != null)
            {
                return Ok(new PlaceCommonResponse
                {
                    Code = ResultCode.ExistsAlready,
                    Message = "the user bookmarked the place already",
                    ClientAlertMessage = "이미 북마크한 장소입니다."
                });
            }
            else
            {
                var newBookmark = new PlaceBookmark
                {
                    UserId = user.Id,
                    PlaceId = bookmarkPostData.PlaceId,
                    Place = place,
                    InsertDate = DateTime.UtcNow
                };

                _context.PlaceBookmark.Add(newBookmark);
                await _context.SaveChangesAsync();

                return Ok(new PlaceCommonResponse
                {
                    Code = ResultCode.Ok,
                    Message = "bookmark successfully created",
                    ClientAlertMessage = "북마크가 추가되었습니다."
                });
            }

        }

        // DELETE: api/place/bookmark/place/5
        [HttpDelete("place/{placeId}")]
        public async Task<ActionResult<PlaceCommonResponse>> DeleteBookmark(int placeId)
        {
            // 요청하고 있는 사용자 확인
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Ok(new PlaceCommonResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "can't find user",
                    ClientAlertMessage = "사용자를 찾을 수 없습니다."
                });
            }

            var place = await _context.Place
                .Where(s => s.PlaceId == placeId)
                .FirstOrDefaultAsync();

            if (place == null)
            {
                return Ok(new PlaceCommonResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "can't find place",
                    ClientAlertMessage = "상점이 존재하지 않습니다."
                });
            }

            var bookmark = await _context.PlaceBookmark
                .Where(b => b.UserId == user.Id && b.PlaceId == placeId)
                .FirstOrDefaultAsync();

            if (bookmark == null)
            {
                return Ok(new PlaceCommonResponse
                {
                    Code = ResultCode.NotFound,
                    Message = "can't find bookmark",
                    ClientAlertMessage = "해제하려는 북마크가 존재하지 않습니다."
                });
            }
            else
            {
                _context.PlaceBookmark.Remove(bookmark);
                await _context.SaveChangesAsync();

                return Ok(new PlaceCommonResponse
                {
                    Code = ResultCode.Ok,
                    Message = "deleted successfully",
                    ClientAlertMessage = "북마크가 해제되었습니다."
                });
            }
        }

    }
}
