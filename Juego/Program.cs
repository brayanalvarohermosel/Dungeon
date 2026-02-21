using System.Text.Json;

using Dungeon.Armaduras;
using Dungeon.Armas;
using Dungeon.Enemigos;
using Dungeon.Pociones;
using Dungeon.Personajes;
using Dungeon.Trinkets;
using Dungeon.Salas;
using Dungeon.FuncionesSalas;
using Dungeon.Combate;
using Dungeon.Estadisticas;
using Dungeon.Eventos;

/// <summary>
/// Carga un archivo JSON y lo desserializa en una lista de tipo T
/// </summary>
/// <typeparam name="T">Tipo de objeto a cargar</typeparam>
/// <param name="ruta">Ruta al archivo JSON</param>
/// <returns>Lista de objetos del tipo especificado</returns>
List<T> CargarJson<T>(string ruta)
{
    var json = File.ReadAllText(ruta);
    var jsonLeido = JsonSerializer.Deserialize<List<T>>(json);

    if (jsonLeido is null)
    {
        throw new Exception($"No se pudo cargar el json: {ruta}");
    }

    return jsonLeido;
}

// Encontrar la ruta base del proyecto (carpeta que contiene "Armaduras")
string rutaBase = Directory.GetCurrentDirectory();
while (!Directory.Exists(Path.Combine(rutaBase, "Armaduras")) &&
       Directory.GetParent(rutaBase) != null)
{
    rutaBase = Directory.GetParent(rutaBase).FullName;
}

Console.WriteLine($"📁 Cargando desde: {rutaBase}\n");
Console.WriteLine("Cargando datos del juego...\n");

// Cargar todos los datos JSON necesarios para el juego
List<Armadura> armaduras = CargarJson<Armadura>(Path.Combine(rutaBase, "Armaduras", "armaduras.json"));
List<Arma> armas = CargarJson<Arma>(Path.Combine(rutaBase, "Armas", "armas.json"));
List<Enemigo> enemigos = CargarJson<Enemigo>(Path.Combine(rutaBase, "Enemigos", "enemigos.json"));
List<Clase> clases = CargarJson<Clase>(Path.Combine(rutaBase, "Personajes", "clases.json"));
List<Pocion> pociones = CargarJson<Pocion>(Path.Combine(rutaBase, "Pociones", "pociones.json"));
List<Trinket> trinkets = CargarJson<Trinket>(Path.Combine(rutaBase, "Trinkets", "trinkets.json"));
List<Evento> eventos = CargarJson<Evento>(Path.Combine(rutaBase, "Eventos", "eventos.json"));

Console.WriteLine("✅ Datos cargados correctamente!\n");

// Generar el dungeon con todas las salas necesarias
var salas = SalasGenerator.Generar(
    enemigos,
    armas,
    armaduras,
    pociones,
    trinkets
);

Console.WriteLine($"✅ Dungeon generado: {salas.Count} salas\n");

// Variables de control del juego
bool salir = false;              // Controla si salir del juego
Personaje? personaje = null;     // Personaje actual del jugador
int salaActual = 0;              // Índice de la sala actual

