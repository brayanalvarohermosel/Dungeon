using Dungeon.Personajes;

namespace Dungeon.Estadisticas
{
    /// <summary>
    /// Sistema que gestiona la presentación de estadísticas del jugador
    /// </summary>
    public static class SistemaEstadisticas
    {
        /// <summary>
        /// Muestra las estadísticas completas del personaje
        /// Incluye enemigos derrotados, daño total, mejores items, etc.
        /// </summary>
        /// <param name="personaje">Personaje cuyas estadísticas se mostrarán</param>
        public static void MostrarEstadisticas(Personaje personaje)
        {
            Console.Clear();

            Console.WriteLine("╔═══════════════════════════════════════╗");
            Console.WriteLine("║       📊 ESTADÍSTICAS FINALES         ║");
            Console.WriteLine("╚═══════════════════════════════════════╝\n");

            // INFORMACIÓN BÁSICA DEL PERSONAJE
            Console.WriteLine($"👤 {personaje.Nombre} ({personaje.Clase.Nombre})");
            Console.WriteLine($"{'─',40}\n");

            // ESTADÍSTICAS DE PROGRESO
            Console.WriteLine($"💀 Enemigos derrotados: {personaje.EnemigosDerrotados.Count}");
            Console.WriteLine($"⚔️ Daño total infligido: {personaje.DañoTotalInfligido}");
            Console.WriteLine($"🏰 Salas completadas: {personaje.SalasCompletadas}");

            // DESGLOSE DE ENEMIGOS DERROTADOS POR TIPO
            if (personaje.EnemigosDerrotados.Any())
            {
                Console.WriteLine("\n🗡️ ENEMIGOS DERROTADOS POR TIPO:");
                var enemigosPorTipo = personaje.EnemigosDerrotados
                    .GroupBy(e => e.Tipo)
                    .Select(grupo => $"  • {grupo.Key}: {grupo.Count()}")
                    .Aggregate((a, b) => a + "\n" + b);

                Console.WriteLine(enemigosPorTipo);
            }

            // MEJOR ARMA EN INVENTARIO
            if (personaje.InventarioArmas.Any())
            {
                var mejorArma = personaje.InventarioArmas.MaxBy(a => a.Daño);
                Console.WriteLine($"\n⭐ Mejor arma: {mejorArma?.Nombre} (⚔️ {mejorArma?.Daño} daño)");
            }

            // MEJOR ARMADURA EN INVENTARIO
            if (personaje.InventarioArmaduras.Any())
            {
                var mejorArmadura = personaje.InventarioArmaduras.MaxBy(a => a.Defensa);
                Console.WriteLine($"⭐ Mejor armadura: {mejorArmadura?.Nombre} (🛡️ {mejorArmadura?.Defensa} defensa)");
            }

            // TOTAL DE OBJETOS ACUMULADOS
            int totalObjetos = personaje.InventarioArmas.Count +
            personaje.InventarioArmaduras.Count + personaje.InventarioPociones.Count + personaje.InventarioTrinkets.Count;

            Console.WriteLine($"\n📦 Total de objetos: {totalObjetos}");

            Console.WriteLine($"\n{'═',40}");
        }
    }
}