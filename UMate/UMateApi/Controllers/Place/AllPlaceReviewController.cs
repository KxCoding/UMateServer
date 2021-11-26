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

        // GET: api/PlaceReviewApi
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