using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Data
{
    [Table(name: "AspNetRoleMenuPermissions")]
    public class RoleMenuPermission
    {
        [Column(TypeName = "nvarchar(450)")]
        [Display(Name = "Perfil")]

        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; }

        public Guid NavigationMenuId { get; set; }

        [ForeignKey("NavigationMenuId")]
        [Display(Name = "Menu")]
        public virtual NavigationMenu NavigationMenu { get; set; }


        [Display(Name = "Ler")]
        public bool? Get { get; set; }

        [Display(Name = "Escrever")]
        public bool? Post { get; set; }


    }
}