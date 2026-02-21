using System.Text.Json.Serialization;

namespace Dungeon.Personajes;

public class Clase
{
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = "";

    [JsonPropertyName("habilidades")]
    public List<string> Habilidades { get; set; } = new();

    [JsonPropertyName("atributos")]
    public Dictionary<string, int> Atributos { get; set; } = new();
}
