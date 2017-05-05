using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class Computador
    {
        [Required(ErrorMessage = "Digite o Nome do Computador")]
        [DisplayName("Computador")]
        [StringLength(maximumLength: 28, ErrorMessage = "O Nome deve ter no máximo 28 caracteres")]
        public string Hostname { get; set; }
        [Required(ErrorMessage = "Digite o Nome do Computador")]
        [DisplayName("Nome do Computador")]
        [StringLength(maximumLength: 28, ErrorMessage = "O Nome deve ter no máximo 28 caracteres")]
        public string NomeComputador { get; set; }
        public string SistemaOperacional { get; set; }
        public string Cidade { get; set; }
        public string Unidade { get; set; }
        public string Departamento { get; set; }
        public string GrupoNome { get; set; }
        [Required(ErrorMessage = "Digite o Nome do Técnico")]
        [DisplayName("Técnico")]
        [StringLength(maximumLength: 28, ErrorMessage = "O Nome deve ter no máximo 28 caracteres")]
        public string Tecnico { get; set; }

        //Relacionamento
        public virtual OrganizationalUnit OU { get; set; }

        //Relacionamento
        public virtual GrupoAD GrupoAD { get; set; }

        public virtual User User { get; set; }
    }
}