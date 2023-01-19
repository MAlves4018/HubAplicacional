
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserRolesViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Nome do perfil")]

        public string Name { get; set; }

        [Display(Name = "Selecionado")]
        public bool Selected { get; set; }
    }
}
