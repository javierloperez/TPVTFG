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
using MahApps.Metro.Controls.Dialogs;
using TPVTFG.Backend.Modelos;
using TPVTFG.MVVM;

namespace TPVTFG.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para AgregarCliente.xaml
    /// </summary>
    public partial class AgregarCliente : MetroWindow
    {
        private Cliente _cliente;
        private MVClientes _mvClientes;
        private bool _editar;
        public AgregarCliente(MVClientes mv, bool editar)
        {
            InitializeComponent();
            _mvClientes = mv;
            DataContext = _mvClientes;
            _cliente = new Cliente();
            _mvClientes.btnGuardar = btnGuardar;
            _editar = editar;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mvClientes._crearCliente = new Cliente();
            this.Close();
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            _mvClientes._crearCliente.Activado = "si";
            if (_mvClientes.IsValid(this))
            {
                if (!_editar)
                {
                    if (_mvClientes.guarda)
                    {
                        await this.ShowMessageAsync("Gestión crear cliente", "El cliehte se ha guardado correctamente");
                        DialogResult = true;
                        _mvClientes._crearCliente = new Cliente();
                    }
                    else
                    {
                        await this.ShowMessageAsync("Gestión crear cliente", "Error, algun campo esta incompleto o no es válido");


                    }
                }
                else
                {
                    if (_mvClientes.actualizar)
                    {
                        await this.ShowMessageAsync("Gestión actualizar cliente", "El producto se ha guardado correctamente");
                        DialogResult = true;

                    }
                    else
                    {
                        await this.ShowMessageAsync("Gestión actualizar cliente", "Error, algun campo esta incompleto o no es válido");


                    }

                }
                await _mvClientes.RecargarListaClientesAsync();
            }
            else
            {
                this.ShowMessageAsync("Gestión crear producto", "Tienes campos obligatorios sin rellenar correctamente");

            }

        }
    }
}
