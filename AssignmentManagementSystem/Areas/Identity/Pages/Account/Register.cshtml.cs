using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using AssignmentManagementSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Amazon.SimpleNotificationService;
using Amazon;
using Microsoft.Extensions.Configuration;
using System.IO;
using Amazon.SimpleNotificationService.Model;

namespace AssignmentManagementSystem.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AssignmentManagementSystemUser> _signInManager;
        private readonly UserManager<AssignmentManagementSystemUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string topicArn = "arn:aws:sns:us-east-1:412960202609:testSNS";


        public SelectList RoleSelectList = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Selected = true, Text = "Select Role", Value = ""},
            new SelectListItem { Selected = false, Text = "Admin", Value = "Admin"},
            new SelectListItem { Selected = false, Text = "Lecturer", Value = "Lecturer"},
            new SelectListItem { Selected = false, Text = "Student", Value = "Student"}
        }, "Value", "Text", 1);

        public RegisterModel(
            UserManager<AssignmentManagementSystemUser> userManager,
            SignInManager<AssignmentManagementSystemUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
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

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string firstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string lastName { get; set; }

            [Required]
            [Display(Name = "User Role")]
            public string userrole { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                bool roleExistResult = await _roleManager.RoleExistsAsync("Admin");
                if(! roleExistResult)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                roleExistResult = await _roleManager.RoleExistsAsync("Lecturer");
                if (!roleExistResult)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Lecturer"));
                }

                roleExistResult = await _roleManager.RoleExistsAsync("Student");
                if (!roleExistResult)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Student"));
                }

                var user = new AssignmentManagementSystemUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    firstName = Input.firstName,
                    lastName = Input.lastName,
                    userrole = Input.userrole,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //subscribe student to sns
                    if (Input.userrole == "Student")
                    {

                        List<string> keyLists = getAWSCredentialInfo();

                        //2. setup the connection to S3 bucket
                        var snsClient = new AmazonSimpleNotificationServiceClient(keyLists[0], keyLists[1], keyLists[2], RegionEndpoint.USEast1);

                        SubscribeRequest emailRequest = new SubscribeRequest(topicArn, "email", Input.Email);

                        SubscribeResponse emailSubscribeResponse = await snsClient.SubscribeAsync(emailRequest);
                    }
                    

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _userManager.AddToRoleAsync(user, Input.userrole);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
