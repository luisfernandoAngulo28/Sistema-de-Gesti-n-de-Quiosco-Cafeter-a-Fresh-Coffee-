using Microsoft.EntityFrameworkCore;
using QuioscoAPI.Models;

namespace QuioscoAPI.Data;

public static class DataSeederLFAH
{
    public static async Task SeedAsync(QuioscoDbContext context)
    {
        // Seed mesas siempre, independiente de si los productos ya existen
        if (!context.Mesas.Any())
        {
            var mesas = Enumerable.Range(1, 8).Select(n => new Mesa
            {
                Numero     = n,
                Capacidad  = n <= 4 ? 2 : 4,
                Disponible = true,
                CreatedAt  = DateTime.UtcNow,
                UpdatedAt  = DateTime.UtcNow
            }).ToList();

            context.Mesas.AddRange(mesas);
            await context.SaveChangesAsync();
        }

        // Si ya tiene los 59 productos correctos, no hace nada
        if (context.Productos.Count() >= 59) return;

        // Limpia y reinicia las secuencias para que los IDs coincidan con el frontend
        await context.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE pedido_productos, pedidos, productos, categorias RESTART IDENTITY CASCADE");

        var categorias = new List<Categoria>
        {
            new() { Nombre = "Café",         Icono = "cafe",        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Hamburguesas",  Icono = "hamburguesa", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizzas",        Icono = "pizza",       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Donas",         Icono = "dona",        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pasteles",      Icono = "pastel",      CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Galletas",      Icono = "galletas",    CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        context.Categorias.AddRange(categorias);
        await context.SaveChangesAsync();

        // Referencia por índice (EF Core asigna los IDs en orden: 1-6)
        int cafe         = categorias[0].Id;
        int hamburguesas = categorias[1].Id;
        int pizzas       = categorias[2].Id;
        int donas        = categorias[3].Id;
        int pasteles     = categorias[4].Id;
        int galletas     = categorias[5].Id;

        var productos = new List<Producto>
        {
            // ── Café (14) ──────────────────────────────────────────────────────
            new() { Nombre = "Café Caramel con Chocolate",               Precio = 59.9, Imagen = "cafe_01",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Frio con Chocolate Grande",           Precio = 49.9, Imagen = "cafe_02",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Latte Frio con Chocolate Grande",          Precio = 54.9, Imagen = "cafe_03",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Latte Frio con Chocolate Grande",          Precio = 54.9, Imagen = "cafe_04",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Malteada Fria con Chocolate Grande",       Precio = 54.9, Imagen = "cafe_05",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Mocha Caliente Chico",                Precio = 39.9, Imagen = "cafe_06",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Mocha Caliente Grande con Chocolate", Precio = 59.9, Imagen = "cafe_07",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Caliente Capuchino Grande",           Precio = 59.9, Imagen = "cafe_08",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Mocha Caliente Mediano",              Precio = 49.9, Imagen = "cafe_09",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Mocha Frio con Caramelo Mediano",     Precio = 49.9, Imagen = "cafe_10",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Mocha Frio con Chocolate Mediano",    Precio = 49.9, Imagen = "cafe_11",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Espresso",                            Precio = 29.9, Imagen = "cafe_12",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Capuchino Grande con Caramelo",       Precio = 59.9, Imagen = "cafe_13",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Café Caramelo Grande",                     Precio = 59.9, Imagen = "cafe_14",         Disponible = true, CategoriaId = cafe,         CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // ── Donas (14) ─────────────────────────────────────────────────────
            new() { Nombre = "Paquete de 3 donas de Chocolate",              Precio = 39.9, Imagen = "donas_01",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete de 3 donas Glaseadas",                 Precio = 39.9, Imagen = "donas_02",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Dona de Fresa",                                Precio = 19.9, Imagen = "donas_03",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Dona con Galleta de Chocolate",                Precio = 19.9, Imagen = "donas_04",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Dona glass con Chispas Sabor Fresa",           Precio = 19.9, Imagen = "donas_05",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Dona glass con Chocolate",                     Precio = 19.9, Imagen = "donas_06",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Dona de Chocolate con MÁS Chocolate",          Precio = 19.9, Imagen = "donas_07",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete de 3 donas de Chocolate",              Precio = 39.9, Imagen = "donas_08",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete de 3 donas con Vainilla y Chocolate",  Precio = 39.9, Imagen = "donas_09",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete de 6 Donas",                           Precio = 69.9, Imagen = "donas_10",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete de 3 Variadas",                        Precio = 39.9, Imagen = "donas_11",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Dona Natural con Chocolate",                   Precio = 19.9, Imagen = "donas_12",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete de 3 Donas de Chocolate con Chispas",  Precio = 39.9, Imagen = "donas_13",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Dona Chocolate y Coco",                        Precio = 19.9, Imagen = "donas_14",   Disponible = true, CategoriaId = donas,        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // ── Galletas (6) ───────────────────────────────────────────────────
            new() { Nombre = "Paquete Galletas de Chocolate",           Precio = 29.9, Imagen = "galletas_01",     Disponible = true, CategoriaId = galletas,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete Galletas de Chocolate y Avena",   Precio = 39.9, Imagen = "galletas_02",     Disponible = true, CategoriaId = galletas,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete de Muffins de Vainilla",          Precio = 39.9, Imagen = "galletas_03",     Disponible = true, CategoriaId = galletas,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete de 4 Galletas de Avena",          Precio = 24.9, Imagen = "galletas_04",     Disponible = true, CategoriaId = galletas,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Galletas de Mantequilla Variadas",        Precio = 39.9, Imagen = "galletas_05",     Disponible = true, CategoriaId = galletas,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Galletas de sabores frutales",            Precio = 39.9, Imagen = "galletas_06",     Disponible = true, CategoriaId = galletas,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // ── Hamburguesas (8) ───────────────────────────────────────────────
            new() { Nombre = "Hamburguesa Sencilla",            Precio = 59.9, Imagen = "hamburguesas_01",         Disponible = true, CategoriaId = hamburguesas, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Hamburguesa de Pollo",            Precio = 59.9, Imagen = "hamburguesas_02",         Disponible = true, CategoriaId = hamburguesas, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Hamburguesa de Pollo y Chili",    Precio = 59.9, Imagen = "hamburguesas_03",         Disponible = true, CategoriaId = hamburguesas, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Hamburguesa Queso y Pepinos",     Precio = 59.9, Imagen = "hamburguesas_04",         Disponible = true, CategoriaId = hamburguesas, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Hamburguesa Cuarto de Libra",     Precio = 59.9, Imagen = "hamburguesas_05",         Disponible = true, CategoriaId = hamburguesas, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Hamburguesa Doble Queso",         Precio = 69.9, Imagen = "hamburguesas_06",         Disponible = true, CategoriaId = hamburguesas, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Hot Dog Especial",                Precio = 49.9, Imagen = "hamburguesas_07",         Disponible = true, CategoriaId = hamburguesas, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Paquete 2 Hot Dogs",              Precio = 69.9, Imagen = "hamburguesas_08",         Disponible = true, CategoriaId = hamburguesas, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // ── Pasteles (6) ───────────────────────────────────────────────────
            new() { Nombre = "4 Rebanadas de Pay de Queso",  Precio = 69.9, Imagen = "pastel_01",                 Disponible = true, CategoriaId = pasteles,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Waffle Especial",              Precio = 49.9, Imagen = "pastel_02",                 Disponible = true, CategoriaId = pasteles,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Croissants De la casa",        Precio = 39.9, Imagen = "pastel_03",                 Disponible = true, CategoriaId = pasteles,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pay de Queso",                 Precio = 19.9, Imagen = "pastel_04",                 Disponible = true, CategoriaId = pasteles,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pastel de Chocolate",          Precio = 29.9, Imagen = "pastel_05",                 Disponible = true, CategoriaId = pasteles,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Rebanada Pastel de Chocolate", Precio = 29.9, Imagen = "pastel_06",                 Disponible = true, CategoriaId = pasteles,     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // ── Pizzas (11) ────────────────────────────────────────────────────
            new() { Nombre = "Pizza Spicy con Doble Queso",        Precio = 69.9, Imagen = "pizzas_01",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Jamón y Queso",                Precio = 69.9, Imagen = "pizzas_02",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Doble Queso",                  Precio = 69.9, Imagen = "pizzas_03",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Especial de la Casa",          Precio = 69.9, Imagen = "pizzas_04",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Chorizo",                      Precio = 69.9, Imagen = "pizzas_05",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Hawaiana",                     Precio = 69.9, Imagen = "pizzas_06",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Tocino",                       Precio = 69.9, Imagen = "pizzas_07",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Vegetales y Queso",            Precio = 69.9, Imagen = "pizzas_08",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Pepperoni y Queso",            Precio = 69.9, Imagen = "pizzas_09",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Aceitunas y Queso",            Precio = 69.9, Imagen = "pizzas_10",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Nombre = "Pizza Queso, Jamón y Champiñones",   Precio = 69.9, Imagen = "pizzas_11",           Disponible = true, CategoriaId = pizzas,       CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        context.Productos.AddRange(productos);
        await context.SaveChangesAsync();
    }
}
