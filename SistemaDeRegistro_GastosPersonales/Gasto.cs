using System;

namespace SistemaGastos.Entidades
{
    public class Gasto
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public decimal Monto { get; set; }
        public string ? Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        public override string ToString()
        {
            return $"{Fecha:dd/MM/yyyy} - {Descripcion}: {Monto:C}";
        }
    }
}