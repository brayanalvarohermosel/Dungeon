using Dungeon.Personajes;
using Dungeon.Salas;

namespace Dungeon.Eventos
{
    /// <summary>
    /// Sistema que gestiona eventos especiales dentro del dungeon
    /// </summary>
    public static class SistemaEventos
    {
        private static Random rnd = new Random();
        
        /// <summary>
        /// Genera un evento aleatorio basado en probabilidades
        /// </summary>
        /// <param name="todosEventos">Lista de todos los eventos disponibles</param>
        /// <param name="sala">Sala donde ocurre el evento</param>
        /// <returns>Evento seleccionado o null si no hay evento</returns>
        public static Evento? GenerarEvento(List<Evento> todosEventos, Sala sala)
        {
            if (!todosEventos.Any())
                return null;

            // La probabilidad base es más alta en salas jefe
            int probabilidadBase = sala.SalaBoss ? 80 : 60;

            // Seleccionar un evento basado en probabilidades
            var evento = todosEventos
                .Where(e => rnd.Next(0, 100) < (e.Probabilidad + probabilidadBase - 30))
                .OrderBy(_ => rnd.Next())
                .FirstOrDefault();

            return evento;
        }

        /// <summary>
        /// Procesa un evento aplicando todos sus efectos
        /// </summary>
        /// <param name="evento">Evento a procesar</param>
        /// <param name="personaje">Personaje que recibe los efectos</param>
        /// <param name="sala">Sala donde ocurre el evento</param>
        public static void ProcesarEvento(Evento evento, Personaje personaje, Sala sala)
        {
            Console.WriteLine($"\n⚡ EVENTO: {evento.Tipo}");
            Console.WriteLine($"   {evento.Descripcion}");

            // Aplicar todos los efectos del evento
            if (evento.Efectos.Any())
            {
                var efectosTexto = evento.Efectos
                    .Select(efecto => AplicarEfecto(efecto, personaje, sala))
                    .Where(texto => !string.IsNullOrEmpty(texto))
                    .ToList();

                if (efectosTexto.Any())
                {
                    var textoFinal = efectosTexto.Aggregate((a, b) => a + "\n" + b);
                    Console.WriteLine(textoFinal);
                }
            }

            Console.WriteLine();
        }
        
        /// <summary>
        /// Aplica un efecto específico al personaje
        /// Maneja daño, vida, mana, tesoro, bendiciones, etc.
        /// </summary>
        /// <param name="efecto">Par clave-valor con tipo y cantidad del efecto</param>
        /// <param name="personaje">Personaje que recibe el efecto</param>
        /// <param name="sala">Sala para obtener objetos</param>
        /// <returns>Texto descriptivo del efecto aplicado</returns>
        private static string AplicarEfecto(KeyValuePair<string, int> efecto, Personaje personaje, Sala sala)
        {
            switch (efecto.Key)
            {
                case "daño":
                    // Recibir daño
                    int dañoRecibido = Math.Abs(efecto.Value);
                    personaje.VidaActual = Math.Max(personaje.VidaActual - dañoRecibido, 0);
                    return $"   💔 Recibes {dañoRecibido} de daño";

                case "vida":
                    // Recuperar vida (máximo hasta la vida máxima)
                    int vidaMax = personaje.Clase.Atributos["vida"];
                    int vidaAntes = personaje.VidaActual;
                    personaje.VidaActual = Math.Min(personaje.VidaActual + efecto.Value, vidaMax);
                    int vidaCurada = personaje.VidaActual - vidaAntes;
                    return vidaCurada > 0 ? $"   💚 Recuperas {vidaCurada} puntos de vida" : "";

                case "mana":
                    // Recuperar mana (máximo hasta el mana máximo)
                    int manaMax = personaje.Clase.Atributos["mana"];
                    int manaAntes = personaje.ManaActual;
                    personaje.ManaActual = Math.Min(personaje.ManaActual + efecto.Value, manaMax);
                    int manaCurado = personaje.ManaActual - manaAntes;
                    return manaCurado > 0 ? $"   ✨ Recuperas {manaCurado} puntos de mana" : "";

                case "tesoro":
                    // Obtener un objeto raro de la sala
                    var objetoRaro = sala.Objetos
                        .Where(obj =>
                        {
                            string rareza = ObtenerRareza(obj);
                            return rareza == "Rara" || rareza == "Épica" || rareza == "Legendaria";
                        })
                        .OrderBy(_ => rnd.Next())
                        .FirstOrDefault();

                    if (objetoRaro != null)
                    {
                        string nombre = AñadirObjetoAInventario(objetoRaro, personaje);
                        return $"   💎 ¡Obtienes: {nombre}!";
                    }
                    return "   💎 El cofre estaba vacío";

                case "bendicion":
                    // Bendición (aumentar próximo ataque)
                    return $"   ✨ ¡Te sientes más fuerte! (Próximo ataque +50%)";

                case "mercader":
                    // Encuentro con mercader
                    return $"   🏪 El mercader no tiene nada que te interese hoy";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Obtiene la rareza de un objeto según su tipo
        /// </summary>
        /// <param name="obj">Objeto a evaluar</param>
        /// <returns>Texto de rareza o "Común" por defecto</returns>
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
        /// Añade un objeto al inventario del personaje según su tipo
        /// </summary>
        /// <param name="obj">Objeto a añadir</param>
        /// <param name="personaje">Personaje que recibe el objeto</param>
        /// <returns>Nombre del objeto añadido</returns>
        private static string AñadirObjetoAInventario(object obj, Personaje personaje)
        {
            switch (obj)
            {
                case Arma arma:
                    personaje.InventarioArmas.Add(arma);
                    return $"{arma.Nombre} ({arma.Rareza})";

                case Armadura armadura:
                    personaje.InventarioArmaduras.Add(armadura);
                    return $"{armadura.Nombre} ({armadura.Rareza})";

                case Pocion pocion:
                    personaje.InventarioPociones.Add(pocion);
                    return $"{pocion.Nombre} ({pocion.Rareza})";

                case Trinket trinket:
                    personaje.InventarioTrinkets.Add(trinket);
                    return trinket.Nombre;

                default:
                    return "Objeto desconocido";
            }
        }
    }
}