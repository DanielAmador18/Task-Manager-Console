using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;
using Gestor;
using ClaseTarea;
using Microsoft.Extensions.Configuration;

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