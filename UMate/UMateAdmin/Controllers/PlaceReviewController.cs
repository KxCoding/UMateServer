using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities.Place;
using X.PagedList;

namespace UmateAdmin.Controllers
{
    public class PlaceReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlaceReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlaceReview
        public async Task<IActionResult> Index(int? page)
        {
            var applicationDbContext = _context.PlaceReview.Include(p => p.Place);

            return View(await applicationDbContext.ToPagedListAsync(page ?? 1, 20));
        }

        // GET: PlaceReview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placeReview = await _context.PlaceReview
                .Include(p => p.Place)
                .FirstOrDefaultAsync(m => m.PlaceReviewId == id);
            if (placeReview == null)
            {
                return NotFound();
            }

            return View(placeReview);
        }

        // GET: PlaceReview/Create
        public IActionResult Create()
        {
            ViewData["PlaceId"] = new SelectList(_context.Place, "PlaceId", "Name");
            return View();
        }

        // POST: PlaceReview/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaceReviewId,UserId,PlaceId,Taste,Service,Mood,Price,Amount,StarRating,ReviewText,RecommendationCount,InsertDate,UpdateDate")] PlaceReview placeReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(placeReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceId"] = new SelectList(_context.Place, "PlaceId", "Name", placeReview.PlaceId);
            return View(placeReview);
        }

        // GET: PlaceReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placeReview = await _context.PlaceReview.FindAsync(id);
            if (placeReview == null)
            {
                return NotFound();
            }
            ViewData["PlaceId"] = new SelectList(_context.Place, "PlaceId", "Name", placeReview.PlaceId);
            return View(placeReview);
        }

        // POST: PlaceReview/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaceReviewId,UserId,PlaceId,Taste,Service,Mood,Price,Amount,StarRating,ReviewText,RecommendationCount,InsertDate,UpdateDate")] PlaceReview placeReview)
        {
            if (id != placeReview.PlaceReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(placeReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceReviewExists(placeReview.PlaceReviewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceId"] = new SelectList(_context.Place, "PlaceId", "Name", placeReview.PlaceId);
            return View(placeReview);
        }

        // GET: PlaceReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placeReview = await _context.PlaceReview
                .Include(p => p.Place)
                .FirstOrDefaultAsync(m => m.PlaceReviewId == id);
            if (placeReview == null)
            {
                return NotFound();
            }

            return View(placeReview);
        }

        // POST: PlaceReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placeReview = await _context.PlaceReview.FindAsync(id);
            _context.PlaceReview.Remove(placeReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceReviewExists(int id)
        {
            return _context.PlaceReview.Any(e => e.PlaceReviewId == id);
        }
    }
}
