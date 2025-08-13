namespace Users
{
    public class Usuario
    {
        int Id { get; set; }
        string Nombre { get; set; }

        public void Loggin()
        {
            Console.WriteLine("Nombre de usuario: ");
            Nombre = Console.ReadLine();

            Console.WriteLine("Contraseña:");
            string Contraseña = Console.ReadLine();
        }

    }
}
