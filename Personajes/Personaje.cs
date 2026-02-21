namespace Dungeon.Personajes
{
    /// <summary>
    /// Representa un personaje jugador en el dungeon
    /// Contiene información de clase, inventarios y estadísticas
    /// </summary>
    public class Personaje
    {
        // PROPIEDADES BÁSICAS
        public Clase Clase { get; set; } = new();      // Clase del personaje
        public string Nombre { get; set; } = "Aventurero";  // Nombre del personaje
        public int VidaActual { get; set; }            // Vida actual del personaje
        public int ManaActual { get; set; }            // Mana actual del personaje

        // HABILIDADES Y COMBATE
        public List<Habilidad> HabilidadesDisponibles { get; set; } = new();

        // INVENTARIOS DE OBJETOS
        public List<Arma> InventarioArmas { get; set; } = new();
        public List<Armadura> InventarioArmaduras { get; set; } = new();
        public List<Pocion> InventarioPociones { get; set; } = new();
        public List<Trinket> InventarioTrinkets { get; set; } = new();

        // ESTADÍSTICAS DE PROGRESO
        public List<Enemigo> EnemigosDerrotados { get; set; } = new();
        public int DañoTotalInfligido { get; set; } = 0;
        public int SalasCompletadas { get; set; } = 0;
        
        /// <summary>
        /// Constructor sin parámetros
        /// </summary>
        public Personaje() { }

        /// <summary>
        /// Constructor que crea un nuevo personaje con clase y nombre
        /// Inicializa vida, mana y habilidades según la clase
        /// </summary>
        /// <param name="clase">Clase seleccionada para el personaje</param>
        /// <param name="nombre">Nombre del personaje</param>
        public Personaje(Clase clase, string nombre)
        {
            Clase = clase;
            Nombre = nombre;
            // Inicializar vida y mana según los atributos de la clase
            VidaActual = clase.Atributos.GetValueOrDefault("vida", 100);
            ManaActual = clase.Atributos.GetValueOrDefault("mana", 50);

            // Generar habilidades disponibles según la clase
            HabilidadesDisponibles = GeneradorHabilidades.ObtenerHabilidadesDeClase(clase);
        }
    }
}