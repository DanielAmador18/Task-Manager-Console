using Microsoft.Data.SqlClient;

namespace ClaseTarea
{
    public class Tarea
    {

        //Propiedades por defecto de una "Tarea"
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public bool Completada { get; set; } = false;


        //Metodos "virtuales" que puede ser sobreescritos (override) por las clases hijas
        public virtual string MostrarInfo()
        {
            string estado = Completada ? "[*]" : "[ ]";
            return $"{estado}.- {Descripcion}";
        }

        public virtual void MarcarCompletada()
        {
            Completada = true;
            Console.WriteLine("Tarea marcada como completada");
        }

    }

    public class TareaPersonal : Tarea                 //Clase heredada de Tarea
    {

        public override string MostrarInfo()            //Permite llamar al metodo y sobreescribirlo
        {
            string estado = Completada ? "[*]" : "[ ]";
            return $"{estado}.- {Descripcion}";
        }

        public override void MarcarCompletada()
        {
            Completada = true;
            Console.WriteLine("Tarea personal marcada como completada");
        }

    }

    public class TareaTrabajo : Tarea
    {
        public int Prioridad { get; set; } = 1;


        public override string MostrarInfo()
        {
            string estado = Completada ? "[*]" : "[ ]";
            string prioridadTexto = "";

            if (Prioridad == 3)
            {
                prioridadTexto = "ALTA";
            }
            else if (Prioridad == 2)
            {
                prioridadTexto = "MEDIA";
            }
            else if (Prioridad == 1)
            {
                prioridadTexto = "BAJA";
            }
            else
            {
                Console.WriteLine("No hay prioridad Ingresada.");
            }

            return $"{estado}, [Prioridad :{prioridadTexto}] {Descripcion}";
        }


        public override void MarcarCompletada()
        {
            Completada = true;
            if (Prioridad == 3)
            {
                Console.WriteLine("Tarea de alta prioridad marcada como completada.");
            }
            else
            {
                Console.WriteLine("Tarea de trabajo marcada como completada.");
            }
        }
    }

    public class TareaFactory
    {
        public static Tarea CrearTarea(string tipo, SqlDataReader reader)
        {
            return tipo switch
            {
                "Personal" => new TareaPersonal(),
                "Trabajo" => new TareaTrabajo
                {
                    Prioridad = reader["Prioridad"] != DBNull.Value ? (int)reader["Prioridad"] : 1  //Lee la prioridad otorgada, si es Null
                },
                _ => throw new ArgumentException($"Tipo de tarea '{tipo}' no valido.")
            };
        }
    }
}
