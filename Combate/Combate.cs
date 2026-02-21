using Dungeon.Personajes;
using Dungeon.Salas;
using Dungeon.Eventos;

namespace Dungeon.Combate
{
    /// <summary>
    /// Clase que gestiona el sistema de combate del juego
    /// </summary>
    public static class Combate
    {
        private static Random rnd = new Random();

        /// <summary>
        /// Permite al personaje seleccionar una acción de combate
        /// Muestra ataques melee y habilidades disponibles
        /// </summary>
        /// <param name="personaje">Personaje que realiza la acción</param>
        /// <returns>Tupla con tipo de acción y habilidad si aplica</returns>
        public static (string accion, Habilidad? habilidad) SeleccionarAccion(Personaje personaje)
        {
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine("║         ¿QUÉ QUIERES HACER?         ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine($"❤️  Vida: {personaje.VidaActual}/{personaje.Clase.Atributos["vida"]}");
            Console.WriteLine($"✨ Mana: {personaje.ManaActual}/{personaje.Clase.Atributos["mana"]}\n");
            
            // OPCIÓN 1: Ataque cuerpo a cuerpo
            Console.WriteLine("1. ⚔️  Ataque cuerpo a cuerpo (sin coste de mana)");
            
            // Separar habilidades por disponibilidad de mana
            var habilidadesUsables = personaje.HabilidadesDisponibles
                .Where(h => h.CosteMana <= personaje.ManaActual)
                .ToList();
            
            var habilidadesBloqueadas = personaje.HabilidadesDisponibles
                .Where(h => h.CosteMana > personaje.ManaActual)
                .ToList();
            
            // Mostrar habilidades usables
            if (habilidadesUsables.Any())
            {
                var menuHabilidades = habilidadesUsables
                    .Select((h, index) => 
                        $"{index + 2}. ✨ {h.Nombre} ({h.CosteMana} mana) - {h.Descripcion}")
                    .Aggregate((a, b) => a + "\n" + b);
                
                Console.WriteLine(menuHabilidades);
            }
            
            // Mostrar habilidades bloqueadas por falta de mana
            if (habilidadesBloqueadas.Any())
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                
                var menuBloqueadas = habilidadesBloqueadas
                    .Select(h => $"   🚫 {h.Nombre} (Necesita {h.CosteMana} mana)")
                    .Aggregate((a, b) => a + "\n" + b);
                
                Console.WriteLine(menuBloqueadas);
                Console.ResetColor();
            }
            
            // Leer selección del usuario
            Console.Write("\n👉 Elige opción: ");
            if (!int.TryParse(Console.ReadLine(), out int seleccion))
                return ("melee", null);
            
            // Ataque melee por defecto
            if (seleccion == 1)
                return ("melee", null);
            
            // Verificar selección de habilidad
            int indiceHabilidad = seleccion - 2;
            if (indiceHabilidad >= 0 && indiceHabilidad < habilidadesUsables.Count)
            {
                return ("habilidad", habilidadesUsables[indiceHabilidad]);
            }
            
            return ("melee", null);
        }

        /// <summary>
        /// Usa una habilidad especial en combate
        /// Aplica efectos (daño, curación, mana) según la habilidad
        /// </summary>
        /// <param name="personaje">Personaje que usa la habilidad</param>
        /// <param name="habilidad">Habilidad a usar</param>
        /// <param name="enemigo">Enemigo objetivo</param>
        /// <returns>Daño causado por la habilidad</returns>
        public static int UsarHabilidad(Personaje personaje, Habilidad habilidad, Enemigo enemigo)
        {
            // Restar costo de mana
            personaje.ManaActual -= habilidad.CosteMana;
            Console.WriteLine($"\n✨ ¡Usas {habilidad.Nombre}!");
            
            // Aplicar efecto de curación si existe
            if (habilidad.EfectosExtra.ContainsKey("curacion"))
            {
                int curacion = habilidad.EfectosExtra["curacion"];
                int vidaMax = personaje.Clase.Atributos["vida"];
                int vidaAntes = personaje.VidaActual;
                personaje.VidaActual = Math.Min(personaje.VidaActual + curacion, vidaMax);
                int vidaCurada = personaje.VidaActual - vidaAntes;
                Console.WriteLine($"💚 Recuperas {vidaCurada} puntos de vida");
            }
            
            // Aplicar efecto de recuperación de mana si existe
            if (habilidad.EfectosExtra.ContainsKey("mana"))
            {
                int manaCurado = habilidad.EfectosExtra["mana"];
                int manaMax = personaje.Clase.Atributos["mana"];
                personaje.ManaActual = Math.Min(personaje.ManaActual + manaCurado, manaMax);
                Console.WriteLine($"✨ Recuperas {manaCurado} puntos de mana");
            }
            
            // Calcular daño con multiplicador de la habilidad
            if (habilidad.MultiplicadorDaño > 0)
            {
                int dañoBase = CalculoDaño.CalcularDañoBase(personaje);
                return (int)(dañoBase * habilidad.MultiplicadorDaño);
            }
            
            return 0;
        }
        
