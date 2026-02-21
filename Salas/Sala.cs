using System.Text.Json.Serialization;
using Dungeon.Enemigos;

namespace Dungeon.Salas
{
    /// <summary>
    /// Representa una sala del dungeon
    /// Contiene enemigos, objetos y información de ubicación
    /// </summary>
    public class Sala
    {
        /// <summary>
        /// ID único de la sala
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        /// <summary>
        /// Lista de enemigos presentes en la sala
        /// </summary>
        [JsonPropertyName("enemigos")]
        public List<Enemigo> Enemigos { get; set; } = new();

        /// <summary>
        /// Número de piso (nivel de dificultad 1-4)
        /// </summary>
        [JsonPropertyName("piso")]
        public int Piso { get; set; }

        /// <summary>
        /// Número de planta dentro del piso (1-3)
        /// La planta 3 es siempre una sala jefe
        /// </summary>
        [JsonPropertyName("planta")]
        public int Planta { get; set; }
        
        /// <summary>
        /// Indica si esta es una sala jefe
        /// (siempre la planta 3 de cada piso)
        /// </summary>
        [JsonPropertyName("SalaBoss")]
        public bool SalaBoss { get; set; }

        /// <summary>
        /// Lista de objetos disponibles en la sala
        /// Pueden ser Arma, Armadura, Pocion o Trinket
        /// </summary>
        [JsonPropertyName("objetos")]
        public List<object> Objetos { get; set; } = new();
    }

}
