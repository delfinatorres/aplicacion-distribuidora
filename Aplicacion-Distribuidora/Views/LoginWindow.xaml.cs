using System.Windows;
using Aplicacion_Distribuidora.Data;
using Aplicacion_Distribuidora.Models;

namespace Aplicacion_Distribuidora.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = TxtUsuario.Text.Trim();
            string contrasena = TxtContrasena.Password.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
            {
                TxtError.Text = "Por favor completá todos los campos.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            using (var db = new AppDbContext())
            {
                var vendedor = db.Vendedores.FirstOrDefault(v =>
                    v.Usuario == usuario && v.Contrasena == contrasena);

                if (vendedor == null)
                {
                    TxtError.Text = "Usuario o contraseña incorrectos.";
                    TxtError.Visibility = Visibility.Visible;
                    return;
                }

                // Login exitoso
                var principal = new MainWindow(vendedor);
                principal.Show();
                this.Close();
            }
        }
    }
}
