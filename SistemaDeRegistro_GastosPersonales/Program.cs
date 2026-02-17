using System;
using System.Collections.Generic;
using Sistema
namespace SistemaGastos.Presentacion

    {
        static GestorFinanzas _gestor = new GestorFinanzas();

        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== SISTEMA DE GASTOS PERSONALES ===");
                Console.WriteLine("1. Registrar Gasto");
                Console.WriteLine("2. Listar Gastos");
                Console.WriteLine("3. Gestión de Categorías");
                Console.WriteLine("4. Ver Reportes / Resumen");
                Console.WriteLine("5. Exportar Resumen Mensual (JSON)");
                Console.WriteLine("6. Salir");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1": 
                        RegistrarGastoUI(); 
                        break;
                    case "2": 
                        ListarGastosUI(); 
                        break;
                    case "3": 
                        MenuCategorias(); 
                        break;
                    case "4": 
                        MostrarReportesUI(); 
                        break;
                    case "5": 
                        ExportarResumenUI(); 
                        break;
                    case "6": 
                        Console.WriteLine("Saliendo del programa...");
                        salir = true; 
                        break;
                    default: 
                        Console.WriteLine("Opción inválida.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void RegistrarGastoUI()
        {
            Console.Clear();
            Console.WriteLine("--- NUEVO GASTO ---");
            
            var categorias = _gestor.ListarCategorias();
            if (categorias.Count == 0)
            {
                Mensaje("Debe crear categorías antes de registrar gastos.");
                return;
            }

            Console.WriteLine("Categorías disponibles:");
            foreach (var c in categorias) Console.WriteLine(c.ToString());

            try
            {
                Console.Write("\nID de Categoría: ");
                int catId = int.Parse(Console.ReadLine()!);

                Console.Write("Descripción: ");
                string desc = Console.ReadLine()!;

                Console.Write("Monto: ");
                decimal monto = decimal.Parse(Console.ReadLine()!);

                string advertencia = _gestor.RegistrarGasto(monto, catId, desc);
                
                Console.WriteLine("\nGasto registrado exitosamente!");
                
                if (!string.IsNullOrEmpty(advertencia))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(advertencia);
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Mensaje($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        static void ListarGastosUI()
        {
            Console.Clear();
            Console.WriteLine("--- LISTADO DE GASTOS ---");
            var gastos = _gestor.ListarGastos();
            var categorias = _gestor.ListarCategorias();

            if (gastos.Count == 0) Console.WriteLine("No hay gastos registrados.");
            
            foreach (var g in gastos)
            {
                var catNombre = categorias.Find(c => c.Id == g.CategoriaId)?.Nombre ?? "Sin Cat";
                Console.WriteLine($"[{catNombre}] {g.ToString()}");
            }
            Console.ReadKey();
        }

        static void MenuCategorias()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("--- GESTIÓN DE CATEGORÍAS ---");
                Console.WriteLine("1. Listar Categorías");
                Console.WriteLine("2. Agregar Categoría");
                Console.WriteLine("3. Eliminar Categoría");
                Console.WriteLine("4. Volver");
                Console.Write("Opción: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("\nListado:");
                        foreach (var c in _gestor.ListarCategorias()) Console.WriteLine(c);
                        Console.ReadKey();
                        break;
                    case "2":
                        try {
                            Console.Write("Nombre: ");
                            string nom = Console.ReadLine()!;
                            Console.Write("Presupuesto Mensual: ");
                            decimal pre = decimal.Parse(Console.ReadLine()!);
                            _gestor.AgregarCategoria(nom, pre);
                            Mensaje("Categoría agregada.");
                        } catch (Exception ex) { Mensaje("Error: " + ex.Message); }
                        break;
                    case "3":
                        try {
                            Console.Write("ID a eliminar: ");
                            int id = int.Parse(Console.ReadLine()!);
                            _gestor.EliminarCategoria(id);
                            Mensaje("Categoría eliminada.");
                        } catch (Exception ex) { Mensaje("Error: " + ex.Message); }
                        break;
                    case "4": 
                        volver = true; 
                        break;
                    default: 
                        Mensaje("Opción inválida.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void MostrarReportesUI()
        {
            Console.Clear();
            Console.WriteLine("--- REPORTES Y ESTADÍSTICAS ---");
            
            var datos = _gestor.GenerarResumenPantalla();
            
            Console.WriteLine($"\nTOTAL GENERAL GASTADO: {datos["TotalGeneral"]:C}\n");

            Console.WriteLine("--- Por Categoría ---");
            var listaCat = (dynamic)datos["PorCategoria"];
            foreach (var item in listaCat)
            {
                Console.WriteLine($"{item.Categoria}: {item.Total:C} ({item.Porcentaje:F2}%)");
            }

            Console.WriteLine("\n--- Por Mes ---");
            var listaMes = (dynamic)datos["PorMes"];
            foreach (var item in listaMes)
            {
                Console.WriteLine($"{item.Fecha}: {item.Total:C}");
            }

            Console.ReadKey();
        }

        static void ExportarResumenUI()
        {
            Console.Clear();
            Console.WriteLine("--- EXPORTAR RESUMEN MENSUAL ---");
            try
            {
                Console.Write("Ingrese el Mes (1-12): ");
                int mes = int.Parse(Console.ReadLine()!);
                Console.Write("Ingrese el Año (ej. 2025): ");
                int anio = int.Parse(Console.ReadLine()!);

                _gestor.ExportarResumenMensual(mes, anio);
                Mensaje($"Archivo 'resumen_{anio}_{mes:D2}.json' generado correctamente en la carpeta del proyecto.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Mensaje("Error al exportar: " + ex.Message);
            }
        }

        static void Mensaje(string txt)
        {
            Console.WriteLine("\n" + txt);
            Console.WriteLine("Presione cualquier tecla...");
            Console.ReadKey();
        }
    }
}
