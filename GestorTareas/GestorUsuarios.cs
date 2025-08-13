using Users;
namespace GestorU
{
    public class GestorUsuarios
    {
        public string ValidarNombre()
        {
            do
            {
                
                Console.WriteLine("Ingrese su Nombre de usuario:");
                string Nombre = Console.ReadLine();

                if (string.IsNullOrEmpty(Nombre))
                {
                    return "El nombre no puede estar vacio";
                }
                    
                Console.ReadKey();
            
            }while (true);
        }
    }
}
