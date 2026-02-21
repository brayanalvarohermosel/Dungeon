using Dungeon.Salas;
using Dungeon.Personajes;

namespace Dungeon.FuncionesSalas
{
    /// <summary>
    /// Clase con funciones para gestionar la presentación de salas
    /// </summary>
    public static class FuncionesSalas
    {
        /// <summary>
        /// Muestra la información completa de una sala al entrar
        /// Incluye enemigos, objetos y estado del personaje
        /// </summary>
        /// <param name="sala">Sala a mostrar</param>
        /// <param name="personaje">Personaje del jugador</param>
        public static void EntrarSala(Sala sala, Personaje personaje)
        {
            Console.Clear();
            
            string tipoSala = sala.SalaBoss ? "⚔️ SALA DEL JEFE ⚔️" : "Sala Normal";
            
            // ENCABEZADO DE SALA
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine($"║  🏰 PISO {sala.Piso} - PLANTA {sala.Planta}                       ║");
            Console.WriteLine($"║  {tipoSala,-44} ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝\n");

            // MOSTRAR ENEMIGOS SI LOS HAY
            if (sala.Enemigos.Any())
            {
                // Ordenar enemigos por vida descendente (efectivos primero)
                var enemigosOrdenados = sala.Enemigos
                    .OrderByDescending(e => e.Vida)
                    .ToList();
                
                Console.WriteLine("🗡️  ENEMIGOS:");
                var textoEnemigos = enemigosOrdenados
                    .Select((e, index) => 
                    {
                        string tipoIcono = e.Tipo == "Jefe" ? "👑" : "⚔️";
                        return $"  {index + 1}. {tipoIcono} {e.Nombre,-20} | ❤️ {e.Vida,-3} | ⚔️ {e.Ataque,-2} | 🛡️ {e.Defensa,-2}";
                    })
                    .Aggregate((a, b) => a + "\n" + b);
                
                Console.WriteLine(textoEnemigos);
                
                // Mostrar estadísticas de enemigos por tipo
                Console.WriteLine("\n📊 COMPOSICIÓN:");
                var enemigosPorTipo = sala.Enemigos
                    .GroupBy(e => e.Tipo)
                    .Select(g => $"  • {g.Key}: {g.Count()}")
                    .Aggregate((a, b) => a + "\n" + b);
                
                Console.WriteLine(enemigosPorTipo);
            }

            // MOSTRAR OBJETOS SI LOS HAY
            if (sala.Objetos.Any())
            {
                Console.WriteLine("\n💎 OBJETOS EN LA SALA:");
                
                // Separar objetos con rareza y ordenar por rareza descendente
                var objetosConRareza = sala.Objetos
                    .Select(obj => new
                    {
                        Objeto = obj,
                        Nombre = ObtenerNombre(obj),
                        Rareza = ObtenerRareza(obj),
                        RarezaNumerica = ConvertirRarezaANumero(ObtenerRareza(obj))
                    })
                    .OrderByDescending(o => o.RarezaNumerica)
                    .ToList();

                var infoObjetos = objetosConRareza
                    .Select((o, index) => $"  {index + 1}. [{o.Rareza}] {o.Nombre}")
                    .Aggregate((a, b) => a + "\n" + b);
                
                Console.WriteLine(infoObjetos);
            }

            // MOSTRAR ESTADO ACTUAL DEL PERSONAJE
            Console.WriteLine($"\n{'─',50}");
            Console.WriteLine($"❤️  Vida: {personaje.VidaActual}/{personaje.Clase.Atributos["vida"]}");
            Console.WriteLine($"✨ Mana: {personaje.ManaActual}/{personaje.Clase.Atributos["mana"]}");
            Console.WriteLine($"{'─',50}");
        }

        /// <summary>
        /// Obtiene el nombre de un objeto según su tipo
        /// </summary>
        /// <param name="obj">Objeto del cual obtener el nombre</param>
        /// <returns>Nombre del objeto o "Objeto desconocido"</returns>
        private static string ObtenerNombre(object obj)
        {
            return obj switch
            {
                Arma arma => arma.Nombre,
                Armadura armadura => armadura.Nombre,
                Pocion pocion => pocion.Nombre,
                Trinket trinket => trinket.Nombre,
                _ => "Objeto desconocido"
            };
        }

        /// <summary>
        /// Obtiene la rareza de un objeto según su tipo
        /// </summary>
        /// <param name="obj">Objeto del cual obtener la rareza</param>
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

        /// <summary>
        /// Convierte un texto de rareza a un número para ordenamiento
        /// (6 = Mítica, 5 = Legendaria, etc.)
        /// </summary>
        /// <param name="rareza">Texto de rareza</param>
        /// <returns>Número correspondiente a la rareza</returns>
        private static int ConvertirRarezaANumero(string rareza)
        {
            return rareza switch
            {
                "Mítica" => 6,
                "Legendaria" => 5,
                "Épica" => 4,
                "Rara" => 3,
                "Poco Común" => 2,
                "Común" => 1,
                _ => 0
            };
        }
    }
}