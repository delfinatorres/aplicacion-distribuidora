using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion_Distribuidora.Models
{
    public class Vendedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
    }
}
