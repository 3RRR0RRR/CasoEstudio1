using LavacarBLL;
using LavacarBLL.Servicios.Cita;
using LavacarBLL.Servicios.Cliente;
using LavacarBLL.Servicios.Vehiculo;
using LavacarDAL.Data;
using LavacarDAL.Repositorios.Cita;
using LavacarDAL.Repositorios.Cliente;
using LavacarDAL.Repositorios.Generico;
using LavacarDAL.Repositorios.Vehiculo;
using LavacarWeb.Middleware;
using Microsoft.EntityFrameworkCore;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "lavacar.db");

// ✅ SQLite: usar SIEMPRE el archivo dentro del proyecto (LavacarWeb/Data/lavacar.db)
builder.Services.AddDbContext<LavacarDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}")
);

// DEBUG: ver la ruta exacta del archivo que se está usando
Console.WriteLine("DB PATH => " + dbPath);


// ✅ IMPORTANTE: NO crear DB vacía automáticamente.
// Si no existe el archivo lavacar.db en esta ruta, detenemos la app con un mensaje claro.
if (!File.Exists(dbPath))
{
    throw new FileNotFoundException(
        "No se encontró la base de datos SQLite en: " + dbPath + Environment.NewLine +
        "Coloca tu archivo lavacar.db (el que llenaste en DB Browser) dentro de LavacarWeb/Data/ y vuelve a ejecutar."
    );
}
builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IVehiculoRepositorio, VehiculoRepositorio>();
builder.Services.AddScoped<ICitaRepositorio, CitaRepositorio>();

builder.Services.AddScoped<IClienteServicio, ClienteServicio>();
builder.Services.AddScoped<IVehiculoServicio, VehiculoServicio>();
builder.Services.AddScoped<ICitaServicio, CitaServicio>();

builder.Services.AddAutoMapper(cfg => { }, typeof(MapeoClases));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseMiddleware<MiddlewareGlobalExceptionHandler>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();