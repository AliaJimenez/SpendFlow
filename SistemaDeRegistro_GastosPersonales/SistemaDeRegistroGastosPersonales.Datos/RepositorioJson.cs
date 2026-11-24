using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SistemaGastos.Entidades;

namespace SistemaGastos.Datos
{
    public class RepositorioJson<T>
    {
        private readonly string _rutaArchivo;

        public RepositorioJson(string nombreArchivo)
        {
            // se guarda en la ruta base de la aplicación
            _rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreArchivo);
        }

        public List<T> Cargar()
        {
            if (!File.Exists(_rutaArchivo))
            {
                return new List<T>();
            }

            try
            {
                string json = File.ReadAllText(_rutaArchivo);
                if (string.IsNullOrWhiteSpace(json)) return new List<T>();
                
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (Exception)
            {
                // si el archivo está corrupto o hay error, retornamos lista vacía o lanzamos excepción según política
                return new List<T>();
            }
        }

        public void Guardar(List<T> datos)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(datos, opciones);
            File.WriteAllText(_rutaArchivo, json);
        }
    }
}