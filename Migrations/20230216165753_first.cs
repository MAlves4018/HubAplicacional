using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CabecalhoAviso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextoAviso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetNavigationMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    NotAnActionOrController = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetNavigationMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetNavigationMenus_AspNetNavigationMenus_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalTable: "AspNetNavigationMenus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UnidadeId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Get = table.Column<bool>(type: "bit", nullable: true),
                    Post = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleMenuPermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NavigationMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Get = table.Column<bool>(type: "bit", nullable: true),
                    Post = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleMenuPermissions", x => new { x.RoleId, x.NavigationMenuId });
                    table.ForeignKey(
                        name: "FK_AspNetRoleMenuPermissions_AspNetNavigationMenus_NavigationMenuId",
                        column: x => x.NavigationMenuId,
                        principalTable: "AspNetNavigationMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetRoleMenuPermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlertasApplicationUser",
                columns: table => new
                {
                    AlertasId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertasApplicationUser", x => new { x.AlertasId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AlertasApplicationUser_Alertas_AlertasId",
                        column: x => x.AlertasId,
                        principalTable: "Alertas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertasApplicationUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRolesViewModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Selected = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolesViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRolesViewModel_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tecnologias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sigla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Linkdocs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Linklogs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Linkreports = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Maildev = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnologias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tecnologias_Tipos_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Tipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstadosTecnologia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTecnologia = table.Column<int>(type: "int", nullable: true),
                    ADUp = table.Column<bool>(type: "bit", nullable: false),
                    DBUp = table.Column<bool>(type: "bit", nullable: false),
                    Ok = table.Column<bool>(type: "bit", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TecnologiasId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosTecnologia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstadosTecnologia_Tecnologias_TecnologiasId",
                        column: x => x.TecnologiasId,
                        principalTable: "Tecnologias",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetNavigationMenus",
                columns: new[] { "Id", "ActionName", "ControllerName", "DisplayOrder", "ExternalUrl", "IsExternal", "Name", "NotAnActionOrController", "ParentMenuId", "Visible" },
                values: new object[,]
                {
                    { new Guid("17b0525f-23fc-4eb9-9230-def955f15798"), "", "Admin", 1, null, false, "Admin", false, null, true },
                    { new Guid("50db0ac2-6195-4c1a-95d9-8d6a7100c783"), "", "Tipos", 3, null, false, "Tipos de Tecnologias", false, null, true },
                    { new Guid("a7aa2d78-e3ed-42f5-8a89-ba42520eb7a7"), "", "Tecnologias", 2, null, false, "Tecnologias", false, null, true },
                    { new Guid("b7ca7bba-de41-4233-8c23-0e8df61742f8"), "", "EstadoTecnologias", 4, null, false, "Estados de Tecnologias", false, null, true }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UnidadeId", "UserName" },
                values: new object[,]
                {
                    { "2a93c694-5d4e-4404-8b03-2c18a5600c5c", 0, "21a41ca8-d2e3-46ac-b53f-925edccd1eb7", "lourenco.kc@exercito.pt", true, false, null, "lourenco.kc@exercito.pt", "15283213", "AQAAAAEAACcQAAAAEKYfUQJgm2Shlb0Y27ObJK4ttO3dSheYpae6UCcL084qro1IcPBDyBtgg1LYb2uCzw==", null, false, "4Y6BSSJXKUHFGAEMVEUWL3XH3DEGUTRF", false, null, "15283213" },
                    { "6cda8944-fd9b-4998-a5c9-c3469fd90796", 0, "21a41ca8-d2e3-46ac-b53f-925edccd1eb7", "atanasio.gc@exercito.pt", true, false, null, "atanasio.gc@exercito.pt", "03077412", "AQAAAAEAACcQAAAAEKYfUQJgm2Shlb0Y27ObJK4ttO3dSheYpae6UCcL084qro1IcPBDyBtgg1LYb2uCzw==", null, false, "4Y6BSSJXKUHFGAEMVEUWL3XH3DEGUTRF", false, null, "03077412" },
                    { "91ba41fa-2435-47db-98c2-2750107df18f", 0, "21a41ca8-d2e3-46ac-b53f-925edccd1eb7", "alves.mas@exercito.pt", true, false, null, "alves.mas@exercito.pt", "255667182", "AQAFGTEAACcQAAAAEKYfUQJgm2Shlb0Y27ObJK4ttO3dSheYpae6UCcL084qro1IcPBDyBtgg1LYb2uCzw==", null, false, "4Y6BSSJXKUHFGAEMVEUWL3XH3DEGUTRF", false, null, "255667182" }
                });

            migrationBuilder.InsertData(
                table: "Tipos",
                columns: new[] { "Id", "Ativo", "Name", "Ordem" },
                values: new object[,]
                {
                    { 1, true, "Bases de dados ORACLE", 1 },
                    { 2, true, "Bases de dados SQL Server", 2 },
                    { 3, true, "Servidores Aplicacionais", 3 },
                    { 4, true, "Aplicações", 4 }
                });

            migrationBuilder.InsertData(
                table: "AspNetNavigationMenus",
                columns: new[] { "Id", "ActionName", "ControllerName", "DisplayOrder", "ExternalUrl", "IsExternal", "Name", "NotAnActionOrController", "ParentMenuId", "Visible" },
                values: new object[,]
                {
                    { new Guid("346af19e-187f-412b-9ec6-b886b90de93c"), "Menus", "Admin", 3, null, false, "Menus", false, new Guid("17b0525f-23fc-4eb9-9230-def955f15798"), true },
                    { new Guid("43cbcdcf-ca91-4132-abaf-09dff23d5655"), "Index", "Tipos", 1, null, false, "Lista", false, new Guid("50db0ac2-6195-4c1a-95d9-8d6a7100c783"), true },
                    { new Guid("53b8b282-5a2d-472d-afd2-70da3f273c3d"), "Users", "Admin", 2, null, false, "Utilizadores", false, new Guid("17b0525f-23fc-4eb9-9230-def955f15798"), true },
                    { new Guid("81f469a2-cd01-4b84-89cc-cb476fd7d0c5"), "Index", "EstadoTecnologias", 1, null, false, "Lista", false, new Guid("b7ca7bba-de41-4233-8c23-0e8df61742f8"), true },
                    { new Guid("84cf7f95-3c25-4b7a-952f-513adf6f0a2a"), "MonitorizacaoPage", "Tecnologias", 2, null, false, "Monitorização", false, new Guid("a7aa2d78-e3ed-42f5-8a89-ba42520eb7a7"), true },
                    { new Guid("8ede136e-52c1-4e6d-9e08-9a367ef3d8b2"), "Index", "Tecnologias", 1, null, false, "Lista", false, new Guid("a7aa2d78-e3ed-42f5-8a89-ba42520eb7a7"), true },
                    { new Guid("bdb1a8c8-05f0-4d2c-a5d6-3b900b9a9f13"), "Roles", "Admin", 1, null, false, "Perfis", false, new Guid("17b0525f-23fc-4eb9-9230-def955f15798"), true }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleMenuPermissions",
                columns: new[] { "NavigationMenuId", "RoleId", "Get", "Post" },
                values: new object[,]
                {
                    { new Guid("17b0525f-23fc-4eb9-9230-def955f15798"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null },
                    { new Guid("50db0ac2-6195-4c1a-95d9-8d6a7100c783"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null },
                    { new Guid("a7aa2d78-e3ed-42f5-8a89-ba42520eb7a7"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null },
                    { new Guid("b7ca7bba-de41-4233-8c23-0e8df61742f8"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", "2a93c694-5d4e-4404-8b03-2c18a5600c5c" },
                    { "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", "6cda8944-fd9b-4998-a5c9-c3469fd90796" },
                    { "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", "91ba41fa-2435-47db-98c2-2750107df18f" }
                });

            migrationBuilder.InsertData(
                table: "Tecnologias",
                columns: new[] { "Id", "Descricao", "ImageName", "Link", "Linkdocs", "Linklogs", "Linkreports", "Maildev", "Name", "Sigla", "TypeId" },
                values: new object[,]
                {
                    { 1, "", null, "https://exe-webserver02.exercito.local/REF_DCSI", null, null, null, null, "nome", "nome", 1 },
                    { 2, "", null, "https://exe-webserver02.exercito.local/REF_DCSI", null, null, null, null, "nome1", "nome1", 2 },
                    { 3, "", null, "https://exe-webserver02.exercito.local/REF_DCSI", null, null, null, null, "nome2", "nome2", 3 },
                    { 4, "", null, "https://exe-webserver02.exercito.local/REF_DCSI", null, null, null, null, "nome3", "nome3", 4 }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleMenuPermissions",
                columns: new[] { "NavigationMenuId", "RoleId", "Get", "Post" },
                values: new object[,]
                {
                    { new Guid("346af19e-187f-412b-9ec6-b886b90de93c"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null },
                    { new Guid("43cbcdcf-ca91-4132-abaf-09dff23d5655"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null },
                    { new Guid("53b8b282-5a2d-472d-afd2-70da3f273c3d"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null },
                    { new Guid("81f469a2-cd01-4b84-89cc-cb476fd7d0c5"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null },
                    { new Guid("84cf7f95-3c25-4b7a-952f-513adf6f0a2a"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null },
                    { new Guid("8ede136e-52c1-4e6d-9e08-9a367ef3d8b2"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null },
                    { new Guid("bdb1a8c8-05f0-4d2c-a5d6-3b900b9a9f13"), "c611d808-b15c-4f18-ba1f-5ed9a4b38dd6", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertasApplicationUser_UsersId",
                table: "AlertasApplicationUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetNavigationMenus_ParentMenuId",
                table: "AspNetNavigationMenus",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleMenuPermissions_NavigationMenuId",
                table: "AspNetRoleMenuPermissions",
                column: "NavigationMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_Name",
                table: "AspNetRoles",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosTecnologia_TecnologiasId",
                table: "EstadosTecnologia",
                column: "TecnologiasId");

            migrationBuilder.CreateIndex(
                name: "IX_Tecnologias_TypeId",
                table: "Tecnologias",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolesViewModel_ApplicationUserId",
                table: "UserRolesViewModel",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertasApplicationUser");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetRoleMenuPermissions");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "EstadosTecnologia");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRolesViewModel");

            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "AspNetNavigationMenus");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Tecnologias");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Tipos");
        }
    }
}
