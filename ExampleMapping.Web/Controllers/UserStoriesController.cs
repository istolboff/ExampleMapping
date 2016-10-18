using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExampleMapping.Web.Models;

namespace ExampleMapping.Web.Controllers
{
    public sealed class UserStoriesController : Controller
    {
        public UserStoriesController(ExampleMappingContext exampleMappingContext)
        {
            Contract.Requires(exampleMappingContext != null);

            _exampleMappingContext = exampleMappingContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _exampleMappingContext.UserStories.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(ulong? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foundUserStory = await _exampleMappingContext.FindUserStoryById(id.Value);
            if (foundUserStory == null)
            {
                return NotFound();
            }

            return View(foundUserStory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserStory userStory)
        {
            if (ModelState.IsValid)
            {
                _exampleMappingContext.UserStories.Add(userStory);
                _exampleMappingContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userStory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ulong userStoryId, UserStory userStory)
        {
            if (userStoryId != userStory.UserStoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _exampleMappingContext.Update(userStory);
                    await _exampleMappingContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _exampleMappingContext.FindUserStoryById(userStoryId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            return View(userStory);
        }

        private readonly ExampleMappingContext _exampleMappingContext;
    }
}
