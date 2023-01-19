using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.ApplicationModels
{
    public class Alertas
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Cabeçalho do Aviso")]
        public string CabecalhoAviso { get; set; }
        [Required]
        [Display(Name = "Texto do Aviso")]
        public string TextoAviso { get; set; }
        [Required]
        [Display(Name = "Data de Alteração")]
        public DateTime DataAlteracao { get; set; }
        [Required]
        [Display(Name = "Data de Início do Aviso")]
        public DateTime DataInicio { get; set; }
        [Required]
        [Display(Name = "Data de Fim do Aviso")]
        public DateTime DataFim { get; set; }
        [Required]
        public bool Ativo { get; set; }
        [Display(Name = "Users")]
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
