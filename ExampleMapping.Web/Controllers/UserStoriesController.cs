using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View(_exampleMappingContext.UserStories.ToList());
        }

        public IActionResult Create()
        {
            return View();
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

        private readonly ExampleMappingContext _exampleMappingContext;
    }
}
