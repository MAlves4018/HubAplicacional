﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<Unidade> Unidades { get; set; }

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
                UserName = "03077412",
                NormalizedUserName = "03077412",
                Email = "atanasio.gc@exercito.pt",
                NormalizedEmail = "atanasio.gc@exercito.pt",
                EmailConfirmed = true,
                PasswordHash =
                    "AQAAAAEAACcQAAAAEKYfUQJgm2Shlb0Y27ObJK4ttO3dSheYpae6UCcL084qro1IcPBDyBtgg1LYb2uCzw==",
                SecurityStamp = "4Y6BSSJXKUHFGAEMVEUWL3XH3DEGUTRF",
                ConcurrencyStamp = "21a41ca8-d2e3-46ac-b53f-925edccd1eb7",  
            };
            var adminUser2 = new ApplicationUser() /* password is "P@ssw0rd" */
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "15283213",
                NormalizedUserName = "15283213",
                Email = "lourenco.kc@exercito.pt",
                NormalizedEmail = "lourenco.kc@exercito.pt",
                EmailConfirmed = true,
                PasswordHash =
                  "AQAAAAEAACcQAAAAEKYfUQJgm2Shlb0Y27ObJK4ttO3dSheYpae6UCcL084qro1IcPBDyBtgg1LYb2uCzw==",
                SecurityStamp = "4Y6BSSJXKUHFGAEMVEUWL3XH3DEGUTRF",
                ConcurrencyStamp = "21a41ca8-d2e3-46ac-b53f-925edccd1eb7",
            };

            var adminRole = new IdentityRole {Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "Admin"};
            builder.Entity<IdentityRole>().HasData(adminRole);


            builder.Entity<ApplicationUser>().HasData(adminUser, adminUser2);
             
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { UserId = adminUser.Id, RoleId = adminRole.Id },
                new IdentityUserRole<string>() { UserId = adminUser2.Id, RoleId = adminRole.Id }
            );


            var adminMenuId = Guid.NewGuid();
            var menus = new NavigationMenu[]
            {
                new NavigationMenu()
                {
                    Id = adminMenuId,
                    Name = "Admin",
                    ControllerName = "",
                    ActionName = "",
                    ParentMenuId = null,
                    DisplayOrder = 1,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Ver Perfils",
                    ControllerName = "Admin",
                    ActionName = "Roles",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 1,
                    Visible = true,
                },

                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Ver Utilizadores",
                    ControllerName = "Admin",
                    ActionName = "Users",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 2,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Editar menus",
                    ControllerName = "Admin",
                    ActionName = "Menus",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 3,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Criar Perfil",
                    ControllerName = "Admin",
                    ActionName = "CreateRole",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 4,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Editar Perfil",
                    ControllerName = "Admin",
                    ActionName = "EditRole",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 5,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Criar Utilizador",
                    ControllerName = "Admin",
                    ActionName = "CreateUser",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 6,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = Guid.NewGuid(),
                    Name = "Editar Utilizador",
                    ControllerName = "Admin",
                    ActionName = "EditUser",
                    ParentMenuId = adminMenuId,
                    DisplayOrder = 7,
                    Visible = false,
                }
            };

            builder.Entity<NavigationMenu>().HasData(menus);


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

       
    }
}