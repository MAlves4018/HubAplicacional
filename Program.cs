
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models.ApplicationModels;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container. 
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(200);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = false;
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>()
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

//Ignorar loops infinitos nas chamadas a objetos que tem filhos com variáveis desse mesmo objeto.
//builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); 


builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

// Add Navigation menu factory service 
builder.Services.AddScoped<IDynamicAuthorizationDataService, DynamicAuthorizationDataService>();
builder.Services.AddScoped<IAuthorizationHandler, DynamicAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserMessages, UserMessages>();

builder.Services.AddRazorPages();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();
    }
}
if (app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}


app.UseHttpsRedirection();
//app.UseFileServer(new FileServerOptions()
//{
//    FileProvider = new PhysicalFileProvider(
//    Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
//    RequestPath = new PathString("/node_modules"),
//    EnableDirectoryBrowsing = true
//});
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

