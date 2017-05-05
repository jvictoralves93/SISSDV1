using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class EscalaSabado
    {
        [Key]
        public int id { get; set; }

        [StringLength(40)]
        public string title { get; set; }

        [StringLength(40)]
        public string description { get; set; }
        
        public DateTime start { get; set; }
        
        public DateTime end { get; set; }
        
        public bool allday { get; set; }
        
    }
}