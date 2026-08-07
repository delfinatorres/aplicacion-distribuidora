using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion_Distribuidora.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string FormaDePago { get; set; }
        public string TipoEntrega { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }

        public int ClienteId { get; set; }
        public int VendedorId { get; set; }

        public Cliente Cliente { get; set; }
        public Vendedor Vendedor { get; set; }
        public ICollection<DetallePedido> Detalles { get; set; }
        public Boleta Boleta { get; set; }
        public Entrega Entrega { get; set; }
    }
}
