using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssignmentManagementSystem.Areas.Identity.Data;

namespace AssignmentManagementSystem.Models
{
    public class LecturerModule
    {
        //LMid (auto-generate), userid (FK), moduleid (FK)
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string LecturerModuleId { get; set; }
        
        [ForeignKey("UserId")]
        public AssignmentManagementSystemUser Users { get; set; }
        public string UserId { get; set; }

        [ForeignKey("ModuleId")]
        public Module Module { get; set; }
        public string ModuleId { get; set; }

    }
}
