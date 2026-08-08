using System;
using System.Linq;
using System.Windows;
using Aplicacion_Distribuidora.Data;

namespace Aplicacion_Distribuidora.Views
{
    public partial class ConsultarCuentasCorrientesWindow : Window
    {
        public ConsultarCuentasCorrientesWindow()
        {
            InitializeComponent();
            CargarCuentas();
        }

        private void CargarCuentas()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var cuentas = db.Clientes
                        .Where(c => c.FormaDePago == "CuentaCorriente" && c.Saldo > 0)
                        .ToList();

                    DgCuentas.ItemsSource = cuentas;

                    decimal totalDeuda = cuentas.Sum(c => c.Saldo);
                    TxtTotalDeuda.Text = $"Total deuda: {totalDeuda:C}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cuentas: {ex.Message}", "Error",
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

                    var cuentas = db.Clientes
                        .Where(c => c.FormaDePago == "CuentaCorriente" &&
                            c.Saldo > 0 &&
                            (c.Nombre.ToLower().Contains(busqueda) ||
                             c.Apellido.ToLower().Contains(busqueda)))
                        .ToList();

                    if (cuentas.Count == 0)
                    {
                        MessageBox.Show("No se encontraron cuentas corrientes con ese nombre.",
                            "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    DgCuentas.ItemsSource = cuentas;

                    decimal totalDeuda = cuentas.Sum(c => c.Saldo);
                    TxtTotalDeuda.Text = $"Total deuda: {totalDeuda:C}";
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