namespace SISSDV1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.ComponentModel;

    public partial class Funcionarios
    {
        [StringLength(120)]
        [DisplayName("Nome")]
        public string NOME { get; set; }

        [StringLength(11)]
        [DisplayName("CPF")]
        public string CPF { get; set; }

        [Key]
        [StringLength(16)]
        [DisplayName("Chapa")]
        public string CHAPA { get; set; }

        
        [DisplayName("Admiss�o")]
        public DateTime ADMISSAO { get; set; }

        [DisplayName("Data de Nascimento")]
        public DateTime DTNASCIMENTO { get; set; }

        [StringLength(100)]
        [DisplayName("Filial")]
        public string FILIAL { get; set; }

        [StringLength(60)]
        [DisplayName("Coligada")]
        public string COLIGADA { get; set; }

        [StringLength(100)]
        [DisplayName("Cargo")]
        public string CARGO { get; set; }

        [StringLength(100)]
        [DisplayName("Departamento")]
        public string DEPARTAMENTO { get; set; }

        [StringLength(60)]
        [DisplayName("Se��o")]
        public string SECAO { get; set; }

        [StringLength(50)]
        [DisplayName("Situa��o")]
        public string SITUACAO { get; set; }

        [StringLength(100)]
        [DisplayName("Descri��o")]
        public string DESCRICAO { get; set; }
    }
}
