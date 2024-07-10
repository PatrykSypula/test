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
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SessionStatistics
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dictionaries.Include(d => d.Words).Include(d => d.DictionaryLevelValues).Include(d => d.Statistics);
            return View(await applicationDbContext.ToListAsync());
        }
        
        // GET: SessionStatistics
        public async Task<IActionResult> Dictionary(int? id)
        {
            var applicationDbContext = _context.Statistics.Include(s => s.Dictionary).Where(s => s.DictionaryId == id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SessionStatistics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionStatistics = await _context.Statistics
                .Include(s => s.Dictionary)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sessionStatistics == null)
            {
                return NotFound();
            }

            return View(sessionStatistics);
        }

        // GET: SessionStatistics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionStatistics = await _context.Statistics
                .Include(s => s.Dictionary)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sessionStatistics == null)
            {
                return NotFound();
            }

            return View(sessionStatistics);
        }

        // POST: SessionStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessionStatistics = await _context.Statistics.FindAsync(id);
            if (sessionStatistics != null)
            {
                _context.Statistics.Remove(sessionStatistics);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionStatisticsExists(int id)
        {
            return _context.Statistics.Any(e => e.Id == id);
        }
    }
}
