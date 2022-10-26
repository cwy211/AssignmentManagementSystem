using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentManagementSystem.Models
{
    public class AssignLecturerModel
    {
        public Module module { get; set; }
        public IEnumerable<AssignmentManagementSystem.Areas.Identity.Data.AssignmentManagementSystemUser> lecturers { get; set; }
    }
}
