using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Aplicacion_Distribuidora.Data;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion_Distribuidora.Views
{
    public partial class HistorialPedidosWindow : Window
    {
        public HistorialPedidosWindow()
        {
            InitializeComponent();
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .OrderByDescending(p => p.Fecha)
                        .ToList();

                    DgPedidos.ItemsSource = pedidos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pedidos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var query = db.Pedidos
                        .Include(p => p.Cliente)
                        .AsQueryable();

                    string busqueda = TxtBuscarCliente.Text.Trim().ToLower();
                    if (!string.IsNullOrEmpty(busqueda) && busqueda != "buscar por cliente...")
                    {
                        query = query.Where(p =>
                            p.Cliente.Nombre.ToLower().Contains(busqueda) ||
                            p.Cliente.Apellido.ToLower().Contains(busqueda));
                    }

                    var estadoSeleccionado = (CmbFiltroEstado.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (estadoSeleccionado != "Todos")
                    {
                        query = query.Where(p => p.Estado == estadoSeleccionado);
                    }

                    var pedidos = query
                        .OrderByDescending(p => p.Fecha)
                        .ToList();

                    if (pedidos.Count == 0)
                    {
                        MessageBox.Show("No se encontraron pedidos con esos filtros.", "Atención",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    DgPedidos.ItemsSource = pedidos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
