using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Aplicacion_Distribuidora.Data;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion_Distribuidora.Views
{
    public partial class ConsultarEstadoPedidoWindow : Window
    {
        public ConsultarEstadoPedidoWindow()
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
                MessageBox.Show("El número de pedido ingresado no es válido.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var pedido = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.Detalles)
                        .ThenInclude(d => d.Producto)
                        .FirstOrDefault(p => p.Id == numeroPedido);

                    if (pedido == null)
                    {
                        MessageBox.Show("El número ingresado no corresponde a ningún pedido.",
                            "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                        PanelInfo.Visibility = Visibility.Collapsed;
                        DgDetalles.Visibility = Visibility.Collapsed;
                        return;
                    }

                    TxtNroPedido.Text = $"Pedido N°: {pedido.Id}";
                    TxtCliente.Text = $"Cliente: {pedido.Cliente.Nombre} {pedido.Cliente.Apellido}";
                    TxtFecha.Text = $"Fecha: {pedido.Fecha:dd/MM/yyyy HH:mm}";
                    TxtFormaPago.Text = $"Forma de Pago: {pedido.FormaDePago}";
                    TxtTipoEntrega.Text = $"Entrega: {pedido.TipoEntrega}";
                    TxtTotal.Text = $"Total: {pedido.Total:C}";

                    TxtEstado.Text = $"Estado: {pedido.Estado}";
                    TxtEstado.Foreground = pedido.Estado switch
                    {
                        "Registrado" => new SolidColorBrush(Colors.Yellow),
                        "EnPreparacion" => new SolidColorBrush(Colors.Orange),
                        "EnDistribucion" => new SolidColorBrush(Colors.LightBlue),
                        "Entregado" => new SolidColorBrush(Colors.LightGreen),
                        "Cancelado" => new SolidColorBrush(Colors.Red),
                        _ => new SolidColorBrush(Colors.White)
                    };

                    DgDetalles.ItemsSource = pedido.Detalles.ToList();

                    PanelInfo.Visibility = Visibility.Visible;
                    DgDetalles.Visibility = Visibility.Visible;
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
    }
}