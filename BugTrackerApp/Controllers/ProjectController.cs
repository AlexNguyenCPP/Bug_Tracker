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
    }
}
