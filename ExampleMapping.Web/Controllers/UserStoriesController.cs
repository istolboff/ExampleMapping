﻿using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Edit(long id, UserStory userStory)
        {
            if (id != userStory.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(userStory);
            }

            try
            {
                var rules = userStory.Rules ?? new List<Rule>();

                var rulesWithExamples = rules.Where(rule => rule.Examples != null).AsImmutable();
                var exampleIdsToDelete = rulesWithExamples.SelectMany(rule => rule.Examples).GetIdsOfEntitiesMarkedForDeletion();
                foreach (var rule in rulesWithExamples)
                {
                    rule.Examples.RemoveEntitiesMarkedForDeletion();
                }

                var ruleIdsToDelete = rules.GetIdsOfEntitiesMarkedForDeletion();
                rules.RemoveEntitiesMarkedForDeletion();

                var questions = userStory.Questions ?? new List<Question>();
                var questionIdsToDelete = questions.GetIdsOfEntitiesMarkedForDeletion();
                questions.RemoveEntitiesMarkedForDeletion();

                _exampleMappingContext.Update(userStory);

                _exampleMappingContext.Examples.RemoveIf(example => Array.IndexOf(exampleIdsToDelete, example.Id) >= 0);
                _exampleMappingContext.Rules.RemoveIf(rule => Array.IndexOf(ruleIdsToDelete, rule.Id) >= 0);
                _exampleMappingContext.Questions.RemoveIf(question => Array.IndexOf(questionIdsToDelete, question.Id) >= 0);

                await _exampleMappingContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _exampleMappingContext.FindUserStoryById(id) != null)
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
