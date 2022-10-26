using AssignmentManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace AssignmentManagementSystem.Controllers
{
    public class EmailsController : Controller
    {

        private readonly IHttpClientFactory _clientFactory;

        public EmailsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult Index(string msg = "")
        {
            ViewBag.msg = msg;
            return View();
        }

        [HttpPost] //used to receive user input to database
        [ValidateAntiForgeryToken] ///avoid cross-site attack
        public async Task<IActionResult> SendEmail([Bind("message")] Email email)
        {
            //form is valid or not
            if (ModelState.IsValid)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://qnv56fg77d.execute-api.us-east-1.amazonaws.com/production/notification");

 

                request.Content = new StringContent(JsonSerializer.Serialize(email));
                request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index), new { msg = "Email Sent Successfully!" + email.message });

                }
                else
                {
                    return RedirectToAction(nameof(Index), new { msg = "Email Sending Failed!" });
                }
            }

            return View();
        }

        
    }
}
