using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public static class Context
    {
        public static User UserLoged { get; set; }
        public static string ServerIp { get; set; }
    }
}