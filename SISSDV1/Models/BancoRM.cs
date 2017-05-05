using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SISSDV1.Models
{
    public class BancoRM : DbContext
    {
        public BancoRM() : base("Funcionarios")
        {

        }

        public System.Data.Entity.DbSet<SISSDV1.Models.Funcionarios> Funcionarios { get; set; }
    }
}