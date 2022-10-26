using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssignmentManagementSystem.Models
{
    public class Assignment
    {
        [Key]
        public string AssignmentId { get; set; }

        public string AssignmentName { get; set; }

        public DateTime handoutDate { get; set; }

        public DateTime submissionDate { get; set; }

        [ForeignKey("ModuleRefId")]
        public Module Module { get; set; }

        public string ModuleRefId { get; set; }


    }
}
