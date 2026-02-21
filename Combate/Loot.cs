using Dungeon.Personajes;
using Dungeon.Salas;

namespace Dungeon.Combate
{
    /// <summary>
    /// Sistema que gestiona el loot (recompensas) de enemigos derrotados
    /// </summary>
    public static class Loot
    {
        private static Random rnd = new Random();
        
        /// <summary>
        /// Genera las recompensas al derrotar un enemigo
        /// Los jefes dan mejor loot que enemigos normales
        /// </summary>
        /// <param name="personaje">Personaje que recibe el loot</param>
        /// <param name="sala">Sala donde ocurre el loot</param>
        /// <param name="enemigoDerrotado">Enemigo derrotado que da el loot</param>
        public static void GenerarLoot(Personaje personaje, Sala sala, Enemigo enemigoDerrotado)
        {
            Console.WriteLine("\n💰 LOOT:");
            
            // Filtrar objetos según rareza (jefes obtienen mejor loot)
            var objetosDisponibles = sala.Objetos
                .Where(obj => 
                {
                    string rareza = ObtenerRareza(obj);
                    
                    // Los jefes dan loot épico o mejor
                    if (enemigoDerrotado.Tipo == "Jefe")
                        return rareza == "Épica" || rareza == "Legendaria" || rareza == "Mítica";
                    
                    // Enemigos normales dan loot común o mejor
                    return rareza == "Común" || rareza == "Poco Común" || rareza == "Rara";
                })
                .OrderBy(_ => rnd.Next())
                .Take(rnd.Next(1, 3))
                .ToList();
            
            // Verificar si hay objetos disponibles
            if (!objetosDisponibles.Any())
            {
                Console.WriteLine("  No se encontró nada de valor.");
                return;
            }
            
            // Añadir objetos al inventario y mostrar
            var lootTexto = objetosDisponibles
                .Select(obj => AñadirObjetoAInventario(obj, personaje))
                .Aggregate((a, b) => a + "\n" + b);
            
            Console.WriteLine(lootTexto);
        }

        /// <summary>
        /// Añade un objeto al inventario del personaje según su tipo
        /// </summary>
        /// <param name="obj">Objeto a añadir</param>
        /// <param name="personaje">Personaje que recibe el objeto</param>
        /// <returns>Texto descriptivo del objeto añadido</returns>
        private static string AñadirObjetoAInventario(object obj, Personaje personaje)
        {
            switch (obj)
            {
                case Arma arma:
                    personaje.InventarioArmas.Add(arma);
                    return $"  🗡️ {arma.Nombre} ({arma.Rareza})";
                
                case Armadura armadura:
                    personaje.InventarioArmaduras.Add(armadura);
                    return $"  🛡️ {armadura.Nombre} ({armadura.Rareza})";
                
                case Pocion pocion:
                    personaje.InventarioPociones.Add(pocion);
                    return $"  🧪 {pocion.Nombre} ({pocion.Rareza})";
                
                case Trinket trinket:
                    personaje.InventarioTrinkets.Add(trinket);
                    return $"  💍 {trinket.Nombre}";
                
                default:
                    return "  ❓ Objeto desconocido";
            }
        }

        /// <summary>
        /// Obtiene la rareza de un objeto según su tipo
        /// </summary>
        /// <param name="obj">Objeto a evaluar</param>
        /// <returns>Rareza del objeto o "Común" por defecto</returns>
        private static string ObtenerRareza(object obj)
        {
            return obj switch
            {
                Arma arma => arma.Rareza,
                Armadura armadura => armadura.Rareza,
                Pocion pocion => pocion.Rareza,
                _ => "Común"
            };
        }
    }
}