using System;
using System.Linq;
using System.Threading.Tasks;
using DestinoEnsueno.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DestinoEnsueno.API.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("User")) await roleManager.CreateAsync(new IdentityRole("User"));

            if (await userManager.FindByEmailAsync("admin@destino.com") == null)
            {
                var admin = new ApplicationUser { UserName = "admin@destino.com", Email = "admin@destino.com", FirstName = "Admin", LastName = "Sistema" };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if (await userManager.FindByEmailAsync("user@destino.com") == null)
            {
                var user = new ApplicationUser { UserName = "user@destino.com", Email = "user@destino.com", FirstName = "Estudiante", LastName = "Prueba" };
                await userManager.CreateAsync(user, "User123!");
                await userManager.AddToRoleAsync(user, "User");
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Restaurantes", Description = "Comida y bebida" },
                    new Category { Name = "Miradores", Description = "Vistas panorámicas" },
                    new Category { Name = "Emprendimientos", Description = "Negocios locales" },
                    new Category { Name = "Alojamientos", Description = "Hoteles y hostales" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Businesses.Any())
            {
                var catRest = context.Categories.First(c => c.Name == "Restaurantes").Id;
                var catMir = context.Categories.First(c => c.Name == "Miradores").Id;

                var b1 = new Business { Name = "Café del Sol", Description = "El mejor café orgánico", Address = "Calle Principal 123", Phone = "555-0100", CategoryId = catRest, Latitude = 4.6097, Longitude = -74.0817, IsPublished = true };
                var b2 = new Business { Name = "Mirador La Cima", Description = "Vista increíble de la ciudad", Address = "Cerro Alto", Phone = "555-0200", CategoryId = catMir, Latitude = 4.6110, Longitude = -74.0800, IsPublished = true };
                
                context.Businesses.AddRange(b1, b2);
                await context.SaveChangesAsync();

                var user = await userManager.FindByEmailAsync("user@destino.com");
                if (user != null) {
                    context.Reviews.Add(new Review { BusinessId = b1.Id, UserId = user.Id, Rating = 5, Comment = "Excelente servicio y sabor." });
                    await context.SaveChangesAsync();
                }

                context.Events.Add(new Event { Name = "Música en vivo", Description = "Banda de jazz local", Date = DateTime.UtcNow.AddDays(2), Time = new TimeSpan(19, 0, 0), BusinessId = b1.Id, IsPublished = true, Price = 10, Location = b1.Address });
                context.Discounts.Add(new Discount { Name = "20% en Lattes", Description = "Solo esta semana", Percentage = 20, StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(7), BusinessId = b1.Id, IsPublished = true });
                await context.SaveChangesAsync();
            }
        }
    }
}
