using System.ComponentModel;

namespace Users
{
    public class Usuario
    {
        int Id { get; set; }
        string Nombre { get; set; }

        public void Loggin()
        {
            ValidarNombre();
            
            ValidarContraseña();
        }

        public string ValidarNombre()
        {
            do
            {
                Console.WriteLine("Ingrese su nombre de usuario:");
                Nombre = Console.ReadLine();

                if(string.IsNullOrEmpty(Nombre))
                {
                    Console.WriteLine("El nombre no puede estar vacio, intente nuevamente");
                }

            } while (string.IsNullOrEmpty(Nombre));

            return Nombre;
        }

        public string ValidarContraseña()
        {
            string Contraseña;
            do
            {
                Console.WriteLine("Ingrese su contraseña:");
                Contraseña = Console.ReadLine();

                if (string.IsNullOrEmpty(Contraseña))
                {
                    Console.WriteLine("El nombre no puede estar vacio, intente nuevamente");
                }

            } while (string.IsNullOrEmpty(Contraseña));

            return Contraseña;
        }

    }
}
