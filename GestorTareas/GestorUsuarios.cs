using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Gestor;
using Users;
using Microsoft.Extensions.Configuration;
namespace GestorU
{ 
        
    public class GestorUsuarios
    {
            private readonly string _connectionString;

        public GestorUsuarios()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
             _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
   
        public void Inicio()
        {
            Console.WriteLine("Bienvenido");
            Console.WriteLine("1.-Ya tengo una cuenta");
            Console.WriteLine("2.-Quiero registrarme");

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

            if (string.IsNullOrEmpty(nom))
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
            if(contra.Length > 10)
            {
                Console.WriteLine("La contraseña no puede ser tan extensa.");
                Console.ReadKey();
                return;
            }

            string hash = BCrypt.Net.BCrypt.HashPassword(contra);

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Usuarios (Nombre, ContraseñaHash) VALUES (@nom, @hash)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nom", nom);
                        cmd.Parameters.AddWithValue("@hash", hash);
                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Usuario registrado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir al usuario: {ex.Message}");
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
            if (contra.Length > 10)
            {
                Console.WriteLine("La contraseña no puede ser tan extensa.");
                Console.ReadKey();
                return;
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

                            while (reader.Read())
                            {
                                string nombreDB = reader["Nombre"].ToString();
                                string contraDB = reader["ContraseñaHash"].ToString();

                                if (nombreDB.Equals(nom, StringComparison.OrdinalIgnoreCase))
                                {
                                    if (BCrypt.Net.BCrypt.Verify(contra, contraDB))
                                    {
                                        Console.WriteLine("Ha ingresado al Menu");
                                        Console.ReadKey();
                                        GestorTareas gestion = new GestorTareas();
                                        gestion.EjecutarOpcion();
                                    }
                                }

                            }
                        }
                    }

                }
            }catch(Exception ex)
            {
                Console.ReadKey();
                Console.WriteLine("No se ha podido encontrar su cuenta " + ex.Message);
            }
        }
    }
}
