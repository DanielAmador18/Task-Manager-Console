using GestorU;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            GestorUsuarios user = new GestorUsuarios();
            user.Inicio();   //Instancia que llama al metodo para iniciar el programa
        }
    }
}