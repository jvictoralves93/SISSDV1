using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class Operadora
    {
        [Key]
        public int IDOperadora { get; set; }

        [Required(ErrorMessage = "Nome da Operadora é Obrigatório")]
        [StringLength(30)]
        public string NomeOperadora { get; set; }

        [Required(ErrorMessage = "Contato é obrigatório")]
        [StringLength(20)]
        public string Contato { get; set; }

        //Relação
        public List<Unidade> Unidades { get; set; }

        public List<Link> Links { get; set; }

        public List<LinkTelefonia> LinkTelefonias { get; set; }
    }
}