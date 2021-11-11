using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities.UMateBoard;

namespace BoardAdmin.Controllers
{
    public class TestInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestInfo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TestInfo.Include(t => t.LectureInfo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TestInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testInfo = await _context.TestInfo
                .Include(t => t.LectureInfo)
                .FirstOrDefaultAsync(m => m.TestInfoId == id);
            if (testInfo == null)
            {
                return NotFound();
            }

            return View(testInfo);
        }

        // GET: TestInfo/Create
        public IActionResult Create()
        {
            ViewData["LectureInfoId"] = new SelectList(_context.LectureInfo, "LectureInfoId", "LectureInfoId");
            return View();
        }

        // POST: TestInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestInfoId,UserId,LectureInfoId,Semester,TestType,TestStrategy,QuestionTypes,CreatedAt,UpdatedAt")] TestInfo testInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LectureInfoId"] = new SelectList(_context.LectureInfo, "LectureInfoId", "Semesters", testInfo.LectureInfoId);
            return View(testInfo);
        }

        // GET: TestInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testInfo = await _context.TestInfo.FindAsync(id);
            if (testInfo == null)
            {
                return NotFound();
            }
            ViewData["LectureInfoId"] = new SelectList(_context.LectureInfo, "LectureInfoId", "Semesters", testInfo.LectureInfoId);
            return View(testInfo);
        }

        // POST: TestInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestInfoId,UserId,LectureInfoId,Semester,TestType,TestStrategy,QuestionTypes,CreatedAt,UpdatedAt")] TestInfo testInfo)
        {
            if (id != testInfo.TestInfoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestInfoExists(testInfo.TestInfoId))
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
            ViewData["LectureInfoId"] = new SelectList(_context.LectureInfo, "LectureInfoId", "Semesters", testInfo.LectureInfoId);
            return View(testInfo);
        }

        // GET: TestInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testInfo = await _context.TestInfo
                .Include(t => t.LectureInfo)
                .FirstOrDefaultAsync(m => m.TestInfoId == id);
            if (testInfo == null)
            {
                return NotFound();
            }

            return View(testInfo);
        }

        // POST: TestInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testInfo = await _context.TestInfo.FindAsync(id);
            _context.TestInfo.Remove(testInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestInfoExists(int id)
        {
            return _context.TestInfo.Any(e => e.TestInfoId == id);
        }
    }
}
