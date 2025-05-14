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
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TPVTFG.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para CambiarContraseña.xaml
    /// </summary>
    public partial class CambiarContraseña : MetroWindow
    {
        private Usuario _usuario;
        private UsuarioServicio _usuarioServicio;
        public CambiarContraseña(UsuarioServicio us)
        {
            InitializeComponent();
            _usuarioServicio = us;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
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
