using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users
{
    public class Usuario
    {
        int Id { get; set; }
        string Nombre { get; set; }
        string Contraseña { get; set; }


        public void Loggin()
        {
            Console.WriteLine("Nombre de usuario: ");
            Nombre = Console.ReadLine();

            Console.WriteLine("Contraseña:");
            Contraseña = Console.ReadLine();
        }

    }
}
