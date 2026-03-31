using CleaningService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(cs))
{
    throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(cs));

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapGet("/time/utc", () => Results.Ok(DateTime.UtcNow));
app.MapRazorPages();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
// Render handles HTTPS before traffic reaches the app
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// Test route
app.MapGet("/time/utc", () => Results.Ok(DateTime.UtcNow));

// Endpoints
app.MapRazorPages();
app.MapControllers();



app.Run();