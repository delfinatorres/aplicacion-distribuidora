using System.Windows;
using Aplicacion_Distribuidora.Models;
using Aplicacion_Distribuidora.Views;

namespace Aplicacion_Distribuidora
{
    public partial class MainWindow : Window
    {
        private Vendedor _vendedorActual;

        public MainWindow(Vendedor vendedor)
        {
            InitializeComponent();
            _vendedorActual = vendedor;
            TxtBienvenida.Text = $"Bienvenido, {vendedor.Nombre} {vendedor.Apellido}";

            if (vendedor.Rol == "Vendedor")
            {
                BtnReportes.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new RegistrarClienteWindow();
            ventana.ShowDialog();
        }

        private void BtnPedidos_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new PedidosMenuWindow();
            ventana.ShowDialog();
        }

        private void BtnStock_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new StockMenuWindow();
            ventana.ShowDialog();
        }

        private void BtnPagos_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new PagosMenuWindow();
            ventana.ShowDialog();
        }

        private void BtnReportes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Módulo de Reportes en construcción.");
        }
    }
}