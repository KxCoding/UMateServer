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
using UMateModel.Entities.Place;
using UMateModel.Models;
using UMateModel.Models.UMatePlace;

namespace UMateApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PlaceReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IConfiguration Configuration { get; }

        public PlaceReviewController(
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

        // GET: PlaceReview
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaceReview>>> GetPlaceReview()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "유저 정보를 찾을 수 없습니다."
                });
            }

            var list = await _context.PlaceReview
                .Where(p => p.UserId == user.Id)
                .Include(p => p.Place)
                .OrderByDescending(p => p.InsertDate)
                .ToListAsync();

            return Ok(new PlaceReviewListResponse
            {
                Code = ResultCode.Ok,
                TotalCount = list.Count(),
                List = list
            });
        }

        // GET: PlaceReview/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaceReview>> GetPlaceReview(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "유저 정보를 찾을 수 없습니다."
                });
            }

            var placeReview = await _context.PlaceReview
                .Include(p => p.Place)
                .Where(p => p.PlaceReviewId == id && p.UserId == user.Id)
                .Select(p => new PlaceReviewDto(p))
                .FirstOrDefaultAsync();
            if (placeReview == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.ReviewNotExists,
                    Message = "리뷰가 존재하지 않습니다."
                });
            }

            return Ok(new PlaceReviewResponse
            {
                Code = ResultCode.Ok,
                PlaceReview = placeReview
            });
        }

        // PUT: PlaceReview/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaceReview(int id, PlaceReviewPutData placeReview)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "유저 정보를 찾을 수 없습니다."
                });
            }

            if (id != placeReview.PlaceReviewId)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.ReviewNotExists,
                    Message = "리뷰 아이디와 일치하는 리뷰 정보가 존재하지 않습니다."
                });
            }

            var targetPlaceReview = await _context.PlaceReview
                .Where(p => p.PlaceReviewId == id && p.UserId == user.Id)
                .FirstOrDefaultAsync();
            if (targetPlaceReview == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "리뷰 정보를 찾을 수 없습니다."
                });
            }

            var place = _context.Place
                .Where(p => p.Name == placeReview.Place)
                .FirstOrDefault();

            targetPlaceReview.PlaceReviewId = placeReview.PlaceReviewId;
            targetPlaceReview.Place = place;
            targetPlaceReview.Taste = placeReview.Taste;
            targetPlaceReview.Service = placeReview.Service;
            targetPlaceReview.Mood = placeReview.Mood;
            targetPlaceReview.Price = placeReview.Price;
            targetPlaceReview.Amount = placeReview.Amount;
            targetPlaceReview.StarRating = placeReview.StarRating;
            targetPlaceReview.ReviewText = placeReview.ReviewText;
            targetPlaceReview.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new PlaceReviewPutResponse
            {
                Code = ResultCode.Ok,
                Message = "리뷰를 수정했습니다."
            });
        }

        // POST: PlaceReview
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PlaceReview>> PostPlaceReview(PlaceReviewPostData placeReview)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "유저 정보를 찾을 수 없습니다."
                });
            }

            var existingPlace = _context.Place
                .Where(p => p.Name == placeReview.Place)
                .FirstOrDefault();
            if (existingPlace == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "상점 정보가 존재하지 않습니다."
                });
            }

            var existingReview = await _context.PlaceReview
                .Where(p => p.Place.Name == placeReview.Place)
                .FirstOrDefaultAsync();

            var newPlaceReview = new PlaceReview
            {
                UserId = user.Id,
                PlaceId = existingPlace.PlaceId,
                Taste = placeReview.Taste,
                Service = placeReview.Service,
                Mood = placeReview.Mood,
                Price = placeReview.Price,
                Amount = placeReview.Amount,
                StarRating = placeReview.StarRating,
                ReviewText = placeReview.ReviewText,
                RecommendationCount = 0,
                InsertDate = DateTime.UtcNow
            };

            _context.PlaceReview.Add(newPlaceReview);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok,
                Message = "리뷰 추가에 성공했습니다."
            });
        }

        // DELETE: PlaceReview/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PlaceReview>> DeletePlaceReview(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.Fail,
                    Message = "유저 정보를 찾을 수 없습니다."
                });
            }

            var existingPlaceReview = await _context.PlaceReview
                .Where(p => p.PlaceReviewId == id && p.UserId == user.Id)
                .FirstOrDefaultAsync();
            if (existingPlaceReview == null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.ReviewNotExists,
                    Message = "리뷰 아이디와 일치하는 리뷰 정보가 존재하지 않습니다."
                });
            }

            _context.PlaceReview.Remove(existingPlaceReview);
            await _context.SaveChangesAsync();

            return Ok(new CommonResponse
            {
                Code = ResultCode.Ok,
                Message = "리뷰가 삭제되었습니다."
            });
        }
    }
}
