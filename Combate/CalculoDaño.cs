using Dungeon.Personajes;

namespace Dungeon.Combate
{
    /// <summary>
    /// Clase que gestiona los cálculos matemáticos del combate
    /// </summary>
    public static class CalculoDaño
    {
        /// <summary>
        /// Calcula el daño base que inflige el personaje
        /// Incluye atributo de fuerza, armas y trinkets
        /// </summary>
        /// <param name="personaje">Personaje cuyo daño se calcula</param>
        /// <returns>Daño total calculado</returns>
        public static int CalcularDañoBase(Personaje personaje)
        {
            // Daño base de la clase multiplicado por 50
            int dañoBase = personaje.Clase.Atributos.GetValueOrDefault("fuerza", 5) * 50;

            // Bonificación de todas las armas en el inventario
            int bonusArmas = personaje.InventarioArmas
                .Where(a => a != null)
                .Sum(a => a.Daño);

            // Bonificación de trinkets que otorguen fuerza
            int bonusTrinkets = personaje.InventarioTrinkets
                .Where(t => t.Efectos.ContainsKey("fuerza"))
                .Sum(t => t.Efectos["fuerza"]);

            return dañoBase + bonusArmas + bonusTrinkets;
        }
        
        /// <summary>
        /// Calcula la defensa total del personaje
        /// Incluye atributo de constitución, armaduras y trinkets
        /// </summary>
        /// <param name="personaje">Personaje cuya defensa se calcula</param>
        /// <returns>Defensa total calculada</returns>
        public static int CalcularDefensaJugador(Personaje personaje)
        {
            // Defensa base dividida por 2 (constitución/2)
            int defensaBase = personaje.Clase.Atributos.GetValueOrDefault("constitución", 5) / 2;

            // Bonificación de todas las armaduras en el inventario
            int bonusArmaduras = personaje.InventarioArmaduras
                .Where(a => a != null)
                .Sum(a => a.Defensa);

            // Bonificación de trinkets que otorguen defensa
            int bonusTrinkets = personaje.InventarioTrinkets
                .Where(t => t.Efectos.ContainsKey("defensa"))
                .Sum(t => t.Efectos["defensa"]);

            return defensaBase + bonusArmaduras + bonusTrinkets;
        }
        
        /// <summary>
        /// Calcula si el ataque es crítico y su multiplicador de daño
        /// Probabilidad base: 20%, puede aumentarse con trinkets
        /// </summary>
        /// <param name="personaje">Personaje que realiza el ataque</param>
        /// <param name="rnd">Generador de números aleatorios</param>
        /// <returns>Tupla con (es crítico, multiplicador daño)</returns>
        public static (bool esCritico, double multiplicador) CalcularCritico(Personaje personaje, Random rnd)
        {
            // Probabilidad base de crítico: 20%
            int probCritico = 20;
            
            // Bonificación por trinkets que aumenten probabilidad de crítico
            probCritico += personaje.InventarioTrinkets
                .Where(t => t.Efectos.ContainsKey("probabilidad_critico"))
                .Sum(t => t.Efectos["probabilidad_critico"]);
            
            // Determinar si es crítico basado en probabilidad
            bool esCritico = rnd.Next(0, 100) < probCritico;
            
            // Crítico = 2x daño
            if (esCritico)
                return (true, 2.0); 
            
            // Ataque normal = 1.3x daño (13% bonus de precisión)
            return (false, 1.3); 
        }
    }
}