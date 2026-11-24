using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using SistemaGastos.Entidades;
using SistemaGastos.Datos;

namespace SistemaGastos.Negocio
{
    public class GestorFinanzas
    {
        private RepositorioJson<Categoria> _repoCategorias;
        private RepositorioJson<Gasto> _repoGastos;
        private List<Categoria> _categorias;
        private List<Gasto> _gastos;

        public GestorFinanzas()
        {
            _repoCategorias = new RepositorioJson<Categoria>("categorias.json");
            _repoGastos = new RepositorioJson<Gasto>("gastos.json");
            
            // Cargar datos al iniciar
            _categorias = _repoCategorias.Cargar();
            _gastos = _repoGastos.Cargar();
        }

#region Gestión de Categorías
        public void AgregarCategoria(string nombre, decimal presupuesto)
        {
            if (_categorias.Any(c => c.Nombre?.Equals(nombre, StringComparison.OrdinalIgnoreCase) == true))
                throw new Exception("Ya existe una categoría con ese nombre.");

            if (presupuesto <= 0)
                throw new Exception("El presupuesto debe ser mayor a 0.");

            int nuevoId = _categorias.Count > 0 ? _categorias.Max(c => c.Id) + 1 : 1;

            _categorias.Add(new Categoria { Id = nuevoId, Nombre = nombre, PresupuestoMensual = presupuesto });
            _repoCategorias.Guardar(_categorias);
        }

        public List<Categoria> ListarCategorias()
        {
            return _categorias;
        }

        public void EliminarCategoria(int id)
        {
            // integridad referencial
            if (_gastos.Any(g => g.CategoriaId == id))
                throw new Exception("No se puede eliminar la categoría porque tiene gastos asociados.");

            var cat = _categorias.FirstOrDefault(c => c.Id == id);
            if (cat == null) throw new Exception("Categoría no encontrada.");

            _categorias.Remove(cat);
            _repoCategorias.Guardar(_categorias);
        }
#endregion

#region Gestión de Gastos,
        public string RegistrarGasto(decimal monto, int categoriaId, string descripcion)
        {
            if (monto <= 0) throw new Exception("El monto debe ser positivo.");
            if (string.IsNullOrWhiteSpace(descripcion)) throw new Exception("La descripción es obligatoria.");
            
            var categoria = _categorias.FirstOrDefault(c => c.Id == categoriaId);
            if (categoria == null) throw new Exception("La categoría seleccionada no existe.");

            int nuevoId = _gastos.Count > 0 ? _gastos.Max(g => g.Id) + 1 : 1;

            Gasto nuevoGasto = new Gasto
            {
                Id = nuevoId,
                CategoriaId = categoriaId,
                Monto = monto,
                Descripcion = descripcion,
                Fecha = DateTime.Now
            };

            _gastos.Add(nuevoGasto);
            _repoGastos.Guardar(_gastos);

            // control de Presupuesto
            return VerificarPresupuesto(categoriaId);
        }

        private string VerificarPresupuesto(int categoriaId)
        {
            var categoria = _categorias.First(c => c.Id == categoriaId);
            DateTime hoy = DateTime.Now;

            // sumar gastos de esta categoría en el mes actual
            decimal totalMes = _gastos
                .Where(g => g.CategoriaId == categoriaId && g.Fecha.Month == hoy.Month && g.Fecha.Year == hoy.Year)
                .Sum(g => g.Monto);

            if (totalMes > categoria.PresupuestoMensual)
            {
                return $"ADVERTENCIA! Has superado el presupuesto de {categoria.Nombre}. " + $"Presupuesto: {categoria.PresupuestoMensual:C}, Gastado: {totalMes:C}";
            }
            return null;
        }

        public List<Gasto> ListarGastos()
        {
            return _gastos;
        }

        public List<Gasto> BuscarGastosPorCategoria(int categoriaId)
        {
            return _gastos.Where(g => g.CategoriaId == categoriaId).ToList();
        }
        #endregion

        #region Reportes y Exportación
        public Dictionary<string, object> GenerarResumenPantalla()
        {
            var resumen = new Dictionary<string, object>();
            
            decimal totalGeneral = _gastos.Sum(g => g.Monto);
            resumen["TotalGeneral"] = totalGeneral;

            // Totales por categoría
            var porCategoria = _gastos
                .GroupBy(g => g.CategoriaId)
                .Select(grupo => {
                    var cat = _categorias.FirstOrDefault(c => c.Id == grupo.Key);
                    string nombreCat = cat != null ? cat.Nombre : "Desconocida";
                    decimal totalCat = grupo.Sum(g => g.Monto);
                    double porcentaje = totalGeneral > 0 ? (double)(totalCat / totalGeneral * 100) : 0;
                    return new { Categoria = nombreCat, Total = totalCat, Porcentaje = porcentaje };
                }).ToList();

            resumen["PorCategoria"] = porCategoria;

            // Totales por Mes/Año
            var porMes = _gastos
                .GroupBy(g => new { g.Fecha.Year, g.Fecha.Month })
                .Select(grupo => new { 
                    Fecha = $"{grupo.Key.Month}/{grupo.Key.Year}", 
                    Total = grupo.Sum(g => g.Monto) 
                }).ToList();

            resumen["PorMes"] = porMes;

            return resumen;
        }

        public void ExportarResumenMensual(int mes, int anio)
        {
            var gastosMes = _gastos
                .Where(g => g.Fecha.Month == mes && g.Fecha.Year == anio)
                .ToList();

            if (gastosMes.Count == 0) throw new Exception("No hay gastos registrados para el mes seleccionado.");

            decimal total = gastosMes.Sum(g => g.Monto);
            
            var detalleCategorias = gastosMes
                .GroupBy(g => g.CategoriaId)
                .Select(grupo => {
                    var cat = _categorias.FirstOrDefault(c => c.Id == grupo.Key);
                    return new {
                        Categoria = cat?.Nombre ?? "Desconocida",
                        Total = grupo.Sum(g => g.Monto)
                    };
                });

            var objetoExportar = new
            {
                Periodo = $"{mes}/{anio}",
                TotalGastado = total,
                DesglosePorCategoria = detalleCategorias,
                DetalleGastos = gastosMes.Select(g => new { g.Fecha, g.Descripcion, g.Monto })
            };

            string nombreArchivo = $"resumen_{anio}_{mes:D2}.json";
            string rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreArchivo);

            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(objetoExportar, opciones);
            
            File.WriteAllText(rutaCompleta, json);
        }
        #endregion
    }
}