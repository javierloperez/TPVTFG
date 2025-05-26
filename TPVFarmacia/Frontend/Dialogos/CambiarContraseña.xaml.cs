using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TVPFarmacia.Frontend.Dialogos
{
    /// <summary>
    /// Clase que permite cambiar la contraseña de un usuario
    /// </summary>
    public partial class CambiarContraseña : MetroWindow
    {
        /// <summary>
        /// Declaración de variables de usuario
        /// </summary>
        private Usuario _usuario;
        private UsuarioServicio _usuarioServicio;
        /// <summary>
        /// Constructor de la clase CambiarContraseña
        /// </summary>
        /// <param name="us">El usuario servicio</param>
        public CambiarContraseña(UsuarioServicio us)
        {
            InitializeComponent();
            _usuarioServicio = us;
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón cancelar, cierra la ventana de cambiar contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón guardar, comprueba si las contraseñas coinciden y si el usuario existe, y actualiza la contraseña del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Comprobamos que las contraseñas coinciden
            if (txtPass.Password.Equals(txtPass2.Password))
            {
                try
                {
                    _usuario = await _usuarioServicio.GetUsuarioPorNombre(txtUsername.Text);
                    if (_usuario == null)
                    {
                        MessageBox.Show("Error, el usuario introducido no existe ");

                    }
                    else
                    {
                        //Comprobamos que la contraseña nueva no sea igual a la anterior
                        if (txtPass2.Password.Equals(_usuario.Password))
                        {
                            MessageBox.Show("Error, la contraseña no puede ser igual que la anterior");
                        }
                        else
                        {
                            _usuario.Password = txtPass2.Password;
                            await _usuarioServicio.UpdateAsync(_usuario);

                            this.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error, el usuario introducido no existe ");
                }
            }
            else
            {
                MessageBox.Show("Error, las contraseñas no coinciden");
            }

        }
    }
}
