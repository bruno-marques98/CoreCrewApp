using CoreCrewApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext with SQL Server using the connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services with roles support
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>() // Enable roles
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// Seed roles if they do not exist
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "User" }; // Define roles here

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

await SeedAdminUser(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enable authentication
app.UseAuthorization();  // Enable authorization

// Redirect to login if not authenticated
// Redirect to Login if the user is not authenticated
app.MapGet("/", async context =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        context.Response.Redirect("/Home/Index");
    }
    else
    {
        context.Response.Redirect("/Identity/Account/Login");
    }
    await Task.CompletedTask;
});

// Default route mapping for controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Enable Razor Pages for Identity UI

app.Run();


// Seed method
async Task SeedAdminUser(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var scopedServices = scope.ServiceProvider;
        var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

        // Create Admin role if it doesn't exist
        var roleExists = await roleManager.RoleExistsAsync("Admin");
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Create Admin user if it doesn't exist
        const string adminEmail = "admin@example.com"; // Change this to your desired admin email
        const string adminPassword = "Admin@123"; // Use a strong password

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}