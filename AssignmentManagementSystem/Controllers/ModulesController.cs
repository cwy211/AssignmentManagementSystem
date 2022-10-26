using Microsoft.AspNetCore.Authorization;
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssignmentManagementSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ModulesController : Controller
    {
        private readonly AssignmentManagementSystemContext _context;


        public ModulesController(AssignmentManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string msg = "")
        {
            ViewBag.msg = msg;
            return View(await _context.Module.ToListAsync());
        }

        public IActionResult AddModule() //load the insert form
        {
            return View();
        }

        [HttpPost] //used to receive user input to database
        [ValidateAntiForgeryToken] ///avoid cross-site attack
        public async Task<IActionResult> AddModule([Bind("ModuleId", "ModuleName")] Module module)
        {
            //form is valid or not
            if (ModelState.IsValid)
            {
                _context.Module.Add(module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { msg = "Module Created Successfully!" });
            }

            return View(module);
        }

        public async Task<IActionResult> AssignLecturer(string id)
        {

            Module module = new Module();
            module = await _context.Module.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }
            
            IEnumerable<AssignmentManagementSystemUser> lecturers = await _context.User.ToListAsync();
            //var x = RoleManager.Roles.Single(x => x.userrole == "Lecturer").Users;
            var lecturerlist = this._context.User.ToList();
            List<AssignmentManagementSystemUser> lecturerlist1 = new List<AssignmentManagementSystemUser>();
            foreach(AssignmentManagementSystemUser aa in lecturerlist)
            {
                if(aa.userrole == "Lecturer")
                {
                    lecturerlist1.Add(aa);
                }
            }
            AssignLecturerModel model = new AssignLecturerModel();
            model.module = module;
            model.lecturers = lecturerlist1;

            return View(model);
        }
    }
}
