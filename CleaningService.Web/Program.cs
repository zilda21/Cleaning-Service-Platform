using CleaningService.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages to the container
builder.Services.AddRazorPages();

// Register EF Core with connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register your services (BookService, etc.)
// builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();




// Add services to the container.
// builder.Services.AddRazorPages();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

// app.MapStaticAssets();
app.MapRazorPages();
//    .WithStaticAssets();

app.Run();