        /// <summary>
        /// Realiza un combate turno a turno entre personaje y enemigo
        /// Gestiona daño, críticos, y fin de combate
        /// </summary>
        /// <param name="personaje">Personaje del jugador</param>
        /// <param name="enemigo">Enemigo a combatir</param>
        /// <param name="sala">Sala donde ocurre el combate</param>
        /// <returns>true si el personaje gana, false si pierde</returns>
        public static bool Combatir(Personaje personaje, Enemigo enemigo, Sala sala)
        {
            Console.Clear();
            Console.WriteLine($"╔═══════════════════════════════════════╗");
            Console.WriteLine($"║        ⚔️  COMBATE  ⚔️                ║");
            Console.WriteLine($"╚═══════════════════════════════════════╝\n");
            Console.WriteLine($"¡Te enfrentas a {enemigo.Nombre}!");
            Console.WriteLine($"Nivel: {enemigo.Nivel} | Tipo: {enemigo.Tipo}\n");

            int vidaEnemigo = enemigo.Vida;
            int turno = 1;

            // BUCLE DE COMBATE - Continúa mientras ambos tengan vida
            while (personaje.VidaActual > 0 && vidaEnemigo > 0)
            {
                Console.WriteLine($"\n{'─',45}");
                Console.WriteLine($"TURNO {turno}");
                Console.WriteLine($"{'─',45}");
                Console.WriteLine($"🐉 {enemigo.Nombre}: {vidaEnemigo}/{enemigo.Vida} HP");
                
                // TURNO DEL PERSONAJE - Seleccionar y ejecutar acción
                var (accion, habilidad) = SeleccionarAccion(personaje);
                
                int dañoJugador = 0;
                
                // Ejecutar acción seleccionada
                if (accion == "melee")
                {
                    // Ataque melee con posibilidad de crítico
                    int dañoBase = CalculoDaño.CalcularDañoBase(personaje);
                    var (esCritico, multiplicador) = CalculoDaño.CalcularCritico(personaje, rnd);
                    
                    dañoJugador = (int)(dañoBase * multiplicador);
                    
                    if (esCritico)
                        Console.WriteLine($"\n💥 ¡GOLPE CRÍTICO! 💥");
                    
                    Console.WriteLine($"⚔️ Atacas con tu arma");
                }
                else if (habilidad != null)
                {
                    // Usar habilidad especial
                    dañoJugador = UsarHabilidad(personaje, habilidad, enemigo);
                }
                
                // Aplicar daño al enemigo, considerando su defensa
                if (dañoJugador > 0)
                {
                    int dañoReal = Math.Max(dañoJugador - enemigo.Defensa, 1);
                    vidaEnemigo -= dañoReal;
                    personaje.DañoTotalInfligido += dañoReal;
                    
                    Console.WriteLine($"💥 Infliges {dañoReal} de daño!");
                    Console.WriteLine($"   {enemigo.Nombre}: {Math.Max(vidaEnemigo, 0)}/{enemigo.Vida} HP");
                }

                // Verificar si el enemigo fue derrotado
                if (vidaEnemigo <= 0)
                {
                    Console.WriteLine($"\n🎉 ¡Has derrotado a {enemigo.Nombre}!");
                    personaje.EnemigosDerrotados.Add(enemigo);
                    // Generar recompensas (loot) del enemigo derrotado
                    Loot.GenerarLoot(personaje, sala, enemigo);
                    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    Console.ReadKey();
                    return true;
                }

                // TURNO DEL ENEMIGO
                Console.WriteLine($"\n🗡️ {enemigo.Nombre} contraataca...");
                System.Threading.Thread.Sleep(800);
                
                // Calcular daño del enemigo considerando defensa del personaje
                int dañoEnemigo = enemigo.Ataque;
                int defensaJugador = CalculoDaño.CalcularDefensaJugador(personaje);
                int dañoRecibido = Math.Max(dañoEnemigo - defensaJugador, 1);
                personaje.VidaActual -= dañoRecibido;

                Console.WriteLine($"💔 Recibes {dañoRecibido} de daño!");
                Console.WriteLine($"   Tu vida: {Math.Max(personaje.VidaActual, 0)}/{personaje.Clase.Atributos["vida"]} HP");

                // Verificar si el personaje fue derrotado
                if (personaje.VidaActual <= 0)
                {
                    Console.WriteLine($"\n💀 Has sido derrotado por {enemigo.Nombre}...");
                    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    Console.ReadKey();
                    return false;
                }

                // Recuperación pasiva de mana cada turno
                int manaMax = personaje.Clase.Atributos["mana"];
                int manaRecuperado = 3;
                
                if (personaje.ManaActual < manaMax)
                {
                    int manaAntes = personaje.ManaActual;
                    personaje.ManaActual = Math.Min(personaje.ManaActual + manaRecuperado, manaMax);
                    int manaReal = personaje.ManaActual - manaAntes;
                    
                    if (manaReal > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"✨ Recuperas {manaReal} de mana ({personaje.ManaActual}/{manaMax})");
                        Console.ResetColor();
                    }
                }

                turno++;
                System.Threading.Thread.Sleep(1000);
            }

