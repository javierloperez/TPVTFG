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

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
