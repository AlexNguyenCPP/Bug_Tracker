using BugTrackerApp.Data;
using BugTrackerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;


namespace BugTrackerApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult ManageUsers()
        {
            var viewModel = new ManageUsersViewModel
            {
                Users = _userManager.Users.ToList(),
                Projects = _context.Projects.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //prevent cross-site request forgery. Not required but recommended

        public async Task<IActionResult> AssignUserToProject(string userId, int projectId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var project = await _context.Projects.FindAsync(projectId);

            if (user != null && project != null)
            {
                project.UserId = user.Id;
                await _context.SaveChangesAsync();
            }

            TempData["success"] = "User assigned to project successfully";
            return RedirectToAction("ManageUsers");
        }


        // GET
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var UserFromDb = _context.Users.Find(id);
            if (UserFromDb == null)
            {
                return NotFound();
            }

            return View(UserFromDb);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //prevent cross-site request forgery. Not required but recommended

        public IActionResult DeletePOST(string id)
        {
            // check if user exists
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            TempData["success"] = "User deleted successfully";
            return RedirectToAction("ManageUsers");
        }

    }
}
