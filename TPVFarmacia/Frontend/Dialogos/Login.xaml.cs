using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.EntityFrameworkCore;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;
using TVPFarmacia.Frontend.Dialogos;

namespace TVPFarmacia.Frontend
{
    /// <summary>
    /// Clase que permite iniciar sesión en la aplicación
    /// </summary>
    public partial class Login : MetroWindow
    {
        /// <summary>
        /// Declaración de variables de usuario y contexto de la base de datos
        /// </summary>
        private TpvbdContext contexto;
        private UsuarioServicio usuarioServicio;
        private Usuario usuario;
        /// <summary>
        /// Constructor de la clase Login
        /// </summary>
        public Login()
        {
            if (ConectarBD()) InitializeComponent();
            usuarioServicio = new UsuarioServicio(contexto);
        }


        /// <summary>
        /// Método que conecta con la base de datos y abre la conexión
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de inicio de sesión, comprueba las credenciales del usuario y si son correctas, abre la ventana principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (await usuarioServicio.Login(txtUsername.Text, txtPassword.Password))
            {
                usuario = await usuarioServicio.GetUsuarioPorNombre(txtUsername.Text);

                MainWindow ventaPrincipal = new MainWindow(contexto, usuario);
                ventaPrincipal.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("El usuario y/o contraseña no son correctos", "Inicio de sesion");
            }
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de cambiar contraseña, abre la ventana de cambiar contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewPass_Click(object sender, RoutedEventArgs e)
        {
            CambiarContraseña cc = new CambiarContraseña( usuarioServicio);
            cc.ShowDialog();
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de salir, cierra la aplicación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
