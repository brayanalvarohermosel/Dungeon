using System.Text.Json.Serialization;
public class Arma
{
    [JsonPropertyName("nombre")]
    public string Nombre {get; set;} = string.Empty;
    [JsonPropertyName("tipo")]
    public string Tipo {get; set;} = string.Empty;
    [JsonPropertyName("daño")]
    public int Daño {get; set;}
    [JsonPropertyName("precio")]
    public int Precio {get; set;}
    [JsonPropertyName("rareza")]
    public string Rareza {get; set;} = string.Empty;

}