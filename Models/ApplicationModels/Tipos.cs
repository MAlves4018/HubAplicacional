using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
