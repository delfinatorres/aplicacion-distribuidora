using System.Windows;

namespace Aplicacion_Distribuidora.Views
{
    public partial class PedidosMenuWindow : Window
    {
        public PedidosMenuWindow()
        {
            InitializeComponent();
        }

        private void BtnRegistrarPedido_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new RegistrarPedidoWindow();
            ventana.ShowDialog();
        }

        private void BtnConsultarEstado_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ConsultarEstadoPedidoWindow();
            ventana.ShowDialog();
        }

        private void BtnHistorial_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new HistorialPedidosWindow();
            ventana.ShowDialog();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}