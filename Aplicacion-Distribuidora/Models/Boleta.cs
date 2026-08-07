using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion_Distribuidora.Models
{
    public class Boleta
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public bool Pagada { get; set; }

        public int PedidoId { get; set; }

        public Pedido Pedido { get; set; }
    }
}
