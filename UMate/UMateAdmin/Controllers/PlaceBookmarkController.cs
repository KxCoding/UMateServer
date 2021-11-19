using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities;
using UMateModel.Entities.Place;

namespace UMateAdmin.Controllers
{
    public class PlaceBookmarkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlaceBookmarkController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlaceBookmark
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PlaceBookmark
                .Include(p => p.Place)
                .OrderBy(p => p.UserId)
                .ThenBy(p => p.InsertDate);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PlaceBookmark/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placeBookmark = await _context.PlaceBookmark
                .Include(p => p.Place)
                .FirstOrDefaultAsync(m => m.PlaceBookmarkId == id);

            if (placeBookmark == null)
            {
                return NotFound();
            }

            return View(placeBookmark);
        }

        // GET: PlaceBookmark/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["PlaceId"] = new SelectList(_context.Place, "PlaceId", "Name");
            return View();
        }

        // POST: PlaceBookmark/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,PlaceId")] PlaceBookmark placeBookmark)
        {
            var existingBookmark = await _context.PlaceBookmark
                .FirstOrDefaultAsync(b => b.PlaceId == placeBookmark.PlaceId && b.UserId == placeBookmark.UserId);

            // 이미 존재한다면 return
            if (existingBookmark != null)
            {
                ViewData["PlaceId"] = new SelectList(_context.Place, "PlaceId", "Name", placeBookmark.PlaceId);
                return View(placeBookmark);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == placeBookmark.UserId);

            var place = await _context.Place
                .FirstOrDefaultAsync(p => p.PlaceId == placeBookmark.PlaceId);

            if (user != null && place != null)
            {
                existingBookmark = new PlaceBookmark
                {
                    UserId = placeBookmark.UserId,
                    PlaceId = placeBookmark.PlaceId,
                    Place = place,
                    InsertDate = DateTime.UtcNow
                };

                if (ModelState.IsValid)
                {
                    _context.Add(existingBookmark);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["PlaceId"] = new SelectList(_context.Place, "PlaceId", "Name", placeBookmark.PlaceId);
            return View(placeBookmark);
        }

        /*
        // GET: PlaceBookmark/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placeBookmark = await _context.PlaceBookmark.FindAsync(id);

            if (placeBookmark == null)
            {
                return NotFound();
            }

            ViewData["PlaceId"] = new SelectList(_context.Place, "PlaceId", "Name", placeBookmark.PlaceId);
            return View(placeBookmark);
        }

        // POST: PlaceBookmark/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaceBookmarkId,UserId,PlaceId,InsertDate")] PlaceBookmark placeBookmark)
        {
            if (id != placeBookmark.PlaceBookmarkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(placeBookmark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceBookmarkExists(placeBookmark.PlaceBookmarkId))
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
            ViewData["PlaceId"] = new SelectList(_context.Place, "PlaceId", "Name", placeBookmark.PlaceId);
            return View(placeBookmark);
        }
        */

        // GET: PlaceBookmark/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placeBookmark = await _context.PlaceBookmark
                .Include(p => p.Place)
                .FirstOrDefaultAsync(m => m.PlaceBookmarkId == id);

            if (placeBookmark == null)
            {
                return NotFound();
            }

            return View(placeBookmark);
        }

        // POST: PlaceBookmark/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placeBookmark = await _context.PlaceBookmark.FindAsync(id);

            _context.PlaceBookmark.Remove(placeBookmark);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceBookmarkExists(int id)
        {
            return _context.PlaceBookmark.Any(e => e.PlaceBookmarkId == id);
        }
    }
}
