using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class Unidade
    {
        [Key]
        public int IDUnidade { get; set; }

        [Required(ErrorMessage = "Digite o nome da Unidade")]
        [MinLength(2, ErrorMessage = "O tamanho mínimo do nome são 2 caracteres")]
        [StringLength(80)]
        [DisplayName("Unidade")]
        public string NomeUnidade { get; set; }

        [Required(ErrorMessage = "Telefone é Obrigatório")]
        [StringLength(30)]
        [DisplayName("Telefone")]
        public string TelefoneUnidade { get; set; }
        
        [DisplayName("Site Code")]
        public int SiteCode { get; set; }        

        [StringLength(30)]
        [DisplayName("Cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Endereço é Obrigatório")]
        [StringLength(80)]
        [DisplayName("Endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Razão Social é Obrigatório")]
        [StringLength(80)]
        [DisplayName("Razão Social")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "CNPJ é Obrigatório")]
        [StringLength(30)]
        [DisplayName("CNPJ")]
        public string CNPJ { get; set; }

        //Chave Estrangeira
        public List<Tecnico> Tecnicos { get; set; }

        public List<Link> Links { get; set; }

        public List<LinkTelefonia> LinkTelefonias { get; set; }

        public List<Firewall> Firewalls { get; set; }
    }
}