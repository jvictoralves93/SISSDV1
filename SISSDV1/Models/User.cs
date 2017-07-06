using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class User
    {
        [Required(ErrorMessage = "Digite o Nome")]
        [DisplayName("Nome")]
        [StringLength(maximumLength: 28, ErrorMessage = "O Nome deve ter no máximo 28 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o Sobrenome")]
        [DisplayName("Sobrenome")]
        [StringLength(maximumLength: 28, ErrorMessage = "O Sobrenome deve ter no máximo 28 caracteres")]
        public string Sobrenome { get; set; }
        
        [Required(ErrorMessage = "Digite o Usuário")]
        [DisplayName("Usuário")]
        [StringLength(maximumLength: 20, ErrorMessage = "O Usuário deve ter no máximo 20 caracteres")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Digite a Senha")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 16, MinimumLength = 6, 
            ErrorMessage = "A senha deve ter entre 6 e 16 caracteres")]
        [RegularExpression(@"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d\!@#\$%&\*]{7,}$",
            ErrorMessage = "A senha não corresponde aos requisitos de diretiva de senha")] //String da Política de Senha
        [DisplayName("Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirme a Senha")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "Senhas não conferem")]
        [DisplayName("Confirmar Senha")]
        public string ConfirmarSenha { get; set; }

        [StringLength(maximumLength: 20, ErrorMessage = "O Telefone deve ter no máximo 20 caracteres")]
        [Required(ErrorMessage = "Digite o Telefone")]
        [DisplayName("Ramal")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Digite o Endereço")]
        [DisplayName("Endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Digite a Chapa")]
        [DisplayName("Chapa")]
        public string Chapa { get; set; }

        [Required(ErrorMessage = "Digite o CPF")]
        [DisplayName("CPF")]
        public string CPF { get; set; }

        [StringLength(maximumLength: 64, ErrorMessage = "O Cargo deve ter no máximo 64 caracteres")]
        [Required(ErrorMessage = "Digite o Cargo")]
        public string Cargo { get; set; }

        [Required(ErrorMessage = "Digite o Gerente")]
        public string Gerente { get; set; }

        //Combobox 
        [Required(ErrorMessage = "Selecione uma Cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Selecione uma Unidade")]
        public string Unidade { get; set; }

        [Required(ErrorMessage = "Selecione um Departamento")]
        public string Departamento { get; set; }

        [Required(ErrorMessage = "Digite O Departamento")]
        [DisplayName("Subdepartamento")]
        public string SubDepartamento { get; set; }

        //Horario de Logon
        public string HorarioInicio { get; set; }
        public string HorarioFim { get; set; }
        public string DiaInicio { get; set; }
        public string DiaFim { get; set; }

        //Partes Preenchidas Automáticamente        
        public string NomeExibicao { get; set; }
        public string Iniciais { get; set; }
        public string Descricao { get; set; }
        public string Escritorio { get; set; }
        public string Email { get; set; }
        public string PaginaWeb { get; set; }
        public string CaixaPostal { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string Pais { get; set; }
        public bool Status { get; set; }
        public string StatusTexto { get; set; }
        public bool Admin { get; set; }
        public string objectSid { get; set; }

        //Grupos
        public string GrupoNome { get; set; }
        
        //Relacionamento
        public virtual OrganizationalUnit OU { get; set; }

        //Relacionamento
        public virtual GrupoAD GrupoAD { get; set; }

    }
}