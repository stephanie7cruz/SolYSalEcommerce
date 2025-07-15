using Microsoft.EntityFrameworkCore; 
using SolYSalEcommerce.Models; 
using System; 
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SolYSalEcommerce.Data.Models;

namespace SolYSalEcommerce.Data
{
    public static class DataSeeder
    {
        public static async Task SeedProductsAsync(ApplicationDbContext context)
        {
            // Solo siembra datos si no hay productos existentes
            if (context.Products.Any())
            {
                Console.WriteLine("La base de datos ya contiene productos. No se realizará la siembra.");
                return;
            }

            // --- ¡IMPORTANTE! ACTUALIZA ESTE csvContent CON LAS NUEVAS COLUMNAS DE IMAGEN Y UNA FILA POR VARIANTE ---
            // Asumo que tienes 3 columnas de URL de imagen al final del CSV: ImageUrl1, ImageUrl2, ImageUrl3
            // Y que cada línea del CSV representa una combinación única de Referencia, Talla y Color.
            // Las URLs de ejemplo apuntan a la carpeta wwwroot/images/
            string csvContent = @"
""Referencia"",""Coleccion"",""Talla"",""Color"",""Cantidad en Stock"",""Precio Venta (COP)"",""ImageUrl1"",""ImageUrl2"",""ImageUrl3""
""Dahian"",""Bikini"",""M"",""Negro"",1,59900,""/images/dahian_negro_1.jpg"",""/images/dahian_negro_2.jpg"",""/images/dahian_negro_3.jpg""
""Dahian"",""Bikini"",""S"",""Negro"",1,59900,""/images/dahian_negro_1.jpg"",""/images/dahian_negro_2.jpg"",""/images/dahian_negro_3.jpg""
""Dahian"",""Bikini"",""L"",""Negro"",1,59900,""/images/dahian_negro_1.jpg"",""/images/dahian_negro_2.jpg"",""/images/dahian_negro_3.jpg""
""Dahian"",""Bikini"",""M"",""Blanco"",1,59900,""/images/dahian_blanco_1.jpg"",""/images/dahian_blanco_2.jpg"",""/images/dahian_blanco_3.jpg""
""Dahian"",""Bikini"",""S"",""Blanco"",1,59900,""/images/dahian_blanco_1.jpg"",""/images/dahian_blanco_2.jpg"",""/images/dahian_blanco_3.jpg""
""Dahian"",""Bikini"",""L"",""Blanco"",1,59900,""/images/dahian_blanco_1.jpg"",""/images/dahian_blanco_2.jpg"",""/images/dahian_blanco_3.jpg""
""Antonia"",""Bikini"",""M"",""Blanco"",1,60000,""/images/antonia_blanco_1.jpg"",""/images/antonia_blanco_2.jpg"",""/images/antonia_blanco_3.jpg""
""Antonia"",""Bikini"",""S"",""Blanco"",1,60000,""/images/antonia_blanco_1.jpg"",""/images/antonia_blanco_2.jpg"",""/images/antonia_blanco_3.jpg""
""Antonia"",""Bikini"",""L"",""Blanco"",1,60000,""/images/antonia_blanco_1.jpg"",""/images/antonia_blanco_2.jpg"",""/images/antonia_blanco_3.jpg""
""Lisette"",""Serena"",""M"",""Blanco"",1,69900,""/images/lisette_blanco_1.jpg"",""/images/lisette_blanco_2.jpg"",""/images/lisette_blanco_3.jpg""
""Carmen"",""Enterizo"",""M"",""Negro"",1,72500,""/images/carmen_negro_1.jpg"",""/images/carmen_negro_2.jpg"",""/images/carmen_negro_3.jpg""
""Sublime"",""Bikini"",""M"",""Negro"",1,57500,""/images/sublime_negro_1.jpg"",""/images/sublime_negro_2.jpg"",""/images/sublime_negro_3.jpg""
""Kimono andrea"",""Salidas"",""M"",""Negro"",1,55000,""/images/kimono_andrea_negro_1.jpg"",""/images/kimono_andrea_negro_2.jpg"",""/images/kimono_andrea_negro_3.jpg""
""Celeste"",""Enterizo"",""M"",""Verde"",1,72500,""/images/celeste_verde_1.jpg"",""/images/celeste_verde_2.jpg"",""/images/celeste_verde_3.jpg""
""Caribeña"",""Bikini"",""M"",""Rojo"",1,60000,""/images/caribena_rojo_1.jpg"",""/images/caribena_rojo_2.jpg"",""/images/caribena_rojo_3.jpg""
""Allie"",""Bikini"",""M"",""Blanco"",1,59900,""/images/allie_blanco_1.jpg"",""/images/allie_blanco_2.jpg"",""/images/allie_blanco_3.jpg""
""Bruna"",""Bikini Set"",""M"",""Negro"",1,65000,""/images/bruna_negro_1.jpg"",""/images/bruna_negro_2.jpg"",""/images/bruna_negro_3.jpg""
""Lia"",""Bikini"",""M"",""Azul"",1,59900,""/images/lia_azul_1.jpg"",""/images/lia_azul_2.jpg"",""/images/lia_azul_3.jpg""
""Kimono perla"",""Salidas"",""M"",""Blanco"",1,60000,""/images/kimono_perla_blanco_1.jpg"",""/images/kimono_perla_blanco_2.jpg"",""/images/kimono_perla_blanco_3.jpg""
""Kimono arena"",""Salidas"",""M"",""Blanco"",1,45500,""/images/kimono_arena_blanco_1.jpg"",""/images/kimono_arena_blanco_2.jpg"",""/images/kimono_arena_blanco_3.jpg""
""Renata"",""Serena"",""M"",""Negro"",1,69900,""/images/renata_negro_1.jpg"",""/images/renata_negro_2.jpg"",""/images/renata_negro_3.jpg""
""Emilia"",""Serena"",""M"",""Negro"",1,69900,""/images/emilia_negro_1.jpg"",""/images/emilia_negro_2.jpg"",""/images/emilia_negro_3.jpg""
""Juliette"",""Serena"",""M"",""Negro"",1,69900,""/images/juliette_negro_1.jpg"",""/images/juliette_negro_2.jpg"",""/images/juliette_negro_3.jpg""
""Lorena"",""Serena"",""M"",""Negro"",1,69900,""/images/lorena_negro_1.jpg"",""/images/lorena_negro_2.jpg"",""/images/lorena_negro_3.jpg""
""Mat"",""Bikini Set"",""M"",""cafe"",1,50000,""/images/mat_cafe_1.jpg"",""/images/mat_cafe_2.jpg"",""/images/mat_cafe_3.jpg""
""Lara"",""Bikini Set"",""M"",""Vino"",1,60950,""/images/lara_vino_1.jpg"",""/images/lara_vino_2.jpg"",""/images/lara_vino_3.jpg""
""Rosa"",""Bikini Set"",""M"",""Blanco"",1,50000,""/images/rosa_blanco_1.jpg"",""/images/rosa_blanco_2.jpg"",""/images/rosa_blanco_3.jpg""
""Pamela"",""Bikini"",""M"",""Verde"",1,59900,""/images/pamela_verde_1.jpg"",""/images/pamela_verde_2.jpg"",""/images/pamela_verde_3.jpg""
""Rana"" ,""Bikini"",""M"",""Verde"",1,59900,""/images/rana_verde_1.jpg"",""/images/rana_verde_2.jpg"",""/images/rana_verde_3.jpg""
""Amber"" ,""Bikini"",""M"",""Rojo"",1,59900,""/images/amber_rojo_1.jpg"",""/images/amber_rojo_2.jpg"",""/images/amber_rojo_3.jpg""
""Maia"" ,""Bikini"",""M"",""Azul"",1,59900,""/images/maia_azul_1.jpg"",""/images/maia_azul_2.jpg"",""/images/maia_azul_3.jpg""
""Ventosa"" ,""Bikini"",""M"",""Blanco"",1,69900,""/images/ventosa_blanco_1.jpg"",""/images/ventosa_blanco_2.jpg"",""/images/ventosa_blanco_3.jpg""
""Zoe"" ,""Bikini"",""M"",""Negro"",1,59900,""/images/zoe_negro_1.jpg"",""/images/zoe_negro_2.jpg"",""/images/zoe_negro_3.jpg""
""Flora"" ,""Bikini"",""M"",""Verde"",1,59900,""/images/flora_verde_1.jpg"",""/images/flora_verde_2.jpg"",""/images/flora_verde_3.jpg""
""Eliana"" ,""Bikini"",""M"",""Negro"",1,59900,""/images/eliana_negro_1.jpg"",""/images/eliana_negro_2.jpg"",""/images/eliana_negro_3.jpg""
""Lina"" ,""Bikini"",""M"",""Rojo"",1,59900,""/images/lina_rojo_1.jpg"",""/images/lina_rojo_2.jpg"",""/images/lina_rojo_3.jpg""
""Salida Dalia"" ,""Salidas"",""M"",""Blanco"",1,25000,""/images/salida_dalia_blanco_1.jpg"",""/images/salida_dalia_blanco_2.jpg"",""/images/salida_dalia_blanco_3.jpg""
""Salida Liora"" ,""Salidas"",""M"",""Negro"",1,30000,""/images/salida_liora_negro_1.jpg"",""/images/salida_liora_negro_2.jpg"",""/images/salida_liora_negro_3.jpg""
""Salida Dana"" ,""Salidas"",""M"",""Negro"",1,25000,""/images/salida_dana_negro_1.jpg"",""/images/salida_dana_negro_2.jpg"",""/images/salida_dana_negro_3.jpg""
""Salida Lisa"" ,""Salidas"",""M"",""Blanco"",1,25000,""/images/salida_lisa_blanco_1.jpg"",""/images/salida_lisa_blanco_2.jpg"",""/images/salida_lisa_blanco_3.jpg""
""Salida Dafne"" ,""Salidas"",""M"",""Blanco"",1,25000,""/images/salida_dafne_blanco_1.jpg"",""/images/salida_dafne_blanco_2.jpg"",""/images/salida_dafne_blanco_3.jpg""
""Salida Anna"" ,""Salidas"",""M"",""Blanco"",1,25000,""/images/salida_anna_blanco_1.jpg"",""/images/salida_anna_blanco_2.jpg"",""/images/salida_anna_blanco_3.jpg""
""Sobrer Carla"" ,""Accesorios"",""M"",""Blanco"",1,25000,""/images/sombrero_carla_blanco_1.jpg"",""/images/sombrero_carla_blanco_2.jpg"",""/images/sombrero_carla_blanco_3.jpg""
";

            var lines = csvContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                                  .Where(line => !string.IsNullOrWhiteSpace(line))
                                  .ToList();

            if (lines.Count <= 1)
            {
                Console.WriteLine("No hay líneas de datos válidas en el CSV para sembrar.");
                return;
            }

            // El patrón Regex.Split es robusto para comas dentro de comillas
            string csvSplitPattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

            // Usamos un Dictionary para almacenar los productos ya creados en esta ejecución del seeder.
            // La clave es la "Referencia" (Name del Product), y el valor es el objeto Product.
            var productCache = new Dictionary<string, Product>();

            // Iterar sobre cada línea del CSV (excepto la cabecera)
            for (int i = 1; i < lines.Count; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = Regex.Split(line, csvSplitPattern);

                // Asegurarse de que parts tenga suficientes elementos para todas las columnas esperadas (9 en este caso)
                if (parts.Length < 9)
                {
                    Console.WriteLine($"Advertencia: Línea CSV incompleta o mal formada saltada: {line}. Contenido: {line}");
                    continue;
                }

                // Limpiar y parsear cada parte
                var referencia = parts[0].Trim().Trim('"');
                var coleccion = parts[1].Trim().Trim('"');
                var tallaStr = parts[2].Trim().Trim('"');
                var color = parts[3].Trim().Trim('"');

                int cantidadStock;
                if (!int.TryParse(parts[4].Trim().Trim('"'), NumberStyles.Any, CultureInfo.InvariantCulture, out cantidadStock))
                {
                    Console.WriteLine($"Error al parsear cantidadStock para {referencia}. Valor: '{parts[4]}'. Línea: {line}");
                    continue;
                }

                decimal precioVenta;
                if (!decimal.TryParse(parts[5].Trim().Trim('"'), NumberStyles.Any, CultureInfo.InvariantCulture, out precioVenta))
                {
                    Console.WriteLine($"Error al parsear precioVenta para {referencia}. Valor: '{parts[5]}'. Línea: {line}");
                    continue;
                }

                // URLs de las imágenes
                var imageUrl1 = parts[6].Trim().Trim('"');
                var imageUrl2 = parts[7].Trim().Trim('"');
                var imageUrl3 = parts[8].Trim().Trim('"');

                // --- LÓGICA PARA CREAR/RECUPERAR PRODUCTOS Y CREAR/ACTUALIZAR VARIANTES ---
                Product product;

                // Intentar obtener el producto del caché primero
                if (productCache.TryGetValue(referencia, out product))
                {
                    // Producto ya en caché, no necesitamos buscarlo en la DB de nuevo
                }
                else
                {
                    // Si no está en caché, buscar en la DB
                    product = await context.Products
                                   .Include(p => p.Variants) // Incluir variantes para comprobar existentes
                                   .Include(p => p.Images)   // Incluir imágenes para comprobar existentes
                                   .FirstOrDefaultAsync(p => p.Name == referencia && p.Description == coleccion);

                    if (product == null)
                    {
                        // Si no existe en la DB, crearlo
                        product = new Product
                        {
                            Name = referencia,
                            Description = coleccion, // La colección es ahora la descripción del producto
                            BasePrice = precioVenta, // El precio base puede ser el de la primera variante encontrada
                            IsActive = true
                        };
                        context.Products.Add(product);
                        // No SaveChangesAsync aquí, se hará al final de todo el seeder
                    }
                    productCache.Add(referencia, product); // Añadir al caché
                }

                // Crear o actualizar la ProductVariant para esta combinación específica
                string size = tallaStr;
                string sku = $"{referencia.Replace(" ", "").ToUpper()}-{color.Replace(" ", "").ToUpper()}-{size.ToUpper()}";

                var existingVariant = product.Variants.FirstOrDefault(pv => pv.Size.ToUpper() == size.ToUpper() && pv.Color.ToLower() == color.ToLower());

                ProductVariant currentVariant; // Variable para la variante actual que estamos procesando

                if (existingVariant == null)
                {
                    // Si la variante no existe, la creamos
                    currentVariant = new ProductVariant
                    {
                        ProductId = product.Id, // Se asignará el ID después del SaveChanges si el producto es nuevo
                        Color = color,
                        Size = size,
                        SKU = sku,
                        Stock = cantidadStock,
                        ImageUrl = imageUrl1 // La primera imagen como URL principal de la variante
                    };
                    context.ProductVariants.Add(currentVariant);
                }
                else
                {
                    // Si la variante ya existe, la actualizamos
                    existingVariant.Stock += cantidadStock; // Sumar stock si hay duplicados
                    existingVariant.ImageUrl = imageUrl1; // Actualizar la imagen principal de la variante
                    context.ProductVariants.Update(existingVariant);
                    currentVariant = existingVariant; // Usar la variante existente
                }

                // --- Lógica para añadir las 3 imágenes adicionales a la tabla ProductImages ---
                // Para asegurarnos de que la variante tenga un Id antes de asociar imágenes,
                // vamos a hacer un SaveChangesAsync aquí, justo antes de añadir las ProductImages.
                // Esto es necesario si el 'product' y 'currentVariant' son completamente nuevos en esta iteración.
                // Si el producto y/o la variante ya existen, este SaveChangesAsync no hará mucho, pero si son nuevos,
                // les asignará sus respectivos IDs de base de datos.
                await context.SaveChangesAsync();

                // Asegurar que currentVariant tenga un Id si es recién creado
                if (currentVariant.Id == Guid.Empty && context.Entry(currentVariant).State == EntityState.Added)
                {
                    // Si el Id aún está vacío y está marcado como Added, intentamos obtenerlo después de guardar
                    // Esto es un escenario poco común si SaveChangesAsync() se llama correctamente.
                    // Pero para mayor robustez, si el ID no se generó (ej. por algún error en EF o auto-generación),
                    // podríamos tener un problema aquí. Normalmente, EF asigna el GUID al añadir.
                    // currentVariant.Id = Guid.NewGuid(); // Fallback si no hay auto-generación, pero Guid.NewGuid() ya está en el modelo.
                    // Re-fetch the entity if necessary to get the Id populated by EF
                    // currentVariant = await context.ProductVariants.FirstOrDefaultAsync(pv => pv.SKU == sku);
                    // O simplemente confiamos en que el Id ya está asignado si es Guid.NewGuid()
                }


                // Eliminar imágenes antiguas asociadas a esta VARIANTE (para evitar duplicados al re-sembrar)
                var oldVariantImages = await context.ProductImages
                                                  .Where(pi => pi.ProductVariantId == currentVariant.Id)
                                                  .ToListAsync();
                context.ProductImages.RemoveRange(oldVariantImages);

                // Añadir las nuevas imágenes para esta variante
                var imageUrls = new List<string> { imageUrl1, imageUrl2, imageUrl3 };
                for (int j = 0; j < imageUrls.Count; j++)
                {
                    if (!string.IsNullOrWhiteSpace(imageUrls[j]))
                    {
                        context.ProductImages.Add(new ProductImage
                        {
                            ProductId = product.Id, // Se asocia al producto principal
                            ProductVariantId = currentVariant.Id, // ¡Importante! Se asocia a la variante específica
                            ImageUrl = imageUrls[j],
                            Order = j + 1
                        });
                    }
                }
            }

            // Guarda todos los cambios pendientes de Productos, Variantes e Imágenes que no se guardaron en el bucle
            await context.SaveChangesAsync();
            Console.WriteLine("Datos sembrados exitosamente.");
        }
    }
}