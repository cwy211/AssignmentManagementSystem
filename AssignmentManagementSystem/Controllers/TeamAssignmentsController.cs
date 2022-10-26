using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AssignmentManagementSystem.Models;
using AssignmentManagementSystem.Data;
using Amazon.S3; //s3 bucket
using Amazon;
using Amazon.S3.Model; //to get the s3object model class
using System.IO; //files
using Microsoft.Extensions.Configuration; //link to the appsettings.json - getting the key info
using Microsoft.AspNetCore.Http; //upload the file from pc to the network
using Amazon.S3.Transfer;
using System.Net.Mime;
using Microsoft.AspNetCore.Identity;
using AssignmentManagementSystem.Areas.Identity.Data;

namespace AssignmentManagementSystem.Controllers
{

    public class TeamAssignmentsController : Controller
    {
        private readonly AssignmentManagementSystemContext _context;
        const string bucketname = "assignmentmanagementsystem";
        private readonly UserManager<AssignmentManagementSystemUser> _userManager;



        public TeamAssignmentsController(AssignmentManagementSystemContext context, UserManager<AssignmentManagementSystemUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string msg = "")
        {
            ViewBag.msg = msg;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userid = user.Id;
            List<TeamAssignment> teamassignmentlist = await _context.TeamAssignment.Include(m => m.TeammateOne).Include(m => m.TeammateTwo).Include(m => m.TeammateThree).Include(m => m.TeammateFour).ToListAsync();
            List<TeamAssignment> studentTeam = new List<TeamAssignment>();
            foreach (TeamAssignment ta in teamassignmentlist)
            {
                if (ta.Teammate1 == userid || ta.Teammate2 == userid || ta.Teammate3 == userid || ta.Teammate4 == userid)
                {
                    studentTeam.Add(ta);
                }
                
            }
            return View(studentTeam);
        }

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

        public IActionResult UploadAssignment(int? id)
        {
            ViewBag.teamAssignmentId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //avoid cross-site attack
        public async Task<IActionResult> uploadAssignment(IFormFile file,int? id)
        {
            //1.get the keys from the appsettings.json
            List<string> keyLists = getAWSCredentialInfo();

            //2. setup the connection to S3 bucket
            var s3clientobject = new AmazonS3Client(keyLists[0], keyLists[1], keyLists[2], RegionEndpoint.USEast1);

            string filename = "";

            if (file == null)
            {
                return BadRequest("No file selected!");
            }

            var teamAssignment = await _context.TeamAssignment.FindAsync(id);
            

            try
                {
                    //3.2.1 create an upload request for the S3
                    PutObjectRequest uploadrequest = new PutObjectRequest
                    {
                        InputStream = file.OpenReadStream(), //source file
                        BucketName = bucketname + "/assignment/"+id, //bucket path or bucket path with folder
                        Key = file.FileName, //object name in S3
                        CannedACL = S3CannedACL.PublicRead // open to the public to access the upload object
                    };

                    //3.2.2 execute the request command
                    await s3clientobject.PutObjectAsync(uploadrequest);
                teamAssignment.s3Location = "assignment/" + id+"/"+file.FileName;
                teamAssignment.submitStatus = "Yes";
                _context.Update(teamAssignment);
                await _context.SaveChangesAsync();
                filename = filename + " " + file.FileName + " , ";
                }
                catch (AmazonS3Exception ex)
                {
                    return BadRequest("Error in file of " + file.FileName + " : " + ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error in file of " + file.FileName + " : " + ex.Message);
                }
            

            //last sentence: return to index page
            return RedirectToAction("Index", "TeamAssignments", new { msg = "File of " + filename + "submitted successfully" });
        }
    }
}
