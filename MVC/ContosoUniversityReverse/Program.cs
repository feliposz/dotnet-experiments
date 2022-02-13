using Microsoft.EntityFrameworkCore;
using ContosoUniversityReverse.Data;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

//MyInitDb(app);
//MyTestRoutes(app);
app.Run();


// ----------------------------------------------------

void MyInitDb(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        DbInitializer.Initialize(context);
    }
}

void MyTestRoutes(WebApplication app)
{
    app.MapGet("/", () => "Hello World!");

    app.MapGet("/DbScript", (ApplicationDbContext context) => context.Database.GenerateCreateScript());

    app.MapGet("/Students", async (ApplicationDbContext context) =>
    {
        var students = await context.Students.Select(s => s.LastName).ToListAsync();
        return String.Join(",", students);
    });

    app.MapGet("/Instructors", async (ApplicationDbContext context) =>
    {
        var instructors = await context.Instructors
            .Include(x => x.Courses)
                .ThenInclude(x => x.Department)
            .Include(x => x.OfficeAssignment)
            .AsNoTracking()
            .Select(x => new
            {
                LastName = x.LastName,
                FirstName = x.FirstName,
                Location = x.OfficeAssignment.Location,
                Courses = x.Courses.Select(y => new { Department = y.Department.Name, Title = y.Title })
            })
            .ToListAsync();
        return JsonConvert.SerializeObject(instructors);
    });
}
