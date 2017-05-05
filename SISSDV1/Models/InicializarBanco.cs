using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SISSDV1.Models
{
    public class InicializarBanco : CreateDatabaseIfNotExists<BancoContexto>
    {
        protected override void Seed(BancoContexto context)
        {      
            base.Seed(context);
        }
    }
}