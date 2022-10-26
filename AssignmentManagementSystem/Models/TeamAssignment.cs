using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssignmentManagementSystem.Areas.Identity.Data;

namespace AssignmentManagementSystem.Models
{
    public class TeamAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamAssignmentId { get; set; }

        public string AssignmentId { get; set; }
        
        public string s3Location { get; set; }

        public string submitStatus { get; set; }

        public int mark { get; set; }

        [ForeignKey("Teammate1")]
        public AssignmentManagementSystemUser TeammateOne { get; set; }
        public string Teammate1 { get; set; }

        [ForeignKey("Teammate2")]
        public AssignmentManagementSystemUser TeammateTwo { get; set; }
        public string Teammate2 { get; set; }

        [ForeignKey("Teammate3")]
        public AssignmentManagementSystemUser TeammateThree { get; set; }
        public string Teammate3 { get; set; }


        [ForeignKey("Teammate4")]
        public AssignmentManagementSystemUser TeammateFour { get; set; }
        public string Teammate4 { get; set; }

    }
}
