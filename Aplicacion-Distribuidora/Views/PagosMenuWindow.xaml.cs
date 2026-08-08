using System.Windows;

namespace Aplicacion_Distribuidora.Views
{
    public partial class PagosMenuWindow : Window
    {
        public PagosMenuWindow()
        {
            InitializeComponent();
        }

        private void BtnRegistrarPago_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new RegistrarPagoWindow();
            ventana.ShowDialog();
        }

        private void BtnCuentasCorrientes_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ConsultarCuentasCorrientesWindow();
            ventana.ShowDialog();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}