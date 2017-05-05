using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class Firewall
    {
        [Key]
        public int IDFirewall { get; set; }

        [StringLength(30)]
        [DisplayName("Modelo")]
        public string Modelo { get; set; }

        [DisplayName("Licença")]
        public DateTime Licenca { get; set; }

        [StringLength(30)]
        [DisplayName("Acesso Interno")]
        public string AcessoInterno { get; set; }

        [StringLength(30)]
        [DisplayName("Acesso Externo")]
        public string AcessoExterno { get; set; }

        [Required(ErrorMessage = "Escolha a Unidade")]
        [DisplayName("Unidade")]
        public int IDUnidade { get; set; }
        //Lazy Loading
        public virtual Unidade Unidade { get; set; }
    }
}