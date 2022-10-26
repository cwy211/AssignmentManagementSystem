using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssignmentManagementSystem.Models;
using AssignmentManagementSystem.Data;
using Task = AssignmentManagementSystem.Models.Task;
using AssignmentManagementSystem.Areas.Identity.Data;

namespace AssignmentManagementSystem.Controllers
{
    public class TasksController : Controller
    {
        private readonly AssignmentManagementSystemContext _context;

        public TasksController(AssignmentManagementSystemContext context)
        {
            _context = context;
        }

        //View Task
        public async Task<IActionResult> Index(string msg = "")
        {
            ViewBag.msg = msg;

            List<TeamAssignment> teamassignmentlist = await _context.TeamAssignment.ToListAsync();
            string username = User.Identity.Name;
            var userlist = this._context.User.ToList();
            foreach (AssignmentManagementSystemUser user in userlist)
            {
                if (user.UserName == username)
                {
                    string useridnow = user.Id;

                    List<TeamAssignment> subTAlist = new List<TeamAssignment>();

                    foreach (TeamAssignment ta in teamassignmentlist)
                    {
                        if (user.userrole == "Student")
                        {
                            if (ta.Teammate1 == useridnow || ta.Teammate2 == useridnow || ta.Teammate3 == useridnow || ta.Teammate4 == useridnow)
                            {
                                subTAlist.Add(ta);
                            }
                        }
                    }
                    return View(subTAlist);
                }
            }
            return null;
            //return View(await _context.Task.Include(d => d.Assignment).Include(d => d.Users).Include(d=>d.TeamAssignment).ToListAsync());
        }

        //Add Tasks
        public async Task<IActionResult> AddTask(int? taid, string aid)
        {
            ViewBag.taid = taid;
            ViewBag.aid = aid;
            List<Task> taskList = await _context.Task.Include(d => d.Assignment).Include(d => d.Users).Include(d => d.TeamAssignment).ToListAsync();
            List<Task> subTaskList = new List<Task>();
            foreach (Task task in taskList)
            {
                if(task.TeamAssignmentId == taid)
                {
                    subTaskList.Add(task);
                }
            }
            return View(subTaskList);
        }

        ////////////////Create Task
        public async Task<IActionResult> CreateTask(int? taid, string aid)
        {
            ViewBag.taid = taid;
            ViewBag.aid = aid;

            var studentlist = this._context.User.ToList();
            List<TeamAssignment> talist = this._context.TeamAssignment.ToList();
            List<AssignmentManagementSystemUser> studentteamlist = new List<AssignmentManagementSystemUser>();
            List<AssignmentManagementSystemUser> tauser = new List<AssignmentManagementSystemUser>();
            foreach (AssignmentManagementSystemUser aa in studentlist)
            {
                if (aa.userrole == "Student")
                {
                    studentteamlist.Add(aa);
                }
            }
            foreach(TeamAssignment bb in talist )
            {
                if(bb.TeamAssignmentId == taid)
                {
                    tauser.Add(bb.TeammateOne);
                    tauser.Add(bb.TeammateTwo);
                    tauser.Add(bb.TeammateThree);
                    tauser.Add(bb.TeammateFour);
                    return View(tauser);
                }
            }
            return View(tauser);
        }
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([Bind("TeamAssignmentId, AssignmentId, UserId, SubmissionDate, SubmitStatus")] Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Task.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Tasks", new { msg = "Task Created Successfully!" });
            }
            return View(task);
        }

        /////////////////Edit Task
        public async Task<IActionResult> Edit(int? id, string name)
        {
            ViewBag.name = name;
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        private bool TaskExists(int taskId)
        {
            return _context.Task.Any(e => e.TaskId == taskId);
        }

        // POST: Edit Task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId, TaskName, TeamAssignmentId, AssignmentId, UserId, SubmissionDate, SubmitStatus")] Task task)
        {
            if (id != task.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        /////////////////Task Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.Include(d=>d.Users)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        /////////////////GET: Delete Task
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.Include(d => d.Users)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        ///////////////POST: Delete Task
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Task.FindAsync(id);
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
