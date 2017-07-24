using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class Servidor
    {
        [Key]
        public int IDServidor { get; set; }

        [Required(ErrorMessage = "Digite o nome do Servidor")]
        [StringLength(20)]
        [DisplayName("Nome do Servidor")]
        public string Hostname { get; set; }

        [Required(ErrorMessage = "Digite o IP do Servidor")]
        [DisplayName("IP")]
        [StringLength(15)]
        public string IP { get; set; }

        [Required(ErrorMessage = "Digite a descrição do Servidor")]
        [StringLength(30)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }
        
        [Required(ErrorMessage = "Escolha a Unidade")]
        [DisplayName("Unidade")]
        public int IDUnidade { get; set; }
        //Lazy Loading
        public virtual Unidade Unidade { get; set; }
    }
}