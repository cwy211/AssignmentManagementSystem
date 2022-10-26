using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignmentManagementSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssignmentManagementSystem.Data
{
    public class AssignmentManagementSystemContext : IdentityDbContext<AssignmentManagementSystemUser>
    {
        public AssignmentManagementSystemContext(DbContextOptions<AssignmentManagementSystemContext> options)
            : base(options)
        {
        }
        public DbSet<AssignmentManagementSystem.Models.Module> Module { get; set; }
        public DbSet<AssignmentManagementSystemUser> User { get; set; }
        public DbSet<AssignmentManagementSystem.Models.Assignment> Assignment { get; set; }

        public DbSet<AssignmentManagementSystem.Models.TeamAssignment> TeamAssignment { get; set; }

        public DbSet<AssignmentManagementSystem.Models.Task> Task { get; set; }

        public DbSet<AssignmentManagementSystem.Models.LecturerModule> LecturerModule { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
