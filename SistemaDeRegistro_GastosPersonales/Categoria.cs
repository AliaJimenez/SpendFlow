using System;

namespace SistemaGastos.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }
        public string ? Nombre { get; set; }
        public decimal PresupuestoMensual { get; set; }

        public override string ToString()
        {
            return $"{Id}. {Nombre} - Presupuesto: {PresupuestoMensual:C}";
        }
    }
}