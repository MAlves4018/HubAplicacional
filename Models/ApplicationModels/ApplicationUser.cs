using WebApp.Models; 
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.ApplicationModels
{
    public class ApplicationUser : IdentityUser
    {
    
        [PersonalData]
        public ICollection<Alertas> Alertas { get; set; }

        public ICollection<UserRolesViewModel>? Roles { get; set; }

        [PersonalData]
        public Unidade Unidade { get; set; }
        public int? UnidadeId { get; set; }


    }
}
