using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AssignmentManagementSystem.Models;
using AssignmentManagementSystem.Data;
using AssignmentManagementSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace AssignmentManagementSystem.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly AssignmentManagementSystemContext _context;
        private readonly UserManager<AssignmentManagementSystemUser> _userManager;

        public AssignmentsController(AssignmentManagementSystemContext context, UserManager<AssignmentManagementSystemUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string msg = "")
        {
            ViewBag.msg = msg;
            List<Assignment> a = await _context.Assignment.Include(d => d.Module).ToListAsync();
          return View(await _context.Assignment.Include(d => d.Module).ToListAsync());
        }

        public async Task<IActionResult> AddAssignment()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userid = user.Id;
            List<LecturerModule> lecturermodulelist = await _context.LecturerModule.Include(d=>d.Module).ToListAsync();
            List<Module> modulelecturer = new List<Module>();
            foreach(LecturerModule lm in lecturermodulelist)
            {
                if(lm.UserId == userid)
                {
                    modulelecturer.Add(lm.Module);
                }
            }
            return View(modulelecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAssignment([Bind("AssignmentId, AssignmentName, handoutDate, submissionDate, Module,ModuleRefId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Assignment.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { msg = "Assignment Created Successfully!" });
            }
            return View(assignment);
        }
    }
}
