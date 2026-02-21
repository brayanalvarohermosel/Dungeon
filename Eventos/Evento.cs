using System.Text.Json.Serialization;

namespace Dungeon.Eventos
{
    public class Evento
    {
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = "";
        
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = "";
        
        [JsonPropertyName("probabilidad")]
        public int Probabilidad { get; set; } = 30;
        
        [JsonPropertyName("efectos")]
        public Dictionary<string, int> Efectos { get; set; } = new();
    }
}