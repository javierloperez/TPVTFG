using System.IO;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.EntityFrameworkCore;
using NLog.Config;
using NLog;
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
        private Logger _logger;
        private string _carpetaLogs = @"C:\ProgramData\TPVFarmacia";
        /// <summary>
        /// Constructor de la clase Login
        /// </summary>
        public Login()
        {
            XmlLoggingConfiguration logConfig = new XmlLoggingConfiguration(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName, "../Logs.xml"));
            LogManager.Configuration = logConfig;
            _logger = LogManager.GetCurrentClassLogger();
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
                _logger.Error("ConectarBD. Error al abrir la conexión a la base de datos: " + ex.Message);  
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
                _logger.Info($"Login. Usuario {usuario.Login} ha iniciado sesión correctamente.");
                MainWindow ventaPrincipal = new MainWindow(contexto, usuario,_logger);
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
