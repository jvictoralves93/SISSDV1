using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class Link
    {
        [Key]
        public int IDLink { get; set; }        

        [Required(ErrorMessage = "Designação não pode estar em branco")]
        [StringLength(30)]
        [DisplayName("Designação")]
        public string Designacao { get; set; }

        [Required(ErrorMessage = "Código do Cliente não pode estar em branco")]
        [StringLength(30)]
        [DisplayName("Código do Cliente")]
        public string CodigoCliente { get; set; }

        [Required(ErrorMessage = "Capacidade não pode estar em branco")]
        [DisplayName("Capacidade")]
        public int Capacidade { get; set; }

        [Required(ErrorMessage = "Tipo não pode estar em branco")]
        [StringLength(10)]
        [DisplayName("Tipo")]
        public string Tipo { get; set; }

        //Relação
        [DisplayName("Operadora")]
        [Required(ErrorMessage = "Escolha a Operadora")]
        public int IDOperadora { get; set; }
        //Lazy Loading
        public virtual Operadora Operadora { get; set; }
        public string Contato { get; set; }

        //Relação
        [DisplayName("Unidade")]
        [Required(ErrorMessage = "Escolha a Unidade")]
        public int IDUnidade { get; set; }
        //Lazy Loading
        public virtual Unidade Unidade { get; set; }

      

    }
}