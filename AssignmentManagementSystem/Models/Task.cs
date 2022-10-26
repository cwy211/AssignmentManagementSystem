using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssignmentManagementSystem.Areas.Identity.Data;

namespace AssignmentManagementSystem.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        [ForeignKey("TeamAssignmentId")]
        public TeamAssignment TeamAssignment { get; set; }
        public int TeamAssignmentId { get; set; }

        [ForeignKey("AssignmentId")]
        public Assignment Assignment { get; set; }

        public string AssignmentId { get; set; }

        [ForeignKey("UserId")]
        public AssignmentManagementSystemUser Users { get; set; }
        public string UserId { get; set; }

        public DateTime SubmissionDate { get; set; }
        public string SubmitStatus { get; set; }

    }
}
