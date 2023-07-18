using BugTrackerApp.Data;
using BugTrackerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerApp.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
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
                TempData["success"] = "Project created successfully";
                return RedirectToAction("Index");// if returning to an action method inside a different controller, do this
            }                                    // return RedirectToAction("Index", "<controller-name");

            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            // validation check
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var projectFromDb = _db.Projects.Find(id);
            //var projectFromDbFirst = _db.Projects.FirstOrDefault(u=>u.Id == id);
            //var projectFromDbSingle = _db.Projects.SingleOrDefault(u => u.Id == id);

            // check if the project with received id exists
            if (projectFromDb == null)
            {
                return NotFound();
            }

            return View(projectFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //prevent cross-site request forgery. Not required but recommended
        public IActionResult Edit(Project obj)
        {
            // make sure the description and the name are not the same
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The name cannot match the description");
            }

            // server side validation
            if (ModelState.IsValid)
            {
                // update the created project in the database
                _db.Projects.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Project updated successfully";
                return RedirectToAction("Index");// if returning to an action method inside a different controller, do this
            }                                    // return RedirectToAction("Index", "<controller-name");

            return View(obj);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            // validation check
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var projectFromDb = _db.Projects.Find(id);
            //var projectFromDbFirst = _db.Projects.FirstOrDefault(u=>u.Id == id);
            //var projectFromDbSingle = _db.Projects.SingleOrDefault(u => u.Id == id);

            // check if the project with received id exists
            if (projectFromDb == null)
            {
                return NotFound();
            }

            return View(projectFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")] // this allows you to simply call the post action method "delete" in the delete view
        [ValidateAntiForgeryToken] //prevent cross-site request forgery. Not required but recommended
        public IActionResult DeletePOST(int? id)    //GET and POST methods can't have exact same signatures, hence "DeletePOST"
        {
            // check if the project exists
            var obj = _db.Projects.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            
            // delete the found project in the database
            _db.Projects.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Project deleted successfully";
            return RedirectToAction("Index");// if returning to an action method inside a different controller, do this
                                                // return RedirectToAction("Index", "<controller-name>")
        }

        // view project details
        public IActionResult Details(int id, string origin)
        {
            // Include is necessary here because only simple types like ints, strings, etc would be returned otherwise
            var project = _db.Projects.Include(p => p.Tickets).SingleOrDefault(p => p.Id == id);
            
            if (project == null)
            {
                return NotFound();
            }

            ViewBag.Origin = origin;
            return View(project);
        }

    }
}
