using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace WebApp.Models.ApplicationModels
{
    public class EstadoTecnologia
    { 
        public int Id { get; set; }

        public int ? IdTecnologia { get; set; }

        [ForeignKey("IdTecnologia")]
        public Tecnologias ? Tecnologias { get; set; }

        public string? NameTecnologia { get; set; }
        public Tecnologias? Tecnologiasname { get; set; }
        
        public bool ADUp { get; set; }
         
        public bool DBUp { get; set; }

        public bool Ok { get; set; }

        public int? StatusCode { get; set; }

        public string? Message { get; set; }

        public DateTime? Timestamp { get; set;}

    } 
}