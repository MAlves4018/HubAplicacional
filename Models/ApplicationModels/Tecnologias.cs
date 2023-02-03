using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.ApplicationModels
{
    public class Tecnologias
    {
        [Key]
        [DisplayName("Technology's ID")]
        public int ID { get; set; }


        [Required]
        [DisplayName("Technology's name")]
        public string? Name { get; set; }

        [Required]
        [DisplayName("Technology's acronym")]
        public string? Sigla { get; set; }

        [Required]
        [DisplayName("Link")]
        public string? Link { get; set; }

        [Required]
        [DisplayName("Logs Link")]
        public string? Linklogs { get; set; }

        [Required]
        [DisplayName("Docs Link")]
        public string? Linkdocs { get; set; }

        [Required]
        [DisplayName("Reports Link")]
        public string? Linkreports { get; set; }

        [Required]
        [DisplayName("AD")]
        public bool? AD { get; set; }

        [Required]
        [DisplayName("DB")]
        public bool? DB { get; set; }

        [Required]
        [DisplayName("Description")]
        public string? Descricao { get; set; }

        [Required]
        [DisplayName("Developer's email")]
        public string? Maildev { get; set; }

        [Required]
        [DisplayName("Image Name")]
        public string ?ImageName { get; set; }

        //public int FotoId { get; set; }

        //[ForeignKey("FotoId")]
        //public Foto? Foto { get; set; }
        [NotMapped]
        [DisplayName("Upload file")]
        public IFormFile ?ImageFile { get; set; }


    }
}
