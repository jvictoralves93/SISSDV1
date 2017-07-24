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
        [StringLength(50)]
        [DisplayName("Unidade")]
        public string NomeUnidade { get; set; }

        [Required(ErrorMessage = "Telefone é Obrigatório")]
        [StringLength(14)]
        [DisplayName("Telefone")]
        public string TelefoneUnidade { get; set; }
        
        [DisplayName("Site Code")]
        [Range(0, 999, ErrorMessage = "No Maximo 3 Digitos")]
        public int SiteCode { get; set; }        

        [StringLength(30)]
        [DisplayName("Cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Endereço é Obrigatório")]
        [StringLength(110)]
        [DisplayName("Endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Razão Social é Obrigatório")]
        [StringLength(50)]
        [DisplayName("Razão Social")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "CNPJ é Obrigatório")]
        [StringLength(18)]
        [DisplayName("CNPJ")]
        public string CNPJ { get; set; }

        //Chave Estrangeira
        public List<Tecnico> Tecnicos { get; set; }

        public List<Link> Links { get; set; }

        public List<LinkTelefonia> LinkTelefonias { get; set; }

        public List<Firewall> Firewalls { get; set; }

        public List<Servidor> Servidores { get; set; }
    }
}