using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SISSDV1.Models
{
    public class GrupoAD
    {
        public string GrupoNome { get; set; }
        public List<User> Membros { get; set; }
    }
}