using Aplicacion_Distribuidora.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion_Distribuidora
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        public int PedidoId { get; set; }
        public int ProductoId { get; set; }

        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }
    }
}
