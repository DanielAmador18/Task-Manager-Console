using ClaseTarea;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Gestor
{
    public class GestorTareas
    {

        private readonly string connectionString;

        public GestorTareas()               //Configuracion del builder para la cadena de conexion
        {

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public void MostrarMenu()
        {
            Console.WriteLine();
            Console.WriteLine("====BIENVENIDO====");
            Console.WriteLine("1.-Añadir tarea personal.");
            Console.WriteLine("2.-Añadir tarea de Trabajo.");
            Console.WriteLine("3.-Mostrar tareas actuales.");
            Console.WriteLine("4.-Eliminar una tarea por numero.");
            Console.WriteLine("5.-Marcar como completada");
            Console.WriteLine("6.-Salir del programa.");
        }

        public void EjecutarOpcion()
        {
            bool seguir = true;  //Declarar una variable como true para realizar la continuacion
                                 //del bucle y guardar en ella valores true o false del metodo
                                 //GestionarOpcion

            while (seguir)
            {
                Console.Clear();
                MostrarMenu();

                Console.WriteLine("Ingrese una opcion:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int opc)) //Validar desde input y guardar valor en opc
                {
                    seguir = GestionarOpcion(opc);
                }
                else
                {
                    Console.WriteLine("Por favor ingresa un numero valido.");
                }
            }
        }

        public bool GestionarOpcion(int opc)
        {
            switch (opc)
            {
                case 1: AñadirTarea(); break;

                case 2: AñadirTareaTrabajo(); break;

                case 3: MostrarTareas(); break;

                case 4: EliminarTarea(); break;

                case 5: MarcarCompletada(); break;

                case 6:
                    Salir();
                    return false;
            }

            return true; //Se devuelve true para que el bucle siga funcionando
        }

        private List<Tarea> ObtenerTareasDeLaBD()
        {
            List<Tarea> tareas = new List<Tarea>();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Descripcion, Completada, TipoTarea, Prioridad FROM Tareas ORDER BY Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tipo = reader["TipoTarea"].ToString();

                        Tarea tarea = TareaFactory.CrearTarea(tipo, reader);

                        tarea.Id = (int)reader["Id"];
                        tarea.Descripcion = reader["Descripcion"].ToString();
                        tarea.Completada = (bool)reader["Completada"];

                        tareas.Add(tarea);
                    }

                }
            }
            return tareas;
        }
        public void AñadirTarea()
        {
            Console.WriteLine("Ingresa la nueva tarea:");
            string desc = Console.ReadLine();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Tareas (Descripcion, TipoTarea) VALUES (@desc, @tipo)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@desc", desc);
                        cmd.Parameters.AddWithValue("@tipo", "Personal");
                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Tarea añadida correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir la tarea: {ex.Message}");
            }
            Console.ReadKey();
        }

        public void AñadirTareaTrabajo()
        {
            Console.WriteLine("Ingresa la nueva tarea de Trabajo:");
            string desc = Console.ReadLine();

            Console.WriteLine("Ingrese el nivel de prioridad (1.-BAJA, 2.-MEDIA, 3.-ALTA): ");
            string input = Console.ReadLine();

            int prioridad = 1; //Por defecto la prioridad sera baja
            if (int.TryParse(input, out int p) && (p >= 1 && p <= 3))
            {
                prioridad = p;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Tareas (Descripcion, TipoTarea, Prioridad) VALUES (@desc, @tipo, @prioridad)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@desc", desc);
                        cmd.Parameters.AddWithValue("@tipo", "Trabajo");
                        cmd.Parameters.AddWithValue("@prioridad", prioridad);
                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Tarea de trabajo añadida correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir la tarea: {ex.Message}");
            }

            Console.ReadKey();
        }

        public void MostrarTareas()
        {
            try
            {
                List<Tarea> tareas = ObtenerTareasDeLaBD();

                if (tareas.Count == 0)
                {
                    Console.WriteLine("Aun no hay tareas registradas.");
                    return;

                }
                else
                {
                    Console.WriteLine("Tareas actuales:");
                    for (int i = 0; i < tareas.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {tareas[i].MostrarInfo()}"); //Llama al metodo MostrarInfo sobreescrito dependiendo del tipo de objeto
                    }
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mostrar tareas: {ex.Message}");
            }
            Console.ReadKey();
        }

        public void EliminarTarea()
        {
            try
            {
                List<Tarea> tareas = ObtenerTareasDeLaBD();

                if (tareas.Count == 0)
                {
                    Console.WriteLine("Aun no hay tareas registradas.");
                    return;

                }
                Console.WriteLine("Ingrese el numero de la tarea que desee eliminar.");

                for (int i = 0; i < tareas.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tareas[i].Descripcion}");
                }

                string input = Console.ReadLine();

                if (int.TryParse(input, out int indice))
                {
                    indice--;
                    if (indice >= 0 && indice < tareas.Count)
                    {
                        int idTarea = tareas[indice].Id;

                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            string query = "DELETE FROM Tareas WHERE Id= @id";

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", idTarea);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        Console.WriteLine("Tarea eliminada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Indice fuera del rango");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no valida");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar eliminar la tarea indicada: {ex.Message}");
            }

        }
        //Metodo que marca una tarea como completada
        public void MarcarCompletada()
        {
            try
            {

                List<Tarea> tareas = ObtenerTareasDeLaBD();
                if (tareas.Count == 0)
                {
                    Console.WriteLine("No hay tareas para marcar como completadas.");
                    Console.ReadKey();
                    return;
                }

                for (int i = 0; i < tareas.Count; i++)
                {
                    string estado = tareas[i].Completada ? "[*]" : "[ ]";
                    Console.WriteLine($"{i + 1}.- {estado} {tareas[i].Descripcion}");
                }

                Console.WriteLine("Ingrese el número de la tarea que desea marcar como completada:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int indice))
                {
                    indice--; // Ajustar al índice real
                    if (indice >= 0 && indice < tareas.Count)
                    {
                        int IdTarea = tareas[indice].Id;

                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            string query = "UPDATE Tareas SET Completada = 1 WHERE Id = @id";

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", IdTarea);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Índice fuera de rango.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no válida.");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al marcar la Tarea: {ex.Message}");
            }
        }

        public void Salir()
        {
            Console.WriteLine("Gracias por usar el programa");
            System.Environment.Exit(0);
        }
    }
}