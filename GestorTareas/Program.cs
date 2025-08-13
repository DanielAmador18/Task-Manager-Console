using Gestor;
using GestorU;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            GestorUsuarios usuario = new GestorUsuarios();
            usuario.ValidarNombre();

            GestorTareas gestion = new GestorTareas();
            gestion.EjecutarOpcion();
        }
    }
}