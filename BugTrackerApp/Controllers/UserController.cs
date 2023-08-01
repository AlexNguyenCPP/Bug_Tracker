using BugTrackerApp.Data;
using BugTrackerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        // GET
        public IActionResult ManageUsers()
        {
            var viewModel = new ManageUsersViewModel
            {
                Users = _userManager.Users.ToList(),
                Projects = _context.Projects.ToList()
            };

            return View(viewModel);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> AssignUserToProject(string userId, int projectId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var project = await _context.Projects.FindAsync(projectId);

            if (user != null && project != null)
            {
                project.UserId = user.Id;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageUsers");
        }

    }
}
