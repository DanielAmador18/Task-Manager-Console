using Gestor;
using GestorU;
using Users;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            GestorUsuarios user = new GestorUsuarios();
            user.Inicio();
        }
    }
}