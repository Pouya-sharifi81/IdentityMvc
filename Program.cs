using Identity.Bugeto.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcBuggetoEx.DAL;
using MvcBuggetoEx.Helper;
using MvcBuggetoEx.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataBaseContex>(p =>
{
    p.UseSqlServer("Server=.;Database=IdentityBuggeto;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");
});


//Add dbContex Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataBaseContex>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<CustomIdentityError>()
    .AddPasswordValidator<MyPasswordValidator>()
    .AddRoles<Role>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Buyer", policy =>
    {
        policy.RequireClaim("Buyer");
    });
    options.AddPolicy("BloodType", policy =>
    {
        policy.RequireClaim("Blood", "Ap", "Op");
    }
    );

    options.AddPolicy("Cradit", policy =>
    {
        policy.Requirements.Add(new UserCreditRequerment(10000));
    });

    options.AddPolicy("IsBlogForUser", policy =>
    {
        policy.AddRequirements(new BlogRequirement());
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, IsBlogForUserAuthorizationHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
