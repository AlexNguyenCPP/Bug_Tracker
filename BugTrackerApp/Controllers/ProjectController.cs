using BugTrackerApp.Data;
using BugTrackerApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerApp.Controllers
{
    public class ProjectController : Controller
    {
        // retrieve project list
        private readonly ApplicationDbContext _db;
        public ProjectController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Project> objProjectList = _db.Projects;
            return View(objProjectList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //prevent cross-site request forgery. Not required but recommended
        public IActionResult Create(Project obj)
        {
            // make sure the description and the name are not the same
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The name cannot match the description");
            }

            // server side validation
            if (ModelState.IsValid)
            {
                // add the created project to the database
                _db.Projects.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");// if returning to an action method inside a different controller, do this
            }                                    // return RedirectToAction("Index", "<controller-name");

            return View(obj);
        }
    }
}
