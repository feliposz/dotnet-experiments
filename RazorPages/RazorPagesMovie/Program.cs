using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<RazorPageMovieContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("RazorPageMovieContext")));
}
else
{
    builder.Services.AddDbContext<RazorPageMovieContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionMovieContext")));
}


var app = builder.Build();

// Get a database context instance from the dependency injection container
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Call the initialize method passing the database context instance
    SeedData.Initialize(services);
    // Dispose the context
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseRequestLocalization("pt-BR", "en-US");

app.Run();
