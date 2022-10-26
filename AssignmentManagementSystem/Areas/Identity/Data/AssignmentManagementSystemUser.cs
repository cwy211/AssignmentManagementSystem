using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AssignmentManagementSystem.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the AssignmentManagementSystemUser class
    public class AssignmentManagementSystemUser : IdentityUser
    {
        [PersonalData]
        public string firstName { get; set; }

        [PersonalData]
        public string lastName { get; set; }

        [PersonalData]
        public string userrole { get; set; }
    }
}
