using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities.Place;
using UMateModel.Models;
using UMateModel.Models.UMatePlace;

namespace UMateApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AllPlaceReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AllPlaceReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 전체 상점 리뷰 데이터를 가져옵니다.
        /// </summary>
        /// <returns> 상점 리뷰 목록 응답 객체 </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaceReview>>> GetPlaceReview()
        {
            var list = await _context.PlaceReview
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
    }
}