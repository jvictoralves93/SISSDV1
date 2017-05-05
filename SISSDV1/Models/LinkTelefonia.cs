using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class LinkTelefonia
    {
        [Key]
        public int IDLinkTelefonia { get; set; }        

        [Required(ErrorMessage = "Código do Cliente não pode estar em branco")]
        [StringLength(30)]
        [DisplayName("Código do Cliente")]
        public string CodigoCliente { get; set; }

        [Required(ErrorMessage = "DDD não pode estar em branco")]
        [DisplayName("DDD")]
        public int DDD { get; set; }

        [Required(ErrorMessage = "Tronco Chave não pode estar em branco")]
        [StringLength(30)]
        [DisplayName("Tronco Chave")]
        public string TroncoChave { get; set; }

        [Required(ErrorMessage = "Numeros Portados Inicio não pode estar em branco")]
        [StringLength(30)]
        [DisplayName("Numeros Portados Inicio")]
        public string NumerosPortadosInicio { get; set; }

        [Required(ErrorMessage = "Numeros Portados Fim não pode estar em branco")]
        [StringLength(30)]
        [DisplayName("Numeros Portados Fim")]
        public string NumerosPortadosFim { get; set; }

        //Relação
        [DisplayName("Operadora")]
        [Required(ErrorMessage = "Escolha a Operadora")]
        public int IDOperadora { get; set; }        
        //Lazy Loading
        public virtual Operadora Operadora { get; set; }

        //Relação
        [DisplayName("Unidade")]
        [Required(ErrorMessage = "Escolha a Unidade")]
        public int IDUnidade { get; set; }
        //Lazy Loading
        public virtual Unidade Unidade { get; set; }
        
    }
}