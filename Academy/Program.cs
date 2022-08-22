using Academy.DAL;
using Academy.DAL.Repositories;
using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Academy.Service.Implementations;
using Academy.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBaseRepository<Student>, StudentRepository>();
builder.Services.AddScoped<IBaseRepository<Group>, GroupRepository>();
builder.Services.AddScoped<IBaseRepository<Course>, CourseRepository>();

builder.Services.AddScoped<IHierarchyService<Student>, StudentService>();
builder.Services.AddScoped<IHierarchyService<Group>, GroupService>();
builder.Services.AddScoped<IHierarchyService<Course>, CourseService>();

builder.Services.AddControllers().AddControllersAsServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithRedirects("/Error/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Course}/{action=GetAll}/{id?}");

app.Run();