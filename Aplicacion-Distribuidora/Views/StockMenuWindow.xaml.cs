using System.Windows;

namespace Aplicacion_Distribuidora.Views
{
    public partial class StockMenuWindow : Window
    {
        public StockMenuWindow()
        {
            InitializeComponent();
        }

        private void BtnRegistrarProducto_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new RegistrarProductoWindow();
            ventana.ShowDialog();
        }

        private void BtnConsultarStock_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ConsultarStockWindow();
            ventana.ShowDialog();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}