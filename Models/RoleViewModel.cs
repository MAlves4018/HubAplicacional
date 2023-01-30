
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RoleViewModel
    {
		public string? Id { get; set; }

        [Display(Name = "Nome do Perfil")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 255 caracteres")]
        [RegularExpression(@"^[\S]+[\S\s]*[\S]+$", ErrorMessage = "O nome indicado não é válido")]
        
        public string? Name { get; set; }

        [Display(Name = "Permissões")]
        public RolePermission[]? Permissions { get; set; }
        
        [Display(Name = "Utilizadores deste perfil")]
        public List<RoleUsersViewModel>?  Members{ get; set; }
    }
}
