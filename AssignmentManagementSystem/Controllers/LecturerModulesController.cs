using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AssignmentManagementSystem.Models;
using AssignmentManagementSystem.Data;

namespace AssignmentManagementSystem.Controllers
{
    public class LecturerModulesController : Controller
    {
        private readonly AssignmentManagementSystemContext _context;

        public LecturerModulesController(AssignmentManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //List<LecturerModule> lm = await _context.LecturerModule.Include(d => d.Module).Include(d => d.Users).ToListAsync();
            //return View(lm);
            return View(await _context.LecturerModule.Include(d => d.Module).Include(d=>d.Users).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignLecturer([Bind("UserId,ModuleId")] LecturerModule lecturerModule)
        {
            if (ModelState.IsValid)
            {
                _context.LecturerModule.Add(lecturerModule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "LecturerModules", new { msg = "Lecturer assigned to module successfully!" });
            }
            return View();
        }
    }
}
