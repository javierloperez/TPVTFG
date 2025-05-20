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
using TPVTFG.Backend.Modelos;
using TPVTFG.Frontend.Dialogos;
using TPVTFG.MVVM;

namespace TPVTFG.Frontend.ControlUser
{
    /// <summary>
    /// Lógica de interacción para ControlClientes.xaml
    /// </summary>
    public partial class ControlClientes : UserControl
    {

        private MVClientes _mvClientes;
        public ControlClientes(MVClientes mvClientes)
        {
            InitializeComponent();
            _mvClientes = mvClientes;

            DataContext = _mvClientes;

        }

        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            _mvClientes._crearCliente = (Cliente)dgAñadirCliente.SelectedItem;

            Cliente clienteAux = _mvClientes.Clonar;
            AgregarCliente ac = new AgregarCliente(_mvClientes, true);
            ac.ShowDialog();

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
            await _mvClientes.RecargarListaClientesAsync();

        }

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
            await _mvClientes.RecargarListaClientesAsync();

        }

        private void AgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            AgregarCliente ac = new AgregarCliente(_mvClientes,false);
            ac.ShowDialog();
        }

        private void txtBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            _mvClientes.Filtrar();
        }
    }
}
