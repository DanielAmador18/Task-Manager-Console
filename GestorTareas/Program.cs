using System.Collections.Generic;
using System;

namespace Listas
{
    class Program
    {
        static void Main(string[] args)
        {
            GestorTareas gestion = new GestorTareas();

            gestion.EjecutarOpcion();
        }
    }

    public class Tarea
    {
        public string Descripcion { get; set; }

        public bool Completada { get; set; } = false;

        public virtual string MostrarInfo()
        {
            string estado = Completada ? "[*]" : "[ ]";
            return $"{estado}.- {Descripcion}";
        }

        public virtual void MarcarCompletada()
        {
            Completada = true;
            Console.WriteLine("Tare marcada como completada");
        }

    }

    public class TareaPersonal : Tarea                  //Clase heredada de Tarea
    {

        public override string MostrarInfo()            //Permite llamar al metodo y sobreescribirlo
        {
            string estado = Completada ? "[*]" : "[ ]";
            return $"{estado}.- {Descripcion}";
        }

        public override void MarcarCompletada()
        {
            Completada = true;
            Console.WriteLine("Tare personal marcada como completada");
        }

    }

    public class GestorTareas
    {
        List<Tarea> tareas = new List<Tarea>();    //Lista de objetos usando la clase Tarea
        
        public void MostrarMenu()
        {
            Console.WriteLine();
            Console.WriteLine("====BIENVENIDO====");
            Console.WriteLine("1.-Añadir tarea personal.");
            Console.WriteLine("2.-Mostrar tareas actuales.");
            Console.WriteLine("3.-Eliminar una tarea por numero.");
            Console.WriteLine("4.-Marcar como completada");
            Console.WriteLine("5.-Salir del programa.");
        }

        public void EjecutarOpcion()
        {
            bool seguir = true;  //Declarar una variable como true para realizar la continuacion
                                 //del bucle y guardar en ella valores true o false del metodo
                                 //GestionarOpcion

            while (seguir)
            {
                Console.Clear();
                MostrarMenu();

                Console.WriteLine("Ingrese una opcion:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int opc)) //Validar desde input y guardar valor en opc
                {
                    seguir = GestionarOpcion(opc);
                }
                else
                {
                    Console.WriteLine("Por favor ingresa un numero valido.");
                }
            }
        }

        public bool GestionarOpcion(int opc)
        {
            switch (opc)
            {
                case 1: AñadirTarea(); break;

                case 2: MostrarTareas(); break;

                case 3: EliminarTarea(); break;

                case 4: MarcarCompletada(); break;

                case 5:
                    Salir();
                    return false;
            }

            return true; //Se devuelve true para que el bucle siga funcionando
        }

        public void AñadirTarea()
        {
            Console.WriteLine("Ingresa la nueva tarea:");
            string desc = Console.ReadLine();
            tareas.Add(new TareaPersonal { Descripcion = desc });
            Console.WriteLine("Tarea añadida correctamente.");
            Console.ReadKey();
        }

        public void MostrarTareas()
        {
            if (tareas.Count == 0)
            {
                Console.WriteLine("Aun no hay tareas registradas.");
                return;

            }
            else
            {
                Console.WriteLine("Tareas actuales:");
                for (int i = 0; i < tareas.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tareas[i].MostrarInfo()}"); //Llama al metodo MostrarInfo sobreescrito dependiendo del tipo de objeto
                }
            }
            Console.ReadKey();
        }

        public void EliminarTarea()
        {
            if (tareas.Count == 0)
            {
                Console.WriteLine("Aun no hay tareas registradas.");
                return;

            }
            Console.WriteLine("Ingrese el numero de la tarea que desee eliminar.");

            for (int i = 0; i < tareas.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tareas[i].Descripcion}");
            }

            string input = Console.ReadLine();

            if (int.TryParse(input, out int indice))
            {
                indice--;
                if (indice >= 0 && indice < tareas.Count)
                {
                    tareas.RemoveAt(indice);
                    Console.WriteLine("Tarea eliminada correctamente");
                }
                else
                {
                    Console.WriteLine("Indice fuera del rango");
                }
            }
            else
            {
                Console.WriteLine("Entrada no valida");
            }

            Console.ReadKey();
        }

        //Metodo que marca una tarea como completada
        public void MarcarCompletada()
        {
            if (tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas para marcar como completadas.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < tareas.Count; i++)
            {
                string estado = tareas[i].Completada ? "[*]" : "[ ]";
                Console.WriteLine($"{i + 1}.- {estado} {tareas[i].Descripcion}");
            }

            Console.WriteLine("Ingrese el número de la tarea que desea marcar como completada:");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int indice))
            {
                indice--; // Ajustar al índice real
                if (indice >= 0 && indice < tareas.Count)
                {
                    tareas[indice].MarcarCompletada();
                    Console.WriteLine("Tarea marcada como completada.");
                }
                else
                {
                    Console.WriteLine("Índice fuera de rango.");
                }
            }
            else
            {
                Console.WriteLine("Entrada no válida.");
            }

            Console.ReadKey();
        }


        public void Salir()
        {
            Console.WriteLine("Gracias por usar el programa");
            System.Environment.Exit(0);
            Console.ReadKey();

        }
    }

}