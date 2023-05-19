using Microsoft.AspNetCore.Identity;

namespace WebApp.Models.ApplicationModels
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        public ICollection<Alertas> Alertas { get; set; }

        public ICollection<UserRolesViewModel>? Roles { get; set; }
        public int? UnidadeId { get; set; }


    }
}
