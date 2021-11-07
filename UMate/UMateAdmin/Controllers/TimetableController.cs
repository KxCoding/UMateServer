using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.Contexts;
using UMateModel.Entities.Timetable;

namespace UMateAdmin.Controllers
{
    public class TimetableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimetableController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Timetable
        public async Task<IActionResult> Index()
        {
            return View(await _context.Timetable.ToListAsync());
        }

        // GET: Timetable/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetable
                .FirstOrDefaultAsync(m => m.TimetableId == id);
            if (timetable == null)
            {
                return NotFound();
            }

            return View(timetable);
        }

        // GET: Timetable/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Timetable/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,TimetableId,CourseId,CourseName,RoomName,ProfessorName,CourseDay,StartTime,EndTime,BackgroundColor")] Timetable timetable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timetable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timetable);
        }

        // GET: Timetable/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetable.FindAsync(id);
            if (timetable == null)
            {
                return NotFound();
            }
            return View(timetable);
        }

        // POST: Timetable/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,TimetableId,CourseId,CourseName,RoomName,ProfessorName,CourseDay,StartTime,EndTime,BackgroundColor")] Timetable timetable)
        {
            if (id != timetable.TimetableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timetable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimetableExists(timetable.TimetableId))
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
            return View(timetable);
        }

        // GET: Timetable/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetable
                .FirstOrDefaultAsync(m => m.TimetableId == id);
            if (timetable == null)
            {
                return NotFound();
            }

            return View(timetable);
        }

        // POST: Timetable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timetable = await _context.Timetable.FindAsync(id);
            _context.Timetable.Remove(timetable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimetableExists(int id)
        {
            return _context.Timetable.Any(e => e.TimetableId == id);
        }
    }
}
