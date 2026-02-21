using Dungeon.Personajes;

namespace Dungeon.Combate
{
    /// <summary>
    /// Clase que gestiona el inventario del personaje
    /// </summary>
    public static class Inventario
    {
        /// <summary>
        /// Permite al personaje usar una poción de su inventario
        /// Aplica los efectos (vida y/o mana)
        /// </summary>
        /// <param name="personaje">Personaje que usa la poción</param>
        public static void UsarPocion(Personaje personaje)
        {
            // Verificar si el personaje tiene pociones
            if (!personaje.InventarioPociones.Any())
            {
                Console.WriteLine("\n❌ No tienes pociones.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n🧪 POCIONES:");
            
            // Mostrar lista de pociones disponibles
            var menuPociones = personaje.InventarioPociones
                .Select((p, index) => $"{index + 1}. {p.Nombre} - {p.Descripcion}")
                .Aggregate((a, b) => a + "\n" + b);
            
            Console.WriteLine(menuPociones);
            Console.Write("\nElige poción (0 para cancelar): ");

            // Leer selección del usuario
            if (int.TryParse(Console.ReadLine(), out int indice) && 
                indice >= 1 && indice <= personaje.InventarioPociones.Count)
            {
                var pocion = personaje.InventarioPociones[indice - 1];
                
                // Aplicar efectos de la poción
                var efectosAplicados = pocion.Efectos
                    .Select(efecto =>
                    {
                        // Efecto de vida
                        if (efecto.Key == "vida")
                        {
                            int vidaMax = personaje.Clase.Atributos["vida"];
                            int vidaAntes = personaje.VidaActual;
                            personaje.VidaActual = Math.Min(personaje.VidaActual + efecto.Value, vidaMax);
                            int vidaCurada = personaje.VidaActual - vidaAntes;
                            return $"  ❤️ +{vidaCurada} Vida";
                        }
                        // Efecto de mana
                        else if (efecto.Key == "mana")
                        {
                            int manaMax = personaje.Clase.Atributos["mana"];
                            int manaAntes = personaje.ManaActual;
                            personaje.ManaActual = Math.Min(personaje.ManaActual + efecto.Value, manaMax);
                            int manaCurado = personaje.ManaActual - manaAntes;
                            return $"  ✨ +{manaCurado} Mana";
                        }
                        return $"  +{efecto.Value} {efecto.Key}";
                    })
                    .Aggregate((a, b) => a + "\n" + b);

                Console.WriteLine($"\n✅ Usaste {pocion.Nombre}:");
                Console.WriteLine(efectosAplicados);

                // Eliminar poción del inventario
                personaje.InventarioPociones = personaje.InventarioPociones
                    .Where((p, i) => i != indice - 1)
                    .ToList();

                Console.ReadKey();
            }
        }
        
        /// <summary>
        /// Muestra el inventario completo del personaje
        /// Armas, armaduras, pociones y trinkets organizados
        /// </summary>
        /// <param name="personaje">Personaje cuyo inventario se mostrará</param>
        public static void MostrarInventario(Personaje personaje)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════╗");
            Console.WriteLine("║           📦 INVENTARIO               ║");
            Console.WriteLine("╚═══════════════════════════════════════╝\n");

            // SECCIÓN DE ARMAS
            if (personaje.InventarioArmas.Any())
            {
                Console.WriteLine("🗡️  ARMAS:");
                var armasInfo = personaje.InventarioArmas
                    .Select(a => $"  • {a.Nombre} (Daño: {a.Daño}, {a.Rareza})")
                    .Aggregate((a, b) => a + "\n" + b);
                Console.WriteLine(armasInfo);
            }

            // SECCIÓN DE ARMADURAS
            if (personaje.InventarioArmaduras.Any())
            {
                Console.WriteLine("\n🛡️  ARMADURAS:");
                var armadurasInfo = personaje.InventarioArmaduras
                    .Select(a => $"  • {a.Nombre} (Defensa: {a.Defensa}, {a.Rareza})")
                    .Aggregate((a, b) => a + "\n" + b);
                Console.WriteLine(armadurasInfo);
            }

            // SECCIÓN DE POCIONES
            if (personaje.InventarioPociones.Any())
            {
                Console.WriteLine("\n🧪 POCIONES:");
                var pocionesInfo = personaje.InventarioPociones
                    .Select(p => $"  • {p.Nombre} ({p.Rareza})")
                    .Aggregate((a, b) => a + "\n" + b);
                Console.WriteLine(pocionesInfo);
            }

            // SECCIÓN DE TRINKETS
            if (personaje.InventarioTrinkets.Any())
            {
                Console.WriteLine("\n💍 TRINKETS:");
                var trinketsInfo = personaje.InventarioTrinkets
                    .Select(t => $"  • {t.Nombre}")
                    .Aggregate((a, b) => a + "\n" + b);
                Console.WriteLine(trinketsInfo);
            }

            // DESTACAR MEJOR ARMA
            var mejorArma = personaje.InventarioArmas.MaxBy(a => a.Daño);
            if (mejorArma != null)
            {
                Console.WriteLine($"\n⭐ Mejor arma: {mejorArma.Nombre} (Daño: {mejorArma.Daño})");
            }

            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}