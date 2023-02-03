using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApp.Models.ApplicationModels
{
    public class Tipos
    {

        [Key]
        [DisplayName("Type's ID")]
        public int ID { get; set; }


        [Required]
        [DisplayName("Type's name")]
        public string? Name { get; set; }

        [Required]
        [DisplayName("Type's state")]
        public bool? Ativo { get; set; }

        [Required]
        [DisplayName("Type's order")]
        public string? Ordem { get; set; }
    }
}
