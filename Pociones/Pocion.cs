using System.Text.Json.Serialization;
public class Pocion
{
    [JsonPropertyName("nombre")]
    public string Nombre {get; set;} = string.Empty;
    [JsonPropertyName("descripcion")]
    public string Descripcion {get; set;} = string.Empty;
    [JsonPropertyName("duracion")]
    public string Rareza {get; set;} = string.Empty;
    [JsonPropertyName("efectos")]
    public Dictionary<string, int> Efectos {get; set;} = new();
}