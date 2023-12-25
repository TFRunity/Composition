using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Composition.Database;
using Composition.Models;
using System.Security.Claims;
using Composition.Interfaces;
using Composition.Database.Methods;


// App start
var _builder = WebApplication.CreateBuilder(args);

//Services
_builder.Services.AddRouting();
_builder.Services.AddControllersWithViews();
_builder.Services.AddRazorPages();
_builder.Services.AddAuthentication();
_builder.Services.AddAuthorization();

//Dependency Injection
_builder.Services.AddScoped<IdentityDbContext<User, UserRole, Guid>, AppDBContext>();
_builder.Services.AddTransient<IProductRepository, ProductMethods>();
_builder.Services.AddTransient<ISubCategoryRepository, SubCategoryMethods>();
_builder.Services.AddTransient<ICategoryRepository, CategoryMethods>();
_builder.Services.AddTransient<IUserPicturesRepository, UserPicturesMethods>();
_builder.Services.AddTransient<IUserRepository, UserMethods>();
_builder.Services.AddTransient<IOrderRepository, OrderMethods>();
_builder.Services.AddTransient<IRelationshipProductCategory, RelationshipProductCategoryMethods>();
//DBContext
_builder.Services.AddDbContext<AppDBContext>(opts =>
{
    opts.UseLazyLoadingProxies()
        .UseSqlServer(_builder.Configuration["ConnectionStrings:IdentityConnection"]);
}).AddIdentity<User, UserRole>()
  .AddEntityFrameworkStores<AppDBContext>()
  .AddDefaultTokenProviders();


//Cookie
_builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Account/Login";
    config.AccessDeniedPath = "/Home/AccessDenied";
});

//Authorization
_builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", builder =>
    {
        builder.RequireClaim(ClaimTypes.Role, "Admin");
    });
    options.AddPolicy("Manager", builder =>
    {
        builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Admin")
                                   || x.User.HasClaim(ClaimTypes.Role, "Manager"));
    });
    options.AddPolicy("Member", builder =>
    {
        builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Member")
                                   || x.User.HasClaim(ClaimTypes.Role, "Manager")
                                   || x.User.HasClaim(ClaimTypes.Role, "Admin"));
    });
});

//Identity configure
_builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+!-";
    options.User.RequireUniqueEmail = false;
});

//App properties
var app = _builder.Build();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();


//Endpoints
app.MapControllerRoute("default","{controller=Home}/{Action=Index}");
app.MapRazorPages();


//Last command
await CreateRoles(app.Services);
await MakeMockSeed(app.Services);
app.Run();

static async Task CreateRoles(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
        string[] roleNames = { "Admin", "Manager", "Member", "Anon" };
        IdentityResult roleResult;
        foreach (var Name in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(Name);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new UserRole(Name));
            }
        }
        var poweruser = new User
        {
            UserName = "Alexey",
            Email = "Alex200346babanov@gmail.com"
        };
        string userPWD = "310185LexaTFRunity";
        var _user = await userManager.FindByEmailAsync("Alex200346babanov@gmail.com");
        if (_user == null)
        {
            var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);
            if (createPowerUser.Succeeded)
            {
                await userManager.AddClaimAsync(poweruser, new Claim(ClaimTypes.Role, "Admin"));

            }
        }
    }
}

static async Task MakeMockSeed(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
        var _user = await userManager.FindByEmailAsync("adex@gmail.com");
        if (_user == null)
        {
            string pwd = "1TFRunity";
            User user1 = new User { UserName = "Aboba1", Email = "adex@gmail.com"};
            User user2 = new User { UserName = "Aboba2", Email = "acex@gmail.com" };
            User user3 = new User { UserName = "Aboba3", Email = "awex@gmail.com" };
            List<User> users = new List<User>
            {
                user1,
                user2,
                user3
            };
            foreach (User user in users)
            {
                await userManager.CreateAsync(user, pwd);
            }
        }
    }
}