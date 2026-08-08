using System;
using System.Windows;
using Aplicacion_Distribuidora.Data;
using Aplicacion_Distribuidora.Models;

namespace Aplicacion_Distribuidora.Views
{
    public partial class RegistrarProductoWindow : Window
    {
        public RegistrarProductoWindow()
        {
            InitializeComponent();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtCodigo.Text) ||
                string.IsNullOrEmpty(TxtNombre.Text) ||
                string.IsNullOrEmpty(TxtMarca.Text) ||
                string.IsNullOrEmpty(TxtPrecio.Text) ||
                string.IsNullOrEmpty(TxtStock.Text) ||
                CmbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Por favor completá todos los campos.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El precio ingresado no es válido.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("El stock ingresado no es válido.", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var productoExistente = db.Productos.FirstOrDefault(p =>
                        p.Codigo == TxtCodigo.Text.Trim());

                    if (productoExistente != null)
                    {
                        MessageBox.Show("El código ingresado ya existe.", "Atención",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var nuevoProducto = new Producto
                    {
                        Codigo = TxtCodigo.Text.Trim(),
                        Nombre = TxtNombre.Text.Trim(),
                        Marca = TxtMarca.Text.Trim(),
                        Categoria = (CmbCategoria.SelectedItem as
                            System.Windows.Controls.ComboBoxItem).Content.ToString(),
                        Precio = precio,
                        Stock = stock,
                        Activo = true
                    };

                    db.Productos.Add(nuevoProducto);
                    db.SaveChanges();

                    MessageBox.Show("Producto registrado correctamente.", "Éxito",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
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

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LimpiarCampos()
        {
            TxtCodigo.Text = "";
            TxtNombre.Text = "";
            TxtMarca.Text = "";
            TxtPrecio.Text = "";
            TxtStock.Text = "";
            CmbCategoria.SelectedItem = null;
        }
    }
}
