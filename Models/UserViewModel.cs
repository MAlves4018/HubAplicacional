
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Models.ApplicationModels;

namespace WebApp.Models
{
    public class UserViewModel
    {
       /* [Required]*/
        public string Id { get; set; }

        [Required(ErrorMessage = "O NIM é obrigatório")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O NIM tem 8 algarismos")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Utilize apenas algarismos")]
        [Display(Name = "NIM")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Telemóvel")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(16, MinimumLength = 9, ErrorMessage = "O telefone tem pelo menos 9 algarismos")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{9,15}$", ErrorMessage = "O número indicado não é válido")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Perfis")]
        public UserRolesViewModel[] Roles { get; set; }
        
        [Display(Name = "Permissões")]
        public RolePermission[] Permissions { get; set; }
        [Display(Name = "Entidades Setoriais")]
        public  Unidade Unidade { get; set; }
    }
}
