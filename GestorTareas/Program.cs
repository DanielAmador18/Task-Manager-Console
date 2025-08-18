using Gestor;
using GestorU;
using Users;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {

            GestorTareas gestion = new GestorTareas();
            gestion.EjecutarOpcion();
        }
    }
}