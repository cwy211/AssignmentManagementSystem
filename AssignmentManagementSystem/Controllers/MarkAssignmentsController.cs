using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AssignmentManagementSystem.Models;
using AssignmentManagementSystem.Data;
using Microsoft.AspNetCore.Identity;
using AssignmentManagementSystem.Areas.Identity.Data;
using Amazon.S3; //s3 bucket
using Amazon;
using Amazon.S3.Model; //to get the s3object model class
using System.IO; //files
using Microsoft.Extensions.Configuration; //link to the appsettings.json - getting the key info
using Microsoft.AspNetCore.Http; //upload the file from pc to the network
using Amazon.S3.Transfer;
using System.Net.Mime;


namespace AssignmentManagementSystem.Controllers
{
    public class MarkAssignmentsController : Controller
    {
        private readonly AssignmentManagementSystemContext _context;
        UserManager<AssignmentManagementSystemUser> UserManager;
        SignInManager<AssignmentManagementSystemUser> SignInManager;
        const string bucketname = "assignmentmanagementsystem";


        private List<string> getAWSCredentialInfo()
        {
            //1.setup the appsettings.json file path in these sentence
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configure = builder.Build(); //before link to the appsettings.json, build it for debugging

            //2. read the key info from the json using configure instance
            List<string> keyLists = new List<string>();
            keyLists.Add(configure["AWScredential:key1"]); //accesskey
            keyLists.Add(configure["AWScredential:key2"]); //sessionkey
            keyLists.Add(configure["AWScredential:key3"]); //tokenkey

            //return to the function who needs the keys
            return keyLists;
        }

        public MarkAssignmentsController(AssignmentManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string msg = "")
        {
            ViewBag.msg = msg;
            List<TeamAssignment> teamassignmentlist = await _context.TeamAssignment.ToListAsync();
            List<LecturerModule> lecturermodulelist = await _context.LecturerModule.ToListAsync();
            List<Assignment> assignmentlist = await _context.Assignment.ToListAsync();
            string username = User.Identity.Name;
            var userlist = this._context.User.ToList();
            foreach(AssignmentManagementSystemUser ee in userlist)
            {
                if(ee.UserName == username)
                {
                    string useridnow = ee.Id;

                    List<string> moduleofuser = new List<string>();
                    List<string> assignmentofuser = new List<string>();
                    List<TeamAssignment> finallist = new List<TeamAssignment>();
                    foreach (LecturerModule lml in lecturermodulelist)
                    {
                        if (lml.UserId == useridnow)
                        {
                            moduleofuser.Add(lml.ModuleId);
                        }
                    }
                    foreach (Assignment a in assignmentlist)
                    {
                        foreach (string b in moduleofuser)
                        {
                            if (a.ModuleRefId == b)
                            {
                                assignmentofuser.Add(a.AssignmentId);
                            }
                        }
                    }
                    foreach (TeamAssignment tal in teamassignmentlist)
                    {
                        foreach (string c in assignmentofuser)
                        {
                            if (tal.AssignmentId == c)
                            {
                                finallist.Add(tal);
                            }
                        }
                    }
                    return View(finallist);
                }
            }
            return null;
        }

        public async Task<IActionResult> Mark(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taid = await _context.TeamAssignment.Include(d=>d.TeammateOne).Include(d=>d.TeammateTwo).Include(d=>d.TeammateThree).Include(d=>d.TeammateFour).FirstOrDefaultAsync(d=>d.TeamAssignmentId==id);
            if (taid == null)
            {
                return NotFound();
            }
            return View(taid);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int TeamAssignmentId, [Bind("TeamAssignmentId,AssignmentId,s3Location,submitStatus,mark,Teammate1,Teammate2,Teammate3,Teammate4")] TeamAssignment teamAssignment)
        {
            if (TeamAssignmentId != teamAssignment.TeamAssignmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                try
                {
                    var taid = await _context.TeamAssignment.FindAsync(TeamAssignmentId);
                    taid.mark = teamAssignment.mark;
                    _context.Update(taid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamAssignmentExists(teamAssignment.TeamAssignmentId))
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
            return View(teamAssignment);
        }

        private bool TeamAssignmentExists(int teamAssignmentId)
        {
            return _context.TeamAssignment.Any(e => e.TeamAssignmentId == teamAssignmentId);
        }

        public async Task<Stream> ReadObjectData(string fileName)
        {

            //1.get the keys from the appsettings.json
            List<string> keyLists = getAWSCredentialInfo();

            //2. setup the connection to S3 bucket
            var s3clientobject = new AmazonS3Client(keyLists[0], keyLists[1], keyLists[2], RegionEndpoint.USEast1);

            //3. start deleting the image
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketname,
                    Key = fileName
                };

                using (var getObjectResponse = await s3clientobject.GetObjectAsync(request))
                {
                    using (var responseStream = getObjectResponse.ResponseStream)
                    {

                        var stream = new MemoryStream();
                        await responseStream.CopyToAsync(stream);
                        stream.Position = 0;
                        return stream;

                    }
                }

            }

            catch (Exception exception)
            {
                throw new Exception("Read object operation failed.", exception);
            }
        }

        [HttpGet]
        public async Task<IActionResult> downloadAssignment(string filename)
        {
            Stream stream = await ReadObjectData(filename);
            string file = Path.GetFileName(filename);
            Response.Headers.Add("Content-Disposition", new ContentDisposition
            {
                FileName = file,
                Inline = false 
            }.ToString());

            return File(stream, "application/pdf");
        }


    }
}