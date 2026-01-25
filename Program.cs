using CleaningService.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// UI ONLY / DEMO MODE
builder.Services.AddRazorPages();

// ❌ DATABASE & AUTH DISABLED FOR DEMO
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ❌ AUTH DISABLED FOR DEMO
// app.UseAuthorization();

app.MapRazorPages();

app.Run();