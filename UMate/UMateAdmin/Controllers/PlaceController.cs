using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using UMateAdmin;
using UMateAdmin.Service;
using UMateModel.Contexts;
using UMateModel.Entities;
using UMateModel.Entities.Place;
using X.PagedList;

namespace UMateAdmin.Controllers
{
    //[Authorize]
    public class PlaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlaceController(ApplicationDbContext context)
        {
            _context = context;
        }

        private StorageManager StorageManager = StorageManager.Shared;

        private CloudBlobContainer PlaceContainer = StorageManager.GetContainer("default");

        // GET: Place
        public async Task<IActionResult> Index(int? page)
        {
            var applicationDbContext = _context.Place
                .Include(p => p.University)
                .OrderBy(p => p.Type)
                .ThenBy(p => p.Name);

            return View(await applicationDbContext.ToPagedListAsync(page ?? 1, 20));
        }

        // GET: Place/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Place
                .Where(m => m.PlaceId == id)
                .FirstOrDefaultAsync();

            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // GET: Place/Create
        public IActionResult Create()
        {
            ViewData["UniversityId"] = new SelectList(_context.University, "UniversityId", "Name");
            return View();
        }

        // POST: Place/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Place place)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(place.UniversityId);
                // thumbnail image
                if (place.ThumbnailFile != null)
                {
                    // 업로드된 파일이 있다면 url을 생성하고 이를 place 인스턴스에 저장합니다.
                    var url = await StorageManager.UploadFile(PlaceContainer, place.ThumbnailFile);

                    if (url != null)
                    {
                        place.ThumbnailImageUrl = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place thumbnail image");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload - thumbnail image");
                }


                // place image 0
                if (place.PlaceImageFile0 != null)
                {
                    // 업로드된 파일이 있다면 url을 생성하고 이를 place 인스턴스에 저장합니다.
                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile0);

                    if (url != null)
                    {
                        place.PlaceImageUrl0 = url;
                    }
                    else
                    { 
                        Console.WriteLine("fail to upload place image 0");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload - place image 0");
                }

                // place image 1
                if (place.PlaceImageFile1 != null)
                {
                    // 업로드된 파일이 있다면 url을 생성하고 이를 place 인스턴스에 저장합니다.
                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile1);

