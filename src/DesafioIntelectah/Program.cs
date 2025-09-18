using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DesafioIntelectah.Data;
using DesafioIntelectah.Models;


var builder = WebApplication.CreateBuilder(args);

var redisConnection = builder.Configuration["Redis:Connection"] ?? "localhost:6379";
src.DesafioIntelectah.RedisConfig.AddRedisCaching(builder.Services, redisConnection);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
           .UseLazyLoadingProxies());

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false) // Alterado para false para facilitar o teste
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();