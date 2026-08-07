using System;
using System.Windows;
using System.Windows.Media;
using Aplicacion_Distribuidora.Data;
using Aplicacion_Distribuidora.Models;

namespace Aplicacion_Distribuidora.Views
{
    public partial class RegistrarClienteWindow : Window
    {
        public RegistrarClienteWindow()
        {
            InitializeComponent();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNombre.Text) ||
                string.IsNullOrEmpty(TxtApellido.Text) ||
                string.IsNullOrEmpty(TxtTelefono.Text) ||
                string.IsNullOrEmpty(TxtDireccion.Text) ||
                string.IsNullOrEmpty(TxtDNI.Text) ||
                CmbFormaPago.SelectedItem == null)
            {
                MostrarMensaje("Por favor completá todos los campos.", "#F38BA8");
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var clienteExistente = db.Clientes.FirstOrDefault(c =>
                        c.Nombre == TxtNombre.Text.Trim() &&
                        c.Apellido == TxtApellido.Text.Trim());

                    if (clienteExistente != null)
                    {
                        MostrarMensaje("El cliente ya se encuentra registrado.", "#F38BA8");
                        return;
                    }

                    var nuevoCliente = new Cliente
                    {
                        Nombre = TxtNombre.Text.Trim(),
                        Apellido = TxtApellido.Text.Trim(),
                        Telefono = TxtTelefono.Text.Trim(),
                        Direccion = TxtDireccion.Text.Trim(),
                        DNI = TxtDNI.Text.Trim(),
                        FormaDePago = (CmbFormaPago.SelectedItem as
                            System.Windows.Controls.ComboBoxItem).Content.ToString(),
                        Saldo = 0
                    };

                    db.Clientes.Add(nuevoCliente);
                    db.SaveChanges();

                    MessageBox.Show("Cliente registrado correctamente.", "Éxito",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                string mensajeCompleto = ex.Message;
                if (ex.InnerException != null)
                {
                    mensajeCompleto += "\n\nDetalle: " + ex.InnerException.Message;
                }
                MessageBox.Show(mensajeCompleto, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MostrarMensaje(string mensaje, string color)
        {
            TxtMensaje.Text = mensaje;
            TxtMensaje.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(color));
            TxtMensaje.Visibility = Visibility.Visible;
        }

        private void LimpiarCampos()
        {
            TxtNombre.Text = "";
            TxtApellido.Text = "";
            TxtTelefono.Text = "";
            TxtDireccion.Text = "";
            TxtDNI.Text = "";
            CmbFormaPago.SelectedItem = null;
        }
    }
}
