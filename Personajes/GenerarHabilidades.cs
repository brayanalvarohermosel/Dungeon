namespace Dungeon.Personajes
{
    public static class GeneradorHabilidades
    {
        private static Dictionary<string, Habilidad> _habilidades = new()
        {
            // Habilidades de Guerrero
            ["Ataque poderoso"] = new Habilidad(
                "Ataque poderoso",
                costeMana: 15,
                multiplicador: 1.5,
                descripcion: "Un golpe devastador que hace 150% de daño"
            ),
            
            ["Defensa robusta"] = new Habilidad(
                "Defensa robusta",
                costeMana: 10,
                multiplicador: 0,
                descripcion: "Aumenta tu defensa temporalmente"
            )
            {
                EfectosExtra = new() { ["defensa_temporal"] = 10 }
            },
            
            ["Furia de batalla"] = new Habilidad(
                "Furia de batalla",
                costeMana: 25,
                multiplicador: 2.0,
                descripcion: "Entras en furia y haces 200% de daño"
            ),

            // Habilidades de Mago
            ["Bola de fuego"] = new Habilidad(
                "Bola de fuego",
                costeMana: 30,
                multiplicador: 2.5,
                descripcion: "Lanza una bola de fuego que hace 250% de daño"
            ),
            
            ["Escudo mágico"] = new Habilidad(
                "Escudo mágico",
                costeMana: 20,
                multiplicador: 0,
                descripcion: "Crea un escudo que bloquea daño"
            )
            {
                EfectosExtra = new() { ["escudo"] = 30 }
            },
            
            ["Teletransportación"] = new Habilidad(
                "Teletransportación",
                costeMana: 15,
                multiplicador: 0,
                descripcion: "Esquivas el siguiente ataque"
            )
            {
                EfectosExtra = new() { ["evasion"] = 1 }
            },

            // Habilidades de Pícaro
            ["Ataque furtivo"] = new Habilidad(
                "Ataque furtivo",
                costeMana: 20,
                multiplicador: 2.0,
                descripcion: "Un ataque sorpresa que hace 200% de daño"
            ),
            
            ["Desactivar trampas"] = new Habilidad(
                "Desactivar trampas",
                costeMana: 10,
                multiplicador: 0,
                descripcion: "Encuentras objetos ocultos"
            )
            {
                EfectosExtra = new() { ["buscar_tesoro"] = 1 }
            },
            
            ["Camuflaje"] = new Habilidad(
                "Camuflaje",
                costeMana: 15,
                multiplicador: 1.0,
                descripcion: "Te vuelves invisible brevemente"
            )
            {
                EfectosExtra = new() { ["invisibilidad"] = 1 }
            },

            // Habilidades de Clérigo
            ["Curación"] = new Habilidad(
                "Curación",
                costeMana: 25,
                multiplicador: 0,
                descripcion: "Restaura 40 puntos de vida"
            )
            {
                EfectosExtra = new() { ["curacion"] = 40 }
            },
            
            ["Protección divina"] = new Habilidad(
                "Protección divina",
                costeMana: 20,
                multiplicador: 0,
                descripcion: "Aumenta tu defensa y vida"
            )
            {
                EfectosExtra = new() { ["defensa_temporal"] = 15, ["curacion"] = 20 }
            },
            
            ["Expulsar no-muertos"] = new Habilidad(
                "Expulsar no-muertos",
                costeMana: 30,
                multiplicador: 3.0,
                descripcion: "Hace 300% de daño a no-muertos"
            ),

            // Habilidades de Explorador
            ["Rastreo"] = new Habilidad(
                "Rastreo",
                costeMana: 10,
                multiplicador: 0,
                descripcion: "Revela información del enemigo"
            )
            {
                EfectosExtra = new() { ["revelar_enemigo"] = 1 }
            },
            
            ["Tiro preciso"] = new Habilidad(
                "Tiro preciso",
                costeMana: 20,
                multiplicador: 1.8,
                descripcion: "Un disparo certero que hace 180% de daño"
            ),
            
            ["Supervivencia"] = new Habilidad(
                "Supervivencia",
                costeMana: 15,
                multiplicador: 0,
                descripcion: "Recuperas 25 de vida y 10 de mana"
            )
            {
                EfectosExtra = new() { ["curacion"] = 25, ["mana"] = 10 }
            }
        };
        public static List<Habilidad> ObtenerHabilidadesDeClase(Clase clase)
        {
            return clase.Habilidades
                .Where(nombreHab => _habilidades.ContainsKey(nombreHab))
                .Select(nombreHab => _habilidades[nombreHab])
                .ToList();
        }
        public static Habilidad? ObtenerHabilidad(string nombre)
        {
            return _habilidades.GetValueOrDefault(nombre);
        }
    }
}