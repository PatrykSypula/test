using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNET_EF.Data;
using ASPNET_EF.Models;

namespace ASPNET.Controllers
{
    public class WordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Words
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dictionaries.Include(d => d.Words).Include(d => d.DictionaryLevelValues);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Words/Create
        public IActionResult Create()
        {
            ViewData["DictionaryId"] = new SelectList(_context.Dictionaries, "Id", "Id");
            return View();
        }

        // POST: Words/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WordPolish,WordTranslated,DictionaryId")] Words words)
        {
            if (ModelState.IsValid)
            {
                _context.Add(words);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditDictionary", new { id = words.DictionaryId });
            }
            ViewData["DictionaryId"] = new SelectList(_context.Dictionaries, "Id", "Id", words.DictionaryId);
            return View(words);
        }

		// GET: Words/Edit/5
		public async Task<IActionResult> EditDictionary(int? id)
        {
			var applicationDbContext = _context.Words.Include(w => w.Dictionary).Where(w => w.DictionaryId == id);
            return View(await applicationDbContext.ToListAsync());
		}

		// GET: Words/Edit/5
		public async Task<IActionResult> EditWord(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var words = await _context.Words.FindAsync(id);
            if (words == null)
            {
                return NotFound();
            }
            ViewData["DictionaryId"] = new SelectList(_context.Dictionaries, "Id", "Id", words.DictionaryId);
            return View(words);
        }

        // POST: Words/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWord(int id, [Bind("Id,WordPolish,WordTranslated,DictionaryId")] Words words)
        {
            if (id != words.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(words);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordsExists(words.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("EditDictionary", new { id = words.DictionaryId });
            }
            ViewData["DictionaryId"] = new SelectList(_context.Dictionaries, "Id", "Id", words.DictionaryId);
            return View(words);
        }

        // GET: Words/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var words = await _context.Words
                .Include(w => w.Dictionary)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (words == null)
            {
                return NotFound();
            }

            return View(words);
        }

        // POST: Words/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var words = await _context.Words.FindAsync(id);
            if (words != null)
            {
                _context.Words.Remove(words);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("EditDictionary", new { id = words.DictionaryId });
        }

        private bool WordsExists(int id)
        {
            return _context.Words.Any(e => e.Id == id);
        }
    }
}
