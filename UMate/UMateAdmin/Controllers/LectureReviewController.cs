using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMateModel.Contexts;
using UMateModel.Entities.UMateBoard;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoardAdmin.Controllers
{
    public class LectureReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LectureReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LectureReview
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LectureReview.Include(l => l.LectureInfo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LectureReview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lectureReview = await _context.LectureReview
                .Include(l => l.LectureInfo)
                .FirstOrDefaultAsync(m => m.LectureReviewId == id);
            if (lectureReview == null)
            {
                return NotFound();
            }

            return View(lectureReview);
        }

        // GET: LectureReview/Create
        public IActionResult Create()
        {
            ViewData["LectureInfoId"] = new SelectList(_context.LectureInfo, "LectureInfoId", "LectureInfoId");//3번째가 실제로 표시할 값
            return View();
        }


        // 위의 것이 작성 화면에 표시되는 데이터이고
        // 아래거는 추가한 데이터에 대한 내용인건가?


        // POST: LectureReview/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LectureReview lectureReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lectureReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LectureInfoId"] = new SelectList(_context.LectureInfo, "LectureInfoId", "Semesters", lectureReview.LectureInfoId);
            return View(lectureReview);
        }





        // GET: LectureReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lectureReview = await _context.LectureReview.FindAsync(id);
            if (lectureReview == null)
            {
                return NotFound();
            }
            ViewData["LectureInfoId"] = new SelectList(_context.LectureInfo, "LectureInfoId", "Semesters", lectureReview.LectureInfoId);
            return View(lectureReview);
        }

        // POST: LectureReview/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LectureReviewId,UserId,LectureInfoId,Assignment,GroupMeeting,Evaluation,Attendance,TestNumber,Rating,Semester,Content,CreatedAt,UpdatedAt")] LectureReview lectureReview)
        {
            if (id != lectureReview.LectureReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lectureReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectureReviewExists(lectureReview.LectureReviewId))
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
            ViewData["LectureInfoId"] = new SelectList(_context.LectureInfo, "LectureInfoId", "Semesters", lectureReview.LectureInfoId);
            return View(lectureReview);
        }

        // GET: LectureReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lectureReview = await _context.LectureReview
                .Include(l => l.LectureInfo)
                .FirstOrDefaultAsync(m => m.LectureReviewId == id);
            if (lectureReview == null)
            {
                return NotFound();
            }

            return View(lectureReview);
        }

        // POST: LectureReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lectureReview = await _context.LectureReview.FindAsync(id);
            _context.LectureReview.Remove(lectureReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LectureReviewExists(int id)
        {
            return _context.LectureReview.Any(e => e.LectureReviewId == id);
        }
    }
}
