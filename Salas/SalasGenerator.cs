using Dungeon.Armaduras;
using Dungeon.Armas;
using Dungeon.Enemigos;
using Dungeon.Pociones;
using Dungeon.Trinkets;

namespace Dungeon.Salas
{
    /// <summary>
    /// Clase encargada de generar el dungeon completo
    /// </summary>
    public class SalasGenerator
    {
        /// <summary>
        /// Genera el dungeon completo con 4 pisos y 3 plantas cada uno
        /// Estructura: Piso 1-4, Planta 1-3 (total 12 salas)
        /// </summary>
        /// <param name="enemigos">Lista de todos los enemigos disponibles</param>
        /// <param name="armas">Lista de todas las armas disponibles</param>
        /// <param name="armaduras">Lista de todas las armaduras disponibles</param>
        /// <param name="pociones">Lista de todas las pociones disponibles</param>
        /// <param name="trinkets">Lista de todos los trinkets disponibles</param>
        /// <returns>Lista de salas generadas</returns>
        public static List<Sala> Generar(
            List<Enemigo> enemigos,
            List<Arma> armas,
            List<Armadura> armaduras,
            List<Pocion> pociones,
            List<Trinket> trinkets
        )
        {
            Random rnd = new();
            List<Sala> salas = new();
            int idSala = 1;

            // Generar 4 pisos (niveles de dificultad)
            for (int piso = 1; piso <= 4; piso++)
            {
                // Cada piso tiene 3 plantas (salas)
                for (int planta = 1; planta <= 3; planta++)
                {
                    // La tercera planta es siempre una sala jefe
                    bool esPlantaBoss = (planta == 3);
                    
                    // Crear sala con sus propiedades
                    Sala sala = new()
                    {
                        Id = idSala,
                        Piso = piso,
                        Planta = planta,
                        SalaBoss = esPlantaBoss,
                        
                        // Generar enemigos específicos para esta sala
                        Enemigos = GenerarEnemigos(enemigos, piso, esPlantaBoss, rnd),
                        // Generar objetos disponibles en la sala
                        Objetos = GenerarObjetos(armas, armaduras, pociones, trinkets, piso, esPlantaBoss, rnd)
                    };
                    
                    salas.Add(sala);
                    idSala++;
                }
            }

            return salas;
        }

        /// Genera los enemigos para una sala específica
        /// <param name="todosEnemigos">Lista de todos los enemigos disponibles</param>
        /// <param name="piso">Número de piso (nivel de dificultad)</param>
        /// <param name="esPlantaBoss">Si es una sala jefe o normal</param>
        /// <param name="rnd">Generador de números aleatorios</param>
        /// <returns>Lista de enemigos para esta sala</returns>
        private static List<Enemigo> GenerarEnemigos(
            List<Enemigo> todosEnemigos, 
            int piso, 
            bool esPlantaBoss,
            Random rnd)
        {
            if (esPlantaBoss)
            {
                // SALA JEFE: 1 jefe + 2 subditos del mismo nivel
                var jefe = todosEnemigos
                    .Where(e => e.Tipo == "Jefe" && e.Nivel == piso)
                    .OrderBy(_ => rnd.Next())
                    .FirstOrDefault();
                
                var subditos = todosEnemigos
                    .Where(e => e.Tipo == "subdito" && e.Nivel == piso)
                    .OrderBy(_ => rnd.Next())
                    .Take(2)
                    .ToList();
                
                // Añadir jefe y subditos a la lista
                var enemigos = new List<Enemigo>();
                if (jefe != null) enemigos.Add(jefe);
                enemigos.AddRange(subditos);
                
                return enemigos;
            }
            else
            {
                // SALA NORMAL: 3 subditos del nivel correspondiente
                return todosEnemigos
                    .Where(e => e.Tipo == "subdito" && e.Nivel == piso)
                    .OrderBy(_ => rnd.Next())
                    .Take(3)
                    .ToList();
            }
        }

        /// <summary>
        /// Genera los objetos disponibles en una sala
        /// </summary>
        /// <param name="armas">Lista de todas las armas</param>
        /// <param name="armaduras">Lista de todas las armaduras</param>
        /// <param name="pociones">Lista de todas las pociones</param>
        /// <param name="trinkets">Lista de todos los trinkets</param>
        /// <param name="piso">Número de piso</param>
        /// <param name="esPlantaBoss">Si es una sala jefe</param>
        /// <param name="rnd">Generador de números aleatorios</param>
        /// <returns>Lista de objetos aleatorios para esta sala</returns>
        private static List<object> GenerarObjetos(
            List<Arma> armas,
            List<Armadura> armaduras,
            List<Pocion> pociones,
            List<Trinket> trinkets,
            int piso,
            bool esPlantaBoss,
            Random rnd)
        {
            // Salas jefe tienen más objetos que las normales
            int cantidadObjetos = esPlantaBoss ? rnd.Next(2, 4) : rnd.Next(1, 3);
            
            // Combinar todas las listas de objetos, barajar y seleccionar
            return armas.Cast<object>()
                .Concat(armaduras)
                .Concat(pociones)
                .Concat(trinkets)
                .OrderBy(_ => rnd.Next())
                .Take(cantidadObjetos)
                .ToList();
        }
    }
}