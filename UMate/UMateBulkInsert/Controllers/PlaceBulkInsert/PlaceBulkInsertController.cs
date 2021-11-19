using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UMateModel.Contexts;
using UMateModel.Models;
using UMateModel.Models.UMatePlace;
using UMateModel.Entities.Place;
//using UMateModel.Models;

namespace UmateBulkInsert.Controllers
{
    [Route("bi/place/place")]
    [ApiController]
    public class PlaceBulkInsertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlaceBulkInsertController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/PlaceApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommonResponse>> PostPost(PlacePostData place)
        {
            // 대학교 검색
            var existingUniversity = _context.University
                .Where(u => u.Name == place.UniversityName)
                .FirstOrDefault();

            // 일치하는 대학이 없다면 기본값 insert
            var universityId = 0; 

            // 이미 존재하는 대학이라면 id 복사
            if (existingUniversity != null)
            {
                universityId = existingUniversity.UniversityId;
            }

            // 상점 검색
            var existingPlace = _context.Place
                .Where(p => p.Name == place.Name)
                .FirstOrDefault();

            // 이미 있는 상점이라면 return
            if (existingPlace != null)
            {
                return Ok(new CommonResponse
                {
                    Code = ResultCode.ExistsAlready,
                    Message = "request data(place) already exists"
                });
            }
            else
            {
                // 없는 상점이라면 생성 후 저장
                var newPlace = new Place
                {
                    Name = place.Name,
                    UniversityId = universityId, // 이름으로 검색한 대학의 id
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
                    Message = "success inserting place" 
                });
            }
        }
    }
}
