using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BasicAuth.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BasicAuth.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _UserManager;

        public ItemController (UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _UserManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
			var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var currentUser = await _UserManager.FindByIdAsync(userId);
            return View(_db.Items.Where(x => x.User.Id == currentUser.Id));
        }
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _UserManager.FindByIdAsync(userId);
            item.User = currentUser;
            _db.Items.Add(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
