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
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.MVVM;

namespace TVPFarmacia.Frontend.Dialogos
{
    /// <summary>
    /// Clase que representa la ventana para agregar o editar un cliente.
    /// </summary>
    public partial class AgregarCliente : MetroWindow
    {
        /// <summary>
        /// Declaración de los Modelos y ViewModels necesarios para la gestión de clientes
        /// </summary>
        private Cliente _cliente;
        private MVClientes _mvClientes;
        //Boolean editar para saber si hay que modificar un objeto o crear uno nuevo
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

        /// <summary>
        /// Evento que se ejecuta al pulsar el boton de cancelar. Cierra la ventana y resetea el cliente a un nuevo objeto Cliente vacío.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mvClientes._crearCliente = new Cliente();
            this.Close();
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de guardar. Valida los campos del cliente y guarda los datos en la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            _mvClientes._crearCliente.Activado = "si";
            if (_mvClientes.IsValid(this))
            {
                //Comprobamos si es una edición o una creación de cliente
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
                //Recargamos al lista de clientes para que se actualice la vista
                await _mvClientes.RecargarListaClientesAsync();
            }
            else
            {
                this.ShowMessageAsync("Gestión crear producto", "Tienes campos obligatorios sin rellenar correctamente");

            }

        }
    }
}
