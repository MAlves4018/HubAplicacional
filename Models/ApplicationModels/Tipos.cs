using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ApplicationModels
{
    public class Tipos
    {

        [Key]
        [DisplayName("Id do tipo")]
        public int Id { get; set; }


        [Required]
        [DisplayName("*Nome")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Estado do tipo: ")]
        public bool Ativo { get; set; }

        [Required]
        [DisplayName("Ordem")]
        public int? Ordem { get; set; }
    }
}
