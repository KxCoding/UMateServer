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
    public class ExampleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExampleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Example
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Example.Include(e => e.TestInfo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Example/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var example = await _context.Example
                .Include(e => e.TestInfo)
                .FirstOrDefaultAsync(m => m.ExampleId == id);
            if (example == null)
            {
                return NotFound();
            }

            return View(example);
        }

        // GET: Example/Create
        public IActionResult Create()
        {
            ViewData["TestInfoId"] = new SelectList(_context.TestInfo, "TestInfoId", "TestInfoId");
            return View();
        }

        // POST: Example/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExampleId,TestInfoId,Content")] Example example)
        {
            if (ModelState.IsValid)
            {
                _context.Add(example);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TestInfoId"] = new SelectList(_context.TestInfo, "TestInfoId", "QuestionTypes", example.TestInfoId);
            return View(example);
        }

        // GET: Example/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var example = await _context.Example.FindAsync(id);
            if (example == null)
            {
                return NotFound();
            }
            ViewData["TestInfoId"] = new SelectList(_context.TestInfo, "TestInfoId", "QuestionTypes", example.TestInfoId);
            return View(example);
        }

        // POST: Example/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExampleId,TestInfoId,Content")] Example example)
        {
            if (id != example.ExampleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(example);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExampleExists(example.ExampleId))
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
            ViewData["TestInfoId"] = new SelectList(_context.TestInfo, "TestInfoId", "QuestionTypes", example.TestInfoId);
            return View(example);
        }

        // GET: Example/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var example = await _context.Example
                .Include(e => e.TestInfo)
                .FirstOrDefaultAsync(m => m.ExampleId == id);
            if (example == null)
            {
                return NotFound();
            }

            return View(example);
        }

        // POST: Example/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var example = await _context.Example.FindAsync(id);
            _context.Example.Remove(example);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExampleExists(int id)
        {
            return _context.Example.Any(e => e.ExampleId == id);
        }
    }
}
