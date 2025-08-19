using Gestor;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace GestorU
{

    public class GestorUsuarios
    {
        private readonly string _connectionString;

        //Constructor que crea la configuracion para la cadena de conexion
        public GestorUsuarios()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //Agrega el appsettings.json donde se encuentra la cadena de conexion
            .Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Inicio()
        {
            Console.WriteLine("Bienvenido");
            Console.WriteLine("1.-Ya tengo una cuenta");
            Console.WriteLine("2.-Quiero registrarme");
            Console.Write("Elija una opcion: ");
            int opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 1: Loggin(); break;

                case 2: Registro(); break;
            }


        }

        public void Registro()
        {
            Console.WriteLine("Ingresa el nombre de usuario:");
            string nom = Console.ReadLine();

            if (string.IsNullOrEmpty(nom))  //Validacion para un posible campo vacio
            {
                Console.WriteLine("El campo no puede estar vacio.");
                return;
            }


            Console.WriteLine("Ingresa la contraseña:");
            string contra = Console.ReadLine();

            if (string.IsNullOrEmpty(contra))
            {
                Console.WriteLine("El campo no puede estar vacio.");
                Console.ReadKey();
                return;
            }
            if (contra.Length > 10)  //Validacion para extension no mayor a 10 caracteres
            {
                Console.WriteLine("La contraseña no puede ser tan extensa.");
                Console.ReadKey();
                return;
            }

            string hash = BCrypt.Net.BCrypt.HashPassword(contra); //contra se convierte en hash y es guardada en la variable hash

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Usuarios (Nombre, ContraseñaHash) VALUES (@nom, @hash)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nom", nom);       //Se utilizan parametros para evitar inyeccion SQL
                        cmd.Parameters.AddWithValue("@hash", hash);
                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Usuario registrado exitosamente");
                VolverInicio(); //Metodo llamado para regresar al inicio
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir al usuario: {ex.Message}");
                VolverInicio();
            }
            Console.ReadKey();
        }


        public void Loggin()
        {

            Console.Clear();
            Console.WriteLine("Ingresa el nombre de usuario:");
            string nom = Console.ReadLine();

            if (string.IsNullOrEmpty(nom))
            {
                Console.WriteLine("El campo no puede estar vacio.");
                VolverInicio();
            }


            Console.WriteLine("Ingresa la contraseña:");
            string contra = Console.ReadLine();

            if (string.IsNullOrEmpty(contra))
            {
                Console.WriteLine("El campo no puede estar vacio.");
                VolverInicio();
            }
            if (contra.Length > 10)
            {
                Console.WriteLine("La contraseña no puede ser tan extensa.");
                VolverInicio();
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT Nombre, ContraseñaHash FROM Usuarios WHERE Nombre = @nombre";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nom);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if(reader.Read()) //La consulta SQL hace el filtrado de Nombre con nom, si no encuentra filas, devulve false y da feedback al usuario.
                            {
                                string nombreDB = reader["Nombre"].ToString();
                                string contraDB = reader["ContraseñaHash"].ToString();

                                    if (BCrypt.Net.BCrypt.Verify(contra, contraDB))
                                    {
                                        Console.WriteLine("Ha ingresado al Menu");
                                        Console.ReadKey();
                                        GestorTareas gestion = new GestorTareas();
                                        gestion.EjecutarOpcion();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Contraseña incorrecta.");
                                        VolverInicio();
                                    }
                            }
                            else
                            {
                                Console.WriteLine("Usuario no encontrado.");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.ReadKey();
                Console.WriteLine("No se ha podido encontrar su cuenta " + ex.Message);
                VolverInicio();
            }
        }

        public void VolverInicio()
        {
            Console.WriteLine("Volviendo a inicio...");
            Console.ReadKey();
            Console.Clear();
            Inicio();
        }
    }
}