            return false;
        }
        
        /// <summary>
        /// Menú principal de combate dentro de una sala
        /// Gestiona eventos, selección de enemigos y acciones disponibles
        /// </summary>
        /// <param name="personaje">Personaje del jugador</param>
        /// <param name="sala">Sala actual donde ocurre el combate</param>
        /// <param name="eventos">Lista de eventos disponibles</param>
        public static void MenuCombate(Personaje personaje, Sala sala, List<Evento> eventos)
        {
            // Generar evento especial aleatorio al entrar en la sala
            if (eventos != null && eventos.Any())
            {
                var evento = SistemaEventos.GenerarEvento(eventos, sala);
                if (evento != null)
                {
                    // Procesar evento si fue generado
                    SistemaEventos.ProcesarEvento(evento, personaje, sala);
                    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }

            // BUCLE DE COMBATE - Continúa mientras haya enemigos en la sala
            while (sala.Enemigos.Any() && personaje.VidaActual > 0)
            {
                // Mostrar información de la sala
                FuncionesSalas.FuncionesSalas.EntrarSala(sala, personaje);

                // MENÚ DE ACCIONES
                Console.WriteLine("\n¿Qué quieres hacer?");
                Console.WriteLine("1. Atacar enemigo");
                Console.WriteLine("2. Usar poción");
                Console.WriteLine("3. Ver inventario");
                Console.WriteLine("4. Huir de la sala");
                Console.Write("\nElige opción: ");

                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        // Atacar - Seleccionar enemigo
                        var enemigosVivos = sala.Enemigos.ToList();
                        
                        Console.WriteLine("\n¿A quién atacas?");
                        if (enemigosVivos.Any())
                        {
                            // Mostrar lista de enemigos disponibles
                            var menuEnemigos = enemigosVivos
                                .Select((e, index) => $"{index + 1}. {e.Nombre} (❤️ {e.Vida})")
                                .Aggregate((a, b) => a + "\n" + b);
                            Console.WriteLine(menuEnemigos);
                            Console.Write("\nElige enemigo: ");
                        }

                        // Leer selección y ejecutar combate
                        if (int.TryParse(Console.ReadLine(), out int indiceEnemigo) && 
                            indiceEnemigo >= 1 && indiceEnemigo <= enemigosVivos.Count)
                        {
                            var enemigoSeleccionado = enemigosVivos[indiceEnemigo - 1];
                            bool victoria = Combatir(personaje, enemigoSeleccionado, sala);
                            
                            if (victoria)
                            {
                                // Si ganó, eliminar el enemigo de la sala
                                sala.Enemigos = sala.Enemigos
                                    .Where(e => e != enemigoSeleccionado)
                                    .ToList();
                            }
                            else
                            {
                                // Si perdió, salir del combate
                                return;
                            }
                        }
                        break;

                    case "2":
                        // Usar poción de curación
                        Inventario.UsarPocion(personaje);
                        break;

                    case "3":
                        // Ver inventario del personaje
                        Inventario.MostrarInventario(personaje);
                        break;

                    case "4":
                        // Huir de la sala (sin eliminar enemigos)
                        Console.WriteLine("¡Huyes de la sala!");
                        return;

                    default:
                        Console.WriteLine("Opción no válida.");
                        Console.ReadKey();
                        break;
                }
            }

            // SALA LIMPIA - Se eliminaron todos los enemigos
            if (!sala.Enemigos.Any() && personaje.VidaActual > 0)
            {
                Console.WriteLine("\n✅ ¡Has limpiado la sala de enemigos!");
                personaje.SalasCompletadas++;  // Incrementar contador de salas completadas
                Console.WriteLine("Presiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}