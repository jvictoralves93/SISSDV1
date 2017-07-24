using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SISSDV1.Models
{
    public class BancoContexto : DbContext
    {
    
        public BancoContexto() : base("SisSD")
        {
        }
        
        public DbSet<Unidade> Unidades { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<Tecnico> Tecnicoes { get; set; }

        public DbSet<LinkTelefonia> LinkTelefonias { get; set; }

        public DbSet<Operadora> Operadoras { get; set; }

        public DbSet<Firewall> Firewalls { get; set; }

        public DbSet<EscalaSabado> Escalas { get; set; }

        public DbSet<Servidor> Servidors { get; set; }
    }
}
