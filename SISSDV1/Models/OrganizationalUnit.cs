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
    public class OrganizationalUnit
    {
        public string Name { get; set; }
        public string Ou { get; set; }
        public string Description { get; set; }
        public string Cidade { get; set; }
        public string Unidade { get; set; }
        public string Departamento { get; set; }

        public List<User> Users { get; set; }
        public List <Computador> Computadores { get; set; }
    }
}