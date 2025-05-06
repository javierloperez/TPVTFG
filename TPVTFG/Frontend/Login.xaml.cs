using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.EntityFrameworkCore;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;

namespace TPVTFG.Frontend
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        private TpvbdContext contexto;
        private UsuarioServicio usuarioServicio;
        public Login()
        {
            if (ConectarBD()) InitializeComponent();
            usuarioServicio = new UsuarioServicio(contexto);
        }

        

        private bool ConectarBD()
        {
            bool correcto = true;
            contexto = new TpvbdContext();
            try
            {
                contexto.Database.OpenConnection();
            }
            catch (Exception ex)
            {
                correcto = false;
                MessageBox.Show("Conexion de la base de datos", "Ups!!!");
            }
            return correcto;
        }

 

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (await usuarioServicio.Login(txtUsername.Text, txtPassword.Password))
            {
                MainWindow ventaPrincipal = new MainWindow(contexto);
                ventaPrincipal.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("El usuario y/o contraseña no son correctos", "Inicio de sesion");
            }
        }

        private void BtnLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            BtnLogin.FontSize = 20;
        }
    }
}
