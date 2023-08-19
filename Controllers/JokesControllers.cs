using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using JokeApp.Data;
using JokeApp.Models;

namespace JokeApp.Controllers
{
  public class JokesController : Controller
  {
    private readonly ApplicationDbContext _context;
    public JokesController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: Jokes
    public async Task<IActionResult> Index()
    {
      return View(await _context.Jokes.ToListAsync());
    }

    // GET: Jokes/ShowSearchForm
    public IActionResult ShowSearchForm()
    {
      return View();
    }

    // POST: Jokes/ShowSearchResults
    public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
    {
      return View("Index", await _context.Jokes.Where(j => j.JokeQuestion.Contains(SearchPhrase)).ToListAsync());
    }

    // GET: Jokes/Create
    [Authorize]
    public IActionResult Create()
    {
      return View();
    }

    // POST: Jokes/Create
    // [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([Bind("Id,JokeQuestion,JokeAnswer")] Joke joke)
    {
      if(ModelState.IsValid)
      {
        _context.Add(joke);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(joke);
    }

    // GET: Jokes/Details/{Id}
    public async Task<IActionResult> Details(int? id)
    {
      if(id == null)
      {
        return NotFound();
      }

      var joke = await _context.Jokes.FirstOrDefaultAsync(m => m.Id == id);

      if(joke == null)
      {
        return NotFound();
      }

      return View(joke);
    }

    // GET: Jokes/Edit/{Id}
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
      if(id == null)
      {
        return NotFound();
      }

      var joke = await _context.Jokes.FindAsync(id);
      if(joke == null)
      {
        return NotFound();
      }
      else
        return View(joke);
    }

    // POST: Jokes/Edit/{Id}
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,JokeQuestion,JokeAnswer")] Joke joke)
    {
      if(id != joke.Id)
      {
        return NotFound();
      }

      if(ModelState.IsValid)
      {
        try
        {
          _context.Update(joke);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if(!JokeExists(joke.Id))
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
      return View(joke);
    }

    // GET: Jokes/Delete/{Id}
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
      if(id == null)
      {
        return NotFound();
      }

      var joke = await _context.Jokes.FirstOrDefaultAsync(m => m.Id == id);
      if(joke == null)
      {
        return NotFound();
      }
      else
        return View(joke);
    }

    // POST: Jokes/Delete/{Id}
    [Authorize]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
      var joke = await _context.Jokes.FindAsync(id);

      if(joke == null)
      {
        return NotFound();
      }
      
      _context.Jokes.Remove(joke);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool JokeExists(int id)
    {
      return _context.Jokes.Any(e => e.Id == id);
    }
  }
}