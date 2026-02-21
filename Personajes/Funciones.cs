namespace Dungeon.Personajes
{
    /// <summary>
    /// Clase con funciones relacionadas a personajes y clases
    /// </summary>
    public static class Funciones
    {
        /// <summary>
        /// Permite al usuario seleccionar una clase con confirmación
        /// Muestra detalles de cada clase disponible
        /// </summary>
        /// <param name="clases">Lista de clases disponibles</param>
        /// <returns>Clase seleccionada por el usuario</returns>
        public static Clase SeleccionClase(List<Clase> clases)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SELECCIONA TU CLASE ===\n");

                // Mostrar todas las clases disponibles
                for (int i = 0; i < clases.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {clases[i].Nombre}");
                }

                // Leer opción del usuario
                Console.Write("\nElige una clase: ");
                if (!int.TryParse(Console.ReadLine(), out int opcion) ||
                    opcion < 1 || opcion > clases.Count)
                {
                    Console.WriteLine("Opción no válida. Pulsa una tecla...");
                    Console.ReadKey();
                    continue;
                }

                Clase claseSeleccionada = clases[opcion - 1];

                // Mostrar información detallada de la clase elegida
                MostrarDetalles(claseSeleccionada);

                // Pedir confirmación
                Console.Write("\n¿Confirmar esta clase? (s/n): ");
                string? confirmar = Console.ReadLine()?.ToLower();

                if (confirmar == "s")
                {
                    Console.WriteLine($"\nHas elegido: {claseSeleccionada.Nombre}");
                    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    
                    Console.ReadKey();
                    return claseSeleccionada;
                }
            }
        }

        /// <summary>
        /// Muestra información detallada de una clase
        /// Incluyendo descripción, habilidades y atributos
        /// </summary>
        /// <param name="clase">Clase a mostrar</param>
        private static void MostrarDetalles(Clase clase)
        {
            Console.WriteLine($"\n=== {clase.Nombre.ToUpper()} ===");
            Console.WriteLine(clase.Descripcion);

            // Mostrar habilidades de la clase
            Console.WriteLine("\nHabilidades:");
            foreach (var h in clase.Habilidades)
                Console.WriteLine(" - " + h);

            // Mostrar atributos base de la clase
            Console.WriteLine("\nAtributos:");
            foreach (var a in clase.Atributos)
            {
                Console.WriteLine(" - " + a.Key + ": " + a.Value);
            }
        }
    }

}