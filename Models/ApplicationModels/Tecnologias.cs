using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.ApplicationModels
{
    public class Tecnologias
    {

        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }


        [Required]
        [DisplayName("*Nome")]
        public string Name { get; set; }

        [Required]
        [DisplayName("*Sigla")]
        public string Sigla { get; set; }


        [DisplayName("Link")]
        public string? Link { get; set; }


        [DisplayName("Link da documentação")]
        public string? Linkdocs { get; set; }


        [DisplayName("Link dos logs")]
        public string? Linklogs { get; set; }



        [DisplayName("Link dos reports")]
        public string? Linkreports { get; set; }




        [DisplayName("Descriçao")]
        public string? Descricao { get; set; }


        [DisplayName("Email do criador")]
        public string? Maildev { get; set; }

        public int TypeId { get; set; }

        [DisplayName("*Nome do tipo")]
        [ForeignKey("TypeId")]
        public Tipos? Tipo { get; set; }

        [DisplayName("Nome da imagem")]
        public string? ImageName { get; set; }

        [NotMapped]
        [DisplayName("*Nome do ficheiro")]
        public IFormFile? ImageFile { get; set; }

        [DisplayName("Estado do tipo: ")]
        public bool Apagado { get; set; }

    }
}
