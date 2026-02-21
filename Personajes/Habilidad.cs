namespace Dungeon.Personajes
{
    public class Habilidad
    {
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int CosteMana { get; set; }
        public double MultiplicadorDaño { get; set; }
        public Dictionary<string, int> EfectosExtra { get; set; } = new();
        public Habilidad(string nombre, int costeMana, double multiplicador, string descripcion = "")
        {
            Nombre = nombre;
            CosteMana = costeMana;
            MultiplicadorDaño = multiplicador;
            Descripcion = descripcion;
        }
    }
}