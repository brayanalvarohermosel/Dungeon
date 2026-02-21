using System.Text.Json.Serialization;
public class Armadura
{
    [JsonPropertyName("nombre")]
    public string Nombre {get; set;} = string.Empty;
    [JsonPropertyName("defensa")]
    public int Defensa {get; set;}
    [JsonPropertyName("peso")]
    public int Peso {get; set;}
    [JsonPropertyName("material")]
    public string Material {get; set;} = string.Empty;
    [JsonPropertyName("rareza")]
    public string Rareza {get; set;} = string.Empty;

}