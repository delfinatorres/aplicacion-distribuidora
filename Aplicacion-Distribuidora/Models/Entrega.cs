using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion_Distribuidora.Models
{
    public class Entrega
    {
        public int Id { get; set; }
        public string Direccion { get; set; }
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; }

        public int PedidoId { get; set; }

        public Pedido Pedido { get; set; }
    }
}
