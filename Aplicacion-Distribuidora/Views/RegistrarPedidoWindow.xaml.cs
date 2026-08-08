using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Aplicacion_Distribuidora.Data;
using Aplicacion_Distribuidora.Models;

namespace Aplicacion_Distribuidora.Views
{
    public partial class RegistrarPedidoWindow : Window
    {
        private Models.Cliente _clienteSeleccionado;
        private List<DetallePedidoTemp> _detalles = new List<DetallePedidoTemp>();

        public RegistrarPedidoWindow()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void CargarProductos()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var productos = db.Productos
                        .Where(p => p.Activo == true && p.Stock > 0)
                        .ToList();
                    CmbProductos.ItemsSource = productos;
                    CmbProductos.DisplayMemberPath = "Nombre";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtBuscarCliente.Text))
            {
                MessageBox.Show("Ingresá el nombre o apellido del cliente.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    string busqueda = TxtBuscarCliente.Text.Trim().ToLower();

                    var cliente = db.Clientes.FirstOrDefault(c =>
                        c.Nombre.ToLower().Contains(busqueda) ||
                        c.Apellido.ToLower().Contains(busqueda));

                    if (cliente == null)
                    {
                        MessageBox.Show("El cliente no se encuentra registrado. Debes registrarlo antes de continuar.",
                            "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (cliente.FormaDePago == "CuentaCorriente" && cliente.Saldo > 0)
                    {
                        var resultado = MessageBox.Show(
                            $"El cliente tiene un saldo pendiente de ${cliente.Saldo}. ¿Desea continuar?",
                            "Saldo Pendiente",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (resultado == MessageBoxResult.No)
                            return;
                    }

                    _clienteSeleccionado = cliente;
                    TxtInfoCliente.Text = $"Cliente: {cliente.Nombre} {cliente.Apellido} | Tipo: {cliente.FormaDePago}";
                    TxtInfoCliente.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar cliente: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (CmbProductos.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un producto.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingresá una cantidad válida.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var producto = (Producto)CmbProductos.SelectedItem;

            if (cantidad > producto.Stock)
            {
                MessageBox.Show($"No hay suficiente stock. Stock disponible: {producto.Stock}",
                    "Sin Stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var detalle = new DetallePedidoTemp
            {
                ProductoId = producto.Id,
                NombreProducto = producto.Nombre,
                Cantidad = cantidad,
                PrecioUnitario = producto.Precio,
                Subtotal = producto.Precio * cantidad
            };

            _detalles.Add(detalle);
            DgProductosPedido.ItemsSource = null;
            DgProductosPedido.ItemsSource = _detalles;

            decimal total = _detalles.Sum(d => d.Subtotal);
            TxtTotal.Text = $"Total: {total:C}";
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSeleccionado == null)
            {
                MessageBox.Show("Buscá y seleccioná un cliente.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_detalles.Count == 0)
            {
                MessageBox.Show("Agregá al menos un producto al pedido.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CmbFormaPago.SelectedItem == null || CmbTipoEntrega.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná la forma de pago y el tipo de entrega.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    decimal total = _detalles.Sum(d => d.Subtotal);

                    var pedido = new Pedido
                    {
                        Fecha = DateTime.Now,
                        ClienteId = _clienteSeleccionado.Id,
                        FormaDePago = (CmbFormaPago.SelectedItem as ComboBoxItem).Content.ToString(),
                        TipoEntrega = (CmbTipoEntrega.SelectedItem as ComboBoxItem).Content.ToString(),
                        Estado = "Registrado",
                        Total = total,
                        VendedorId = 1
                    };

                    db.Pedidos.Add(pedido);
                    db.SaveChanges();

                    foreach (var detalle in _detalles)
                    {
                        var detallePedido = new DetallePedido
                        {
                            PedidoId = pedido.Id,
                            ProductoId = detalle.ProductoId,
                            Cantidad = detalle.Cantidad,
                            PrecioUnitario = detalle.PrecioUnitario,
                            Subtotal = detalle.Subtotal
                        };
                        db.DetallesPedido.Add(detallePedido);

                        var producto = db.Productos.Find(detalle.ProductoId);
                        if (producto != null)
                            producto.Stock -= detalle.Cantidad;
                    }

                    if (_clienteSeleccionado.FormaDePago == "CuentaCorriente")
                    {
                        var cliente = db.Clientes.Find(_clienteSeleccionado.Id);
                        if (cliente != null)
                            cliente.Saldo += total;
                    }

                    db.SaveChanges();

                    MessageBox.Show($"Pedido registrado correctamente. N° de Pedido: {pedido.Id}",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string mensajeCompleto = ex.Message;
                if (ex.InnerException != null)
                    mensajeCompleto += "\n\nDetalle: " + ex.InnerException.Message;

                MessageBox.Show(mensajeCompleto, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class DetallePedidoTemp
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}