using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Gestor;
using Users;
namespace GestorU
{ 
        
    public class GestorUsuarios
    {

        private readonly string _connectionString;

        public GestorUsuarios(string connectionString)
        {
            _connectionString = connectionString;
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

                Console.WriteLine("Usuario añadido correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir al usuario: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}
