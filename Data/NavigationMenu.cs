using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Data
{
	[Table(name: "AspNetNavigationMenus")]
	public class NavigationMenu
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public Guid? ParentMenuId { get; set; }

		[ForeignKey("ParentMenuId")]
		public virtual NavigationMenu ParentNavigationMenu { get; set; }
		
		 
		public string ? ControllerName { get; set; }

		public string ? ActionName { get; set; }
		
		public bool IsExternal { get; set; }

		public string ? ExternalUrl { get; set; }

		public int DisplayOrder { get; set; }

		public bool Visible { get; set; }

        public bool NotAnActionOrController { get; set; }
    }
}