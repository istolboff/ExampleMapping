using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Linq;
using ExampleMapping.Web.Miscellaneous;
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

        public async Task<IActionResult> Edit(long? id)
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
        public async Task<IActionResult> Edit(long userStoryId, UserStory userStory)
        {
            if (userStoryId != userStory.UserStoryId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(userStory);
            }

            try
            {
                var rulesWithExamples = userStory.Rules.Where(rule => rule.Examples != null).AsImmutable();
                var exampleIdsToDelete = rulesWithExamples
                    .SelectMany(rule => rule.Examples)
                    .Where(example => example.ExampleId < 0)
                    .Select(example => -example.ExampleId).ToArray();
                foreach (var rule in rulesWithExamples)
                {
                    rule.Examples.RemoveIf(example => example.ExampleId < 0);
                }

                var ruleIdsToDelete = userStory.Rules.Where(rule => rule.RuleId < 0).Select(rule => -rule.RuleId).ToArray();
                userStory.Rules.RemoveIf(rule => rule.RuleId < 0);
                _exampleMappingContext.Update(userStory);
                _exampleMappingContext.Examples.RemoveIf(example => Array.IndexOf(exampleIdsToDelete, example.ExampleId) >= 0);
                _exampleMappingContext.Rules.RemoveIf(rule => Array.IndexOf(ruleIdsToDelete, rule.RuleId) >= 0);
                await _exampleMappingContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _exampleMappingContext.FindUserStoryById(userStoryId) != null)
                {
                    throw;
                }

                return NotFound();
            }

            return RedirectToAction("Index");
        }

        private readonly ExampleMappingContext _exampleMappingContext;
    }
}