                    if (url != null)
                    {
                        place.PlaceImageUrl1 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image 1");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload - place image 1");
                }

                // place image 2
                if (place.PlaceImageFile2 != null)
                {
                    // 업로드된 파일이 있다면 url을 생성하고 이를 place 인스턴스에 저장합니다.
                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile2);

                    if (url != null)
                    {
                        place.PlaceImageUrl2 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image 2");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload - place image 2");
                }

                // place image 3
                if (place.PlaceImageFile3 != null)
                {
                    // 업로드된 파일이 있다면 url을 생성하고 이를 place 인스턴스에 저장합니다.
                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile3);

                    if (url != null)
                    {
                        place.PlaceImageUrl3 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image 3");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload - place image 3");
                }

                // place image 4
                if (place.PlaceImageFile4 != null)
                {
                    // 업로드된 파일이 있다면 url을 생성하고 이를 place 인스턴스에 저장합니다.
                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile4);

                    if (url != null)
                    {
                        place.PlaceImageUrl4 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image 4");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload - place image 4");
                }

                // place image 5
                if (place.PlaceImageFile5 != null)
                {
                    // 업로드된 파일이 있다면 url을 생성하고 이를 place 인스턴스에 저장합니다.
                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile5);

                    if (url != null)
                    {
                        place.PlaceImageUrl5 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image 5");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload - place image 5");
                }

                _context.Add(place);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UniversityId"] = new SelectList(_context.University, "UniversityId", "Name", place.UniversityId);
            return View(place);
        }

        // GET: Place/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Place.FindAsync(id);

            if (place == null)
            {
                return NotFound();
            }

            ViewData["UniversityId"] = new SelectList(_context.University, "UniversityId", "Name", place.UniversityId);
            return View(place);
        }

        // POST: Place/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Place place)
        {
            if (id != place.PlaceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (place.ThumbnailFile != null)
                {
                    if (place.ThumbnailImageUrl != null)
                    {
                        // 이미 저장된 파일이 있고, 새로 업로드된 파일도 있다면 기존 파일을 삭제합니다.
                        await StorageManager.DeleteFile(PlaceContainer, place.ThumbnailImageUrl);
                    }

                    var url = await StorageManager.UploadFile(PlaceContainer, place.ThumbnailFile);

                    if (url != null)
                    {
                        place.ThumbnailImageUrl = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload");
                }

                if (place.PlaceImageFile0 != null)
                {
                    if (place.PlaceImageUrl0 != null)
                    {
                        // 이미 저장된 파일이 있고, 새로 업로드된 파일도 있다면 기존 파일을 삭제합니다.
                        await StorageManager.DeleteFile(PlaceContainer, place.PlaceImageUrl0);
                    }

                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile0);

                    if (url != null)
                    {
                        place.PlaceImageUrl0 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload");
                }

                if (place.PlaceImageFile1 != null)
                {
                    if (place.PlaceImageUrl1 != null)
                    {
                        // 이미 저장된 파일이 있고, 새로 업로드된 파일도 있다면 기존 파일을 삭제합니다.
                        await StorageManager.DeleteFile(PlaceContainer, place.PlaceImageUrl1);
                    }

                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile1);

                    if (url != null)
                    {
                        place.PlaceImageUrl1 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload");
                }

                if (place.PlaceImageFile2 != null)
                {
                    if (place.PlaceImageUrl2 != null)
                    {
                        // 이미 저장된 파일이 있고, 새로 업로드된 파일도 있다면 기존 파일을 삭제합니다.
                        await StorageManager.DeleteFile(PlaceContainer, place.PlaceImageUrl2);
                    }

                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile2);

                    if (url != null)
                    {
                        place.PlaceImageUrl2 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload");
                }

                if (place.PlaceImageFile3 != null)
                {
                    if (place.PlaceImageUrl3 != null)
                    {
                        // 이미 저장된 파일이 있고, 새로 업로드된 파일도 있다면 기존 파일을 삭제합니다.
                        await StorageManager.DeleteFile(PlaceContainer, place.PlaceImageUrl3);
                    }

                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile3);

                    if (url != null)
                    {
                        place.PlaceImageUrl3 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload");
                }

                if (place.PlaceImageFile4 != null)
                {
                    if (place.PlaceImageUrl4 != null)
                    {
                        // 이미 저장된 파일이 있고, 새로 업로드된 파일도 있다면 기존 파일을 삭제합니다.
                        await StorageManager.DeleteFile(PlaceContainer, place.PlaceImageUrl4);
                    }

                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile4);

                    if (url != null)
                    {
                        place.PlaceImageUrl4 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload");
                }

                if (place.PlaceImageFile5 != null)
                {
                    if (place.PlaceImageUrl5 != null)
                    {
                        // 이미 저장된 파일이 있고, 새로 업로드된 파일도 있다면 기존 파일을 삭제합니다.
                        await StorageManager.DeleteFile(PlaceContainer, place.PlaceImageUrl5);
                    }

                    var url = await StorageManager.UploadFile(PlaceContainer, place.PlaceImageFile5);

                    if (url != null)
                    {
                        place.PlaceImageUrl5 = url;
                    }
                    else
                    {
                        Console.WriteLine("fail to upload place image");
                    }
                }
                else
                {
                    Console.WriteLine("no image file to upload");
                }

                try
                {
                    _context.Update(place);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceExists(place.PlaceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // 목록 화면으로 돌아갑니다. 
                return RedirectToAction(nameof(Index));
            }

            ViewData["UniversityId"] = new SelectList(_context.University, "UniversityId", "Name", place.UniversityId);
            return View(place);
        }

        // GET: Place/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Place
                .Include(p => p.University)
                .FirstOrDefaultAsync(m => m.PlaceId == id);

            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // POST: Place/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var place = await _context.Place.FindAsync(id);

            string[] imageUrls = { place.ThumbnailImageUrl, place.PlaceImageUrl0, place.PlaceImageUrl1, place.PlaceImageUrl2, place.PlaceImageUrl3, place.PlaceImageUrl4, place.PlaceImageUrl5 };

            foreach (string url in imageUrls)
            {
                if (url != null)
                {
                    _ = StorageManager.DeleteFile(PlaceContainer, url);
                }
            }

            _context.Place.Remove(place);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PlaceExists(int id)
        {
            return _context.Place.Any(p => p.PlaceId == id);
        }

    }
}
 