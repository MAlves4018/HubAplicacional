using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{

    public class RolePermission
    {
        public Guid Id { get; set; }

        public Guid? ParentMenuId { get; set; }

        public string Name { get; set; }


        [Display(Name = "Entrar")]
        public bool? Get { get; set; }

        [Display(Name = "Guardar")]
        public bool? Post { get; set; }

        [NotMapped]
        public bool NoNullGet
        {
            get { return Get ?? false; }
            set { Get = value; }
        }

        [NotMapped]
        public bool NoNullPost
        {
            get { return Post ?? false; }
            set { Post = value; }
        }
    }
}