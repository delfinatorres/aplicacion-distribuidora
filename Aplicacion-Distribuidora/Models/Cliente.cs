using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Aplicacion_Distribuidora.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string DNI { get; set; }
        public string FormaDePago { get; set; }
        public decimal Saldo { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
        public ICollection<Pago> Pagos { get; set; }
    }
}