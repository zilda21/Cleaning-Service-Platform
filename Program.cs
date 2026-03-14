using CleaningService.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();


builder.Services.AddControllers();

// ✅ HttpClientFactory (fixes: Unable to resolve IHttpClientFactory)
builder.Services.AddHttpClient();


builder.Services.AddSession();
builder.Services.AddHttpContextAccessor(); // <-- THIS LINE WAS MISSING

// ✅ Session (for Role stored in session)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(cs))
    throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(cs));

var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ IMPORTANT: Session after routing, before endpoints
app.UseSession();

// (You are NOT using real auth yet)
// app.UseAuthentication();
// app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();