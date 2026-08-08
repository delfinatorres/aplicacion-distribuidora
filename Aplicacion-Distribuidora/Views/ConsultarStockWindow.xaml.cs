using System;
using System.Linq;
using System.Windows;
using Aplicacion_Distribuidora.Data;

namespace Aplicacion_Distribuidora.Views
{
    public partial class ConsultarStockWindow : Window
    {
        public ConsultarStockWindow()
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
                        .Where(p => p.Activo == true)
                        .ToList();

                    DgProductos.ItemsSource = productos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    string busqueda = TxtBuscar.Text.Trim().ToLower();

                    var productos = db.Productos
                        .Where(p => p.Activo == true &&
                            (p.Nombre.ToLower().Contains(busqueda) ||
                             p.Codigo.ToLower().Contains(busqueda)))
                        .ToList();

                    if (productos.Count == 0)
                    {
                        MessageBox.Show("No se encontraron productos.", "Atención",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    DgProductos.ItemsSource = productos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}