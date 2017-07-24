using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class Tecnico
    {
        [Key]
        public int IDTecnico { get; set; }

        [Required(ErrorMessage = "Digite o Nome de Tecnico")]
        [StringLength(50)]
        [DisplayName("Tecnico")]
        public string NomeTecnico { get; set; }

        [StringLength(15)]
        [DisplayName("Celular")]
        public string Celular { get; set; }

        [StringLength(15)]
        [DisplayName("Ramal")]
        public string Ramal { get; set; }

        [Required(ErrorMessage = "Escolha a Unidade")]
        [DisplayName("Unidade")]
        public int IDUnidade { get; set; }
        //Lazy Loading
        public virtual Unidade Unidade { get; set; }
    }
}