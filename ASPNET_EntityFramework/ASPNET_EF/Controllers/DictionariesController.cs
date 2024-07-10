using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNET_EF.Models;
using ASPNET_EF.Data;
using System.Data;


namespace ASPNET.Controllers
{
	public class DictionariesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DictionariesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Dictionaries
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Dictionaries.Include(d => d.Words).Include(d => d.DictionaryLevelValues);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: Dictionaries/Details/5
		//public async Task<IActionResult> Details(int? id)
		//{
		//	if (id == null)
		//	{
		//		return NotFound();
		//	}

		//	var dictionaries = await _context.Dictionaries
		//		.Include(d => d.DictionaryLevelValues)
		//		.Include(d => d.User)
		//		.FirstOrDefaultAsync(m => m.Id == id);
		//	if (dictionaries == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(dictionaries);
		//}

		// GET: Dictionaries/Create
		public IActionResult Create()
		{
			ViewData["DictionaryLevelId"] = new SelectList(_context.Set<DictionaryLevelValues>(), "Id", "Id");
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
			return View();
		}

		// POST: Dictionaries/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost, ActionName("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,DictionaryName,DictionaryLevelId,DictionaryDescription,IsDefaultDictionary,IsPublic,UserId")] Dictionaries dictionaries)
		{
			if (ModelState.IsValid)
			{
				_context.Add(dictionaries);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["DictionaryLevelId"] = new SelectList(_context.Set<DictionaryLevelValues>(), "Id", "Id", dictionaries.DictionaryLevelId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", dictionaries.UserId);
			return View(dictionaries);
		}

		// GET: Dictionaries/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var dictionaries = await _context.Dictionaries.FindAsync(id);
			if (dictionaries == null)
			{
				return NotFound();
			}
			ViewData["DictionaryLevelId"] = new SelectList(_context.Set<DictionaryLevelValues>(), "Id", "Id", dictionaries.DictionaryLevelId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", dictionaries.UserId);
			return View(dictionaries);
		}

		// POST: Dictionaries/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,DictionaryName,DictionaryLevelId,DictionaryDescription,IsDefaultDictionary,IsPublic,UserId")] Dictionaries dictionaries)
		{
			if (id != dictionaries.Id)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(dictionaries);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!DictionariesExists(dictionaries.Id))
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
			ViewData["DictionaryLevelId"] = new SelectList(_context.Set<DictionaryLevelValues>(), "Id", "Id", dictionaries.DictionaryLevelId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", dictionaries.UserId);
			return View(dictionaries);
		}

		// GET: Dictionaries/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var dictionaries = await _context.Dictionaries
				.Include(d => d.DictionaryLevelValues)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (dictionaries == null)
			{
				return NotFound();
			}

			return View(dictionaries);
		}

		// POST: Dictionaries/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dictionaries = await _context.Dictionaries.FindAsync(id);
			if (dictionaries != null)
			{
				_context.Dictionaries.Remove(dictionaries);

				List<Words> wordsList = await _context.Words.Where(w => w.DictionaryId == id).ToListAsync();
				foreach (var word in wordsList)
				{
					_context.Words.Remove(word);
				}
				List<Statistics> statsList = await _context.Statistics.Where(s => s.DictionaryId == id).ToListAsync();
				foreach (var stat in statsList)
				{
					_context.Statistics.Remove(stat);
				}
				List<SubscribedDictionary> subedList = await _context.SubscribedDictionary.Where(s => s.DictionaryId == id).ToListAsync();
				foreach (var sub in subedList)
				{
					_context.SubscribedDictionary.Remove(sub);
				}
			}
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		private bool DictionariesExists(int id)
		{
			return _context.Dictionaries.Any(e => e.Id == id);
		}
	}
}
