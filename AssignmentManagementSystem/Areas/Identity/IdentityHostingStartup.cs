using System;
using AssignmentManagementSystem.Areas.Identity.Data;
using AssignmentManagementSystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(AssignmentManagementSystem.Areas.Identity.IdentityHostingStartup))]
namespace AssignmentManagementSystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AssignmentManagementSystemContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AssignmentManagementSystemContextConnection")));

                services.AddDefaultIdentity<AssignmentManagementSystemUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AssignmentManagementSystemContext>();
            });
        }
    }
}