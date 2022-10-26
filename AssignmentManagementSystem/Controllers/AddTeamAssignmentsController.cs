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
    public class AddTeamAssignmentsController : Controller
    {
        private readonly AssignmentManagementSystemContext _context;

        public AddTeamAssignmentsController(AssignmentManagementSystemContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddTeamAssignment(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taid = await _context.Assignment.FindAsync(id);
            if (taid == null)
            {
                return NotFound();
            }
            return View(taid);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTeamAssignment([Bind("AssignmentId")] TeamAssignment teamAssignment)
        {
            if (ModelState.IsValid)
            {
                teamAssignment.submitStatus = "No";
                _context.TeamAssignment.Add(teamAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "MarkAssignments", new { msg = "Team added to assignment successfully!" });
            }
            return View();
        }
    }
}
