using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AssignmentManagementSystem.Models
{
    public class Module
    {
        [Key]
        public string ModuleId { get; set; }
        public string ModuleName { get; set;  }

        public ICollection<Assignment> Assignments { get; set;  }
    }


}
