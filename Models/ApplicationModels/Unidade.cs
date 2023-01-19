using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace WebApp.Models.ApplicationModels
{
    public class Unidade
    {
        [Key]
        public int Id { get; set; }


        public string Nome { get; set; }

        public string Sigla { get; set; }

        public int? Ordem { get; set; }
          

        public ICollection<ApplicationUser>? Users { get; set; }
         
    }


}
