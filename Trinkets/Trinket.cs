using System.Text.Json.Serialization;
public class Trinket
{
    [JsonPropertyName("id")]
    public int Id {get; set;}
    [JsonPropertyName("nombre")]
    public string Nombre {get; set;} = string.Empty;
    [JsonPropertyName("descripcion")]
    public string Descripcion {get; set;} = string.Empty;
    [JsonPropertyName("efectos")]
    public Dictionary<string, int> Efectos {get; set;} = new();
}