using System.ComponentModel.DataAnnotations;


namespace WebApp.Models
{
    public class NavigationMenuModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Ascendente")]
        public Guid? ParentMenuId { get; set; }

        [Display(Name = "Controlador")]
        public string? ControllerName { get; set; }

        [Display(Name = "Comando")]
        public string? ActionName { get; set; }

        [Display(Name = "Link externo")]
        public bool IsExternal { get; set; }

        [Display(Name = "Página externa")]
        public string? ExternalUrl { get; set; }

        [Display(Name = "Ordem de visualização")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Visível")]
        public bool Visible { get; set; }

        [Display(Name = "Não é uma Action ou um controller (ex: menu de organização)")]
        public bool NotAnActionOrController { get; set; }

    }
}
