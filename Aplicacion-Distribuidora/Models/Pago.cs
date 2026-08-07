using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion_Distribuidora.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public decimal Importe { get; set; }
        public DateTime Fecha { get; set; }
        public string MetodoPago { get; set; }

        public int ClienteId { get; set; }
        public int PedidoId { get; set; }

        public Cliente Cliente { get; set; }
        public Pedido Pedido { get; set; }
    }
}
