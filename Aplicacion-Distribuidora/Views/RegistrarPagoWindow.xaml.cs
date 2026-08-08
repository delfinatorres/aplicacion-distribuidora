using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Aplicacion_Distribuidora.Data;
using Aplicacion_Distribuidora.Models;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion_Distribuidora.Views
{
    public partial class RegistrarPagoWindow : Window
    {
        private Pedido _pedidoSeleccionado;

        public RegistrarPagoWindow()
        {
            InitializeComponent();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNumeroPedido.Text))
            {
                MessageBox.Show("Ingresá el número de pedido.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtNumeroPedido.Text, out int numeroPedido))
            {
                MessageBox.Show("El número de pedido no es válido.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var pedido = db.Pedidos
                        .Include(p => p.Cliente)
                        .FirstOrDefault(p => p.Id == numeroPedido);

                    if (pedido == null)
                    {
                        MessageBox.Show("No hay ningún pedido asociado. Verificá que el número sea correcto.",
                            "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                        PanelInfo.Visibility = Visibility.Collapsed;
                        return;
                    }

                    if (pedido.Estado == "Pagado")
                    {
                        MessageBox.Show("El pedido ya fue abonado. No hay saldo restante.",
                            "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                        PanelInfo.Visibility = Visibility.Collapsed;
                        return;
                    }

                    _pedidoSeleccionado = pedido;
                    TxtInfoCliente.Text = $"Cliente: {pedido.Cliente.Nombre} {pedido.Cliente.Apellido}";
                    TxtInfoTotal.Text = $"Monto Total: {pedido.Total:C}";
                    PanelInfo.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar pedido: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (_pedidoSeleccionado == null)
            {
                MessageBox.Show("Buscá un pedido primero.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CmbMetodoPago.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná el método de pago.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var pedido = db.Pedidos
                        .Include(p => p.Cliente)
                        .FirstOrDefault(p => p.Id == _pedidoSeleccionado.Id);

                    var pago = new Pago
                    {
                        Importe = pedido.Total,
                        Fecha = DateTime.Now,
                        MetodoPago = (CmbMetodoPago.SelectedItem as ComboBoxItem).Content.ToString(),
                        ClienteId = pedido.ClienteId,
                        PedidoId = pedido.Id
                    };

                    db.Pagos.Add(pago);

                    pedido.Estado = "Pagado";

                    if (pedido.Cliente.FormaDePago == "CuentaCorriente")
                    {
                        var cliente = db.Clientes.Find(pedido.ClienteId);
                        if (cliente != null)
                            cliente.Saldo -= pedido.Total;
                    }

                    db.SaveChanges();

                    MessageBox.Show($"Pago registrado correctamente.\nMonto: {pedido.Total:C}\nMétodo: {pago.MetodoPago}",
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
}