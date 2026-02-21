using System.Text.Json.Serialization;
public class Enemigo
{
    [JsonPropertyName("nombre")]
    public string Nombre {get; set;} = string.Empty;
    [JsonPropertyName("vida")]
    public int Vida {get; set;}
    [JsonPropertyName("atque")]
    public int Ataque {get; set;}
    [JsonPropertyName("defensa")]
    public int Defensa {get; set;}
    [JsonPropertyName("velocidad")]
    public int Velocidad {get; set;}
    [JsonPropertyName("tipo")]
    public string Tipo {get; set;} = string.Empty;
    [JsonPropertyName("nivel")]
    public int Nivel {get; set;}

}