// BUCLE PRINCIPAL DEL JUEGO - Menú principal
while (!salir)
{
    Console.Clear();
    Console.WriteLine("╔═══════════════════════════════════════════════════╗");
    Console.WriteLine("║                  🏰 DUNGEON RPG 🏰                ║");
    Console.WriteLine("╚═══════════════════════════════════════════════════╝\n");

    // Mostrar información del personaje si existe
    if (personaje != null)
    {
        Console.WriteLine($"👤 Personaje: {personaje.Nombre} ({personaje.Clase.Nombre})");
        Console.WriteLine($"❤️  Vida: {personaje.VidaActual}/{personaje.Clase.Atributos["vida"]}");
        Console.WriteLine($"✨ Mana: {personaje.ManaActual}/{personaje.Clase.Atributos["mana"]}");
        Console.WriteLine($"📍 Sala actual: {salaActual + 1}/{salas.Count}");
        Console.WriteLine($"{'─',50}\n");
    }

    Console.WriteLine("1. Crear/Seleccionar personaje");
    Console.WriteLine("2. Entrar en la mazmorra");
    Console.WriteLine("3. Ver inventario");
    Console.WriteLine("0. Salir");
    Console.Write("\nElige opción: ");

    var opcion = Console.ReadLine();

    // MENÚ PRINCIPAL - Opciones disponibles
    switch (opcion)
    {
        // OPCIÓN 1: Crear/Seleccionar personaje
        case "1":
            Clase claseSeleccionada = Funciones.SeleccionClase(clases);

            Console.Write("\n¿Cómo se llama tu personaje? ");
            string? nombre = Console.ReadLine();
            if (string.IsNullOrEmpty(nombre))
                nombre = "Aventurero";

            personaje = new Personaje(claseSeleccionada, nombre);
            salaActual = 0;

            Console.WriteLine($"\n✅ ¡{personaje.Nombre} ha sido creado!");
            Console.WriteLine($"Clase: {personaje.Clase.Nombre}");

            if (personaje.HabilidadesDisponibles.Any())
            {
                Console.WriteLine($"\n✨ Habilidades aprendidas:");
                var habilidadesTexto = personaje.HabilidadesDisponibles
                    .Select(h => $"  • {h.Nombre} ({h.CosteMana} mana) - {h.Descripcion}")
                    .Aggregate((a, b) => a + "\n" + b);

                Console.WriteLine(habilidadesTexto);
            }

            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
            break;

        // OPCIÓN 2: Entrar en la mazmorra
        case "2":
            if (personaje is null)
            {
                Console.WriteLine("\n❌ Primero crea un personaje.");
                Console.ReadKey();
                break;
            }

            if (personaje.VidaActual <= 0)
            {
                Console.WriteLine("\n💀 Tu personaje ha muerto. Crea uno nuevo.");
                Console.ReadKey();
                break;
            }
            bool salirMazmorra = false;

            while (salaActual < salas.Count && !salirMazmorra && personaje.VidaActual > 0)
            {
                Sala sala = salas[salaActual];

                Combate.MenuCombate(personaje, sala, eventos);

                if (personaje.VidaActual <= 0)
                {
                    Console.WriteLine("\n💀 Has caído en la mazmorra...");
                    Console.WriteLine("\nPresiona cualquier tecla para volver al menú...");
                    Console.ReadKey();
                    salirMazmorra = true;
                    break;
                }

                if (sala.Enemigos.Any())
                {
                    Console.WriteLine("\n🏃 Has huido de la sala.");
                    Console.WriteLine("¿Quieres volver al menú principal? (s/n)");
                    string? respuesta = Console.ReadLine()?.ToLower();

                    if (respuesta == "s")
                    {
                        salirMazmorra = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                Console.WriteLine("\n✅ ¡Sala completada!");
                
                if (sala.Planta == 3)
                {
                    Console.WriteLine($"\n╔{'═',50}╗");
                    Console.WriteLine($"║  🎊 ¡PISO {sala.Piso} COMPLETADO! 🎊{new string(' ', 23)}║");
                    Console.WriteLine($"╚{'═',50}╝");

                    int curacion = personaje.Clase.Atributos["vida"] / 4;
                    personaje.VidaActual = Math.Min(
                        personaje.VidaActual + curacion,
                        personaje.Clase.Atributos["vida"]
                    );
                    Console.WriteLine($"\n💚 Descansas y recuperas {curacion} puntos de vida");

                    int manaCurado = personaje.Clase.Atributos["mana"] / 2;
                    personaje.ManaActual = Math.Min(
                        personaje.ManaActual + manaCurado,
                        personaje.Clase.Atributos["mana"]
                    );
                    Console.WriteLine($"✨ Recuperas {manaCurado} puntos de mana");

                    if (salaActual + 1 < salas.Count)
                    {
                        Console.WriteLine($"\n🔼 ¿Quieres avanzar al Piso {sala.Piso + 1}?");
                        Console.WriteLine("1. Continuar aventura");
                        Console.WriteLine("2. Volver al menú (descansar)");
                        Console.Write("\nElige opción: ");

                        string? respuesta = Console.ReadLine();

                        if (respuesta == "2")
                        {
                            Console.WriteLine("\n🏕️ Decides descansar. Puedes continuar más tarde.");
                            salirMazmorra = true;
                            salaActual++;
                        }
                        else
                        {
                            Console.WriteLine($"\n⬆️  Avanzas al Piso {sala.Piso + 1}...");
                            salaActual++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n╔{'═',50}╗");
                        Console.WriteLine("║  🎊🎊 ¡MAZMORRA COMPLETADA! 🎊🎊              ║");
                        Console.WriteLine($"╚{'═',50}╝");

                        SistemaEstadisticas.MostrarEstadisticas(personaje);
                        salaActual++;
                        salirMazmorra = true;
                    }
                }
                else
                {
                    Console.WriteLine($"➡️  Avanzas a la Planta {sala.Planta + 1}...");
                    salaActual++;

                    System.Threading.Thread.Sleep(1500);
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }

            // Mensaje final si completó todo
            if (salaActual >= salas.Count && personaje.VidaActual > 0)
            {
                Console.WriteLine("\n🏆 ¡Eres un verdadero héroe!");
            }

            break;

        // OPCIÓN 3: Ver inventario y estadísticas
        case "3":
            if (personaje is null)
            {
                Console.WriteLine("\n❌ Primero crea un personaje.");
                Console.ReadKey();
                break;
            }

            SistemaEstadisticas.MostrarEstadisticas(personaje);
            MostrarInventarioCompleto(personaje);
            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
            break;

        // OPCIÓN 0: Salir del juego
        case "0":
            Console.WriteLine("\n👋 ¡Hasta luego, aventurero!");
            salir = true;
            System.Threading.Thread.Sleep(2000);
            break;

        default:
            Console.WriteLine("\n❌ Opción no válida.");
            Console.ReadKey();
            break;
    }
}

/// Muestra de forma detallada todo el inventario del personaje
/// incluyendo armas, armaduras, pociones y trinkets
/// <param name="personaje">Personaje cuyo inventario se mostrará</param>
void MostrarInventarioCompleto(Personaje personaje)
{
    Console.Clear();
    Console.WriteLine("╔═══════════════════════════════════════╗");
    Console.WriteLine("║           📦 INVENTARIO               ║");
    Console.WriteLine("╚═══════════════════════════════════════╝\n");

    int totalObjetos = 0;

    if (personaje.InventarioArmas.Any())
    {
        Console.WriteLine("🗡️  ARMAS:");
        var armasInfo = personaje.InventarioArmas
            .Select(a => $"  • {a.Nombre} (⚔️ Daño: {a.Daño}, {a.Rareza})")
            .Aggregate((a, b) => a + "\n" + b);
        Console.WriteLine(armasInfo);
        totalObjetos += personaje.InventarioArmas.Count;
    }

    if (personaje.InventarioArmaduras.Any())
    {
        Console.WriteLine("\n🛡️  ARMADURAS:");
        var armadurasInfo = personaje.InventarioArmaduras
            .Select(a => $"  • {a.Nombre} (🛡️ Defensa: {a.Defensa}, {a.Rareza})")
            .Aggregate((a, b) => a + "\n" + b);
        Console.WriteLine(armadurasInfo);
        totalObjetos += personaje.InventarioArmaduras.Count;
    }

    if (personaje.InventarioPociones.Any())
    {
        Console.WriteLine("\n🧪 POCIONES:");
        var pocionesInfo = personaje.InventarioPociones
            .Select(p => $"  • {p.Nombre} ({p.Rareza})")
            .Aggregate((a, b) => a + "\n" + b);
        Console.WriteLine(pocionesInfo);
        totalObjetos += personaje.InventarioPociones.Count;
    }

    if (personaje.InventarioTrinkets.Any())
    {
        Console.WriteLine("\n💍 TRINKETS:");
        var trinketsInfo = personaje.InventarioTrinkets
            .Select(t => $"  • {t.Nombre}")
            .Aggregate((a, b) => a + "\n" + b);
        Console.WriteLine(trinketsInfo);
        totalObjetos += personaje.InventarioTrinkets.Count;
    }

    if (totalObjetos == 0)
    {
        Console.WriteLine("Tu inventario está vacío.");
    }
    else
    {
        Console.WriteLine($"\n{'─',40}");
        Console.WriteLine($"📦 Total de objetos: {totalObjetos}");

        var mejorArma = personaje.InventarioArmas.MaxBy(a => a.Daño);
        if (mejorArma != null)
        {
            Console.WriteLine($"⭐ Mejor arma: {mejorArma.Nombre} (⚔️ {mejorArma.Daño})");
        }

        var mejorArmadura = personaje.InventarioArmaduras.MaxBy(a => a.Defensa);
        if (mejorArmadura != null)
        {
            Console.WriteLine($"⭐ Mejor armadura: {mejorArmadura.Nombre} (🛡️ {mejorArmadura.Defensa})");
        }
    }

    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
    Console.ReadKey();
}