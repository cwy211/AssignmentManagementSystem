using AssignmentManagementSystem.Areas.Identity.Data;
using AssignmentManagementSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignmentManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace AssignmentManagementSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class EnrollTeamsController : Controller
    {
        private readonly AssignmentManagementSystemContext _context;
        private readonly UserManager<AssignmentManagementSystemUser> _userManager;


        public EnrollTeamsController(AssignmentManagementSystemContext context, UserManager<AssignmentManagementSystemUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string msg = "")
        {
            ViewBag.msg = msg;
            return View(await _context.Assignment.Include(d => d.Module).ToListAsync());
        }

        public async Task<IActionResult> ViewTeams(string id)
        {
            return View(await _context.TeamAssignment.Include(m => m.TeammateOne).Include(m => m.TeammateTwo).Include(m => m.TeammateThree).Include(m => m.TeammateFour).Where(a=>a.AssignmentId==id).ToListAsync());
        }

        public async Task<IActionResult> JoinTeam(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userid = user.Id;
            var teamAssignment = await _context.TeamAssignment.FindAsync(id);
            if(teamAssignment.Teammate1== userid|| teamAssignment.Teammate2 == userid|| teamAssignment.Teammate3 == userid|| teamAssignment.Teammate4 == userid)
            {
                return RedirectToAction("Index", "EnrollTeams", new { msg = "Already enrolled in team!" });
            }
            if (teamAssignment.Teammate1 == null)
            {
                teamAssignment.Teammate1 = userid;
                _context.Update(teamAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "EnrollTeams", new { msg = "Successfully enrolled to team!" });
            }
            else if(teamAssignment.Teammate2 == null)
            {
                teamAssignment.Teammate2 = userid;
                _context.Update(teamAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "EnrollTeams", new { msg = "Successfully enrolled to team!" });
            }
            else if (teamAssignment.Teammate3 == null)
            {
                teamAssignment.Teammate3 = userid;
                _context.Update(teamAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "EnrollTeams", new { msg = "Successfully enrolled to team!" });
            }
            else if (teamAssignment.Teammate4 == null)
            {
                teamAssignment.Teammate4 = userid;
                _context.Update(teamAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "EnrollTeams", new { msg = "Successfully enrolled to team!" });
            }
            return RedirectToAction("Index", "EnrollTeams", new { msg = "Team is full!" });
        }


    }
}
