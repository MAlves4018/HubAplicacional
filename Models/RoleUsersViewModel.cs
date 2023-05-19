
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RoleUsersViewModel
    {
        public string Id { get; set; }

        [Display(Name = "NIM")]
        public string UserName { get; set; }

        [Display(Name = "Escolhido")]
        public bool Selected { get; set; }

    }
}
