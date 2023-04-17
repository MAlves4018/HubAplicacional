using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity;
using System;
using WebApp.Models;   
using System.Threading.Tasks; 
using System.Collections.Generic;
using System.Data;
using WebApp.Models.ApplicationModels;

namespace WebApp.Data
{
    public class ApplicationDbContext : AuditableIdentityContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; } 
        public DbSet<NavigationMenu> NavigationMenus { get; set; } 
        public DbSet<RolePermission> RolePermissions { get; set; }

        //APPLICATION SPECIFIC MODELS  
        public DbSet<WebApp.Models.ApplicationModels.Alertas> Alertas { get; set; }

        //APPLICATION SPECIFIC MODELS 


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RoleMenuPermission>().HasKey(c => new {c.RoleId, c.NavigationMenuId});

            /* force unique username */
            builder.Entity<ApplicationUser>() 
                .HasIndex(x => x.UserName)
                .IsUnique();


            builder.Entity<IdentityRole>()
                .HasIndex(x => x.Name)
                .IsUnique();

            var adminUser = new ApplicationUser() /* password is "P@ssw0rd" */
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "migkx",
                NormalizedUserName = "255667183",
                Email = "alves.mas@exercito.pt",
                NormalizedEmail = "alves.mas@exercito.pt",
                EmailConfirmed = true,
                PasswordHash =
                  "AQAFGTEAACcQAAAAEKYfUQJgm2Shlb0Y27ObJK4ttO3dSheYpae6UCcL084qro1IcPBDyBtgg1LYb2uCzw==",
                SecurityStamp = "4Y6BSSJXKUHFGAEMVEUWL3XH3DEGUTRF",
                ConcurrencyStamp = "21a41ca8-d2e3-46ac-b53f-925edccd1eb7",
            };

            var adminRole = new IdentityRole {Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "Admin"};
            builder.Entity<IdentityRole>().HasData(adminRole);


            builder.Entity<ApplicationUser>().HasData(adminUser);
             
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { UserId = adminUser.Id, RoleId = adminRole.Id }
            );


            var adminMenuId = Guid.NewGuid();
            var tiposMenuId = Guid.NewGuid();
            var tecnologiasMenuId = Guid.NewGuid();
            var estadosTecnologiasId = Guid.NewGuid();

            var menus = new NavigationMenu[]
            {
                new NavigationMenu()
                {
                    Id = adminMenuId,
                    Name = "Admin",
                    ControllerName = "Admin",
                    ActionName = "",
                    ParentMenuId = null,
                    DisplayOrder = 1,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Utilizadores",
                    ControllerName = "Admin",
                    ActionName = "Users",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 2,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Perfis",
                    ControllerName = "Admin",
                    ActionName = "Roles",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 1,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Menus",
                    ControllerName = "Admin",
                    ActionName = "Menus",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 3,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = tecnologiasMenuId,
                    Name = "Tecnologias",
                    ControllerName = "Tecnologias",
                    ActionName = "",
                    ParentMenuId = null,
                    DisplayOrder = 2,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Lista",
                    ControllerName = "Tecnologias",
                    ActionName = "Index",
                    ParentMenuId = tecnologiasMenuId,
                    DisplayOrder = 1,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Monitorização",
                    ControllerName = "Tecnologias",
                    ActionName = "MonitorizacaoPage",
                    ParentMenuId = tecnologiasMenuId,
                    DisplayOrder = 2,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = tiposMenuId,
                    Name = "Tipos de Tecnologias",
                    ControllerName = "Tipos",
                    ActionName = "",
                    ParentMenuId = null,
                    DisplayOrder = 3,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Lista",
                    ControllerName = "Tipos",
                    ActionName = "Index",
                    ParentMenuId = tiposMenuId,
                    DisplayOrder = 1,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = estadosTecnologiasId,
                    Name = "Estados de Tecnologias",
                    ControllerName = "EstadoTecnologias",
                    ActionName = "",
                    ParentMenuId = null,
                    DisplayOrder = 4,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Lista",
                    ControllerName = "EstadoTecnologias",
                    ActionName = "Index",
                    ParentMenuId = estadosTecnologiasId,
                    DisplayOrder = 1,
                    Visible = true,
                },
            };

            builder.Entity<NavigationMenu>().HasData(menus);

            builder.Entity<Tipos>().HasData(
                new Tipos() { Id = 1, Ativo=true, Name="Bases de dados ORACLE", Ordem=1},
                new Tipos() { Id = 2, Ativo =true, Name="Bases de dados SQL Server", Ordem=2},
                new Tipos() { Id = 3, Ativo =true, Name="Servidores Aplicacionais", Ordem=3},
                new Tipos() { Id = 4, Ativo =true, Name="Aplicações", Ordem=4}
            );

            //builder.Entity<Tecnologias>().HasData(
                //new Tecnologias() { Id = 1, Name="GitHub", Descricao="", TypeId = 4 , Sigla = "GitHub", Link="https://github.com" }
          //   );

            foreach (var item in menus)
            {
                builder.Entity<RoleMenuPermission>().HasData(
                    new RoleMenuPermission {RoleId = adminRole.Id, NavigationMenuId = item.Id}
                );
            }


            base.OnModelCreating(builder);
        }

        // AUXILIARY QUERIES FOR NOT MAPPED TABLES (NOT A GOOD PRACTICE, CHANGE WHEN MANY-TO-MANY IS SUPPORTED BY IDENTITYUSER)
        public async Task<IEnumerable<int>> CleanUserDepositos(string userId)
        {
            await using var connection = Database.GetDbConnection().CreateCommand();
            using (connection)
            {
                bool wasOpen = connection.Connection.State == ConnectionState.Open;
                if (!wasOpen) connection.Connection.Open();
                try
                {
                    connection.CommandText = $"DELETE FROM dbo.ApplicationUserDeposito WHERE UsersId = '{userId}'";
                    var result = await connection.ExecuteScalarAsync();
                    return (IEnumerable<int>) result;
                }
                finally
                {
                    if (!wasOpen) connection.Connection.Close();
                }
            }
        }

        // AUXILIARY QUERIES FOR NOT MAPPED TABLES (NOT A GOOD PRACTICE, CHANGE WHEN MANY-TO-MANY IS SUPPORTED BY IDENTITYUSER)
        public DbSet<WebApp.Models.ApplicationModels.Tecnologias> Tecnologias { get; set; } = default!;
         
        public DbSet<WebApp.Models.ApplicationModels.Tipos> Tipos { get; set; } = default!;

        public DbSet<WebApp.Models.ApplicationModels.EstadoTecnologia> EstadosTecnologia { get; set; } = default!;

        public DbSet<WebApp.Models.ApplicationModels.Deletedtecnologies> Deletedtecnologies { get; set; } = default!;

        
    }
}