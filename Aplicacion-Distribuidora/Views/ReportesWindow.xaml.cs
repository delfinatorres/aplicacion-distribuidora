using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Aplicacion_Distribuidora.Data;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion_Distribuidora.Views
{
    public partial class ReportesWindow : Window
    {
        public ReportesWindow()
        {
            InitializeComponent();
            CargarReporte();
        }

        private void CargarReporte()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .OrderByDescending(p => p.Fecha)
                        .ToList();

                    DgReporte.ItemsSource = pedidos;

                    TxtTotalPedidos.Text = pedidos.Count.ToString();
                    TxtTotalVentas.Text = $"{pedidos.Sum(p => p.Total):C}";

                    var totalDeuda = db.Clientes
                        .Where(c => c.FormaDePago == "CuentaCorriente" && c.Saldo > 0)
                        .Sum(c => c.Saldo);
                    TxtTotalDeuda.Text = $"{totalDeuda:C}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar reporte: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGenerar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var query = db.Pedidos
                        .Include(p => p.Cliente)
                        .AsQueryable();

                    if (DtpDesde.SelectedDate.HasValue)
                        query = query.Where(p => p.Fecha >= DtpDesde.SelectedDate.Value);

                    if (DtpHasta.SelectedDate.HasValue)
                        query = query.Where(p => p.Fecha <= DtpHasta.SelectedDate.Value.AddDays(1));

                    var estadoSeleccionado = (CmbEstado.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (estadoSeleccionado != "Todos")
                        query = query.Where(p => p.Estado == estadoSeleccionado);

                    var pedidos = query
                        .OrderByDescending(p => p.Fecha)
                        .ToList();

                    DgReporte.ItemsSource = pedidos;

                    TxtTotalPedidos.Text = pedidos.Count.ToString();
                    TxtTotalVentas.Text = $"{pedidos.Sum(p => p.Total):C}";

                    var totalDeuda = db.Clientes
                        .Where(c => c.FormaDePago == "CuentaCorriente" && c.Saldo > 0)
                        .Sum(c => c.Saldo);
                    TxtTotalDeuda.Text = $"{totalDeuda:C}";

                    if (pedidos.Count == 0)
                        MessageBox.Show("No se encontraron pedidos con esos filtros.", "Atención",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
