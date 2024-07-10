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
    public class SessionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Session
        public async Task<IActionResult> Index()
        {
			var applicationDbContext = _context.Dictionaries.Include(d => d.Words).Include(d => d.DictionaryLevelValues);
			return View(await applicationDbContext.ToListAsync());
		}

        [HttpGet]
        public async Task<IActionResult> Study(int? id)
        {
            var words = await _context.Words.Include(w => w.Dictionary).Where(d => d.DictionaryId == id).ToListAsync();
            var randomWord = GetRandomWord(words);

            // Początkowe wartości dla GoodAnswers i AllAnswers
            ViewBag.GoodAnswers = 0;
            ViewBag.AllAnswers = 0;
            ViewBag.RandomWord = randomWord;
            ViewBag.Id = id;

            return View(words);
        }

        [HttpPost]
        public async Task<IActionResult> Study(List<Words> words, string action, int? randomWordId, int goodAnswers, int allAnswers, int id)
        {
            allAnswers++; // Zwiększ AllAnswers za każdym razem, gdy jest przesyłane

            if (action == "RemoveRandomWord" && randomWordId.HasValue)
            {
                var wordToRemove = words.FirstOrDefault(w => w.Id == randomWordId.Value);
                if (wordToRemove != null)
                {
                    words.Remove(wordToRemove);
                    goodAnswers++; // Zwiększ GoodAnswers tylko, gdy słowo jest usunięte

                    if (!words.Any())
                    {
                        // Dodawanie statystyk sesji, gdy lista słów jest pusta
                        var Statistics = new Statistics
                        {
                            SessionDate = DateTime.Now,
                            GoodAnswers = goodAnswers,
                            AllAnswers = allAnswers,
                            Percentage = ((double)goodAnswers / allAnswers).ToString("P"), // Obliczanie procentu
                            DictionaryId = id, // Przykładowa wartość, dostosuj według potrzeb
                            UserId = 1 // Przykładowa wartość, dostosuj według potrzeb
                        };

                        _context.Statistics.Add(Statistics);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Details", "Statistics", new { id = Statistics.Id });
                    }
                }
            }

            var randomWord = GetRandomWord(words);
            ViewBag.RandomWord = randomWord;
            ViewBag.GoodAnswers = goodAnswers;
            ViewBag.AllAnswers = allAnswers;
            ViewBag.Id = id;

            return View(words);
        }

        private Words GetRandomWord(List<Words> words)
        {
            if (words.Any())
            {
                var random = new Random();
                int index = random.Next(words.Count);
                return words[index];
            }
            return null;
        }

    }
}
