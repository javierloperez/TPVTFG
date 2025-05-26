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
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Frontend.Dialogos;
using TVPFarmacia.MVVM;

namespace TVPFarmacia.Frontend.ControlUser
{
    /// <summary>
    /// Clase encargada de listar los clientes y permitir su gestión (añadir, editar, eliminar).
    /// </summary>
    public partial class ControlClientes : UserControl
    {
        /// <summary>
        /// Declaración del MV para la gestión de clientes
        /// </summary>
        private MVClientes _mvClientes;

        /// <summary>
        /// Constructor de la clase ControlClientes
        /// </summary>
        /// <param name="mvClientes"> El mv de clientes</param>
        public ControlClientes(MVClientes mvClientes)
        {
            InitializeComponent();
            _mvClientes = mvClientes;

            DataContext = _mvClientes;

        }
        /// <summary>
        /// Evento que se ejecuta al pulsar en el boton de editar un cliente seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            _mvClientes._crearCliente = (Cliente)dgAñadirCliente.SelectedItem;

            Cliente clienteAux = _mvClientes.Clonar;
            AgregarCliente ac = new AgregarCliente(_mvClientes, true);
            ac.ShowDialog();
            //Si pulsa guardar actualizamso los cambios, sino no hacemos nada
            if (ac.DialogResult.Equals(true))
            {
                dgAñadirCliente.Items.Refresh();
                _mvClientes._crearCliente = new Cliente();
            }
            else
            {
                _mvClientes._crearCliente = clienteAux;
                dgAñadirCliente.SelectedItem = clienteAux;
                _mvClientes._crearCliente = new Cliente();

            }
            //Recargamos la lista de clientes para que se muestren los cambios
            await _mvClientes.RecargarListaClientesAsync();

        }

        /// <summary>
        /// Evento que se ejecuta al pulsar en el boton de eliminar un cliente seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            _mvClientes._crearCliente = (Backend.Modelos.Cliente)dgAñadirCliente.SelectedItem;

            _mvClientes._crearCliente.Activado = "no";

            if (_mvClientes.actualizar)
            {
                MessageBox.Show("Cliente eliminado correctamente", "Gestión clientes");
            }
            else
            {
                MessageBox.Show("Error al intentar eliminar el cliente", "Gestión clientes");

            }
            _mvClientes._crearCliente = new Cliente();
            //Recargamos la lista de clientes para que se muestren los cambios
            await _mvClientes.RecargarListaClientesAsync();

        }

        /// <summary>
        /// Evento que se ejecuta al pulsar en el boton de añadir un nuevo cliente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            AgregarCliente ac = new AgregarCliente(_mvClientes,false);
            ac.ShowDialog();
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el texto del campo de búsqueda para filtar por nombre del cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            _mvClientes.Filtrar();
        }
    }
}
