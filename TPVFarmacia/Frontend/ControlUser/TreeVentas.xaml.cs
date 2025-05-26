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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TVPFarmacia.MVVM;

namespace TVPFarmacia.Frontend.ControlUser
{
    /// <summary>
    /// Clase que muestra un arbol con las ventas y clientes asociados.
    /// </summary>
    public partial class TreeVentas : UserControl
    {
        /// <summary>
        /// Declara las variables de la vista modelo de ventas y clientes.
        /// </summary>
        private MVVentas _mvVentas;
        private MVClientes _mvClientes;

        /// <summary>
        /// Constructor de la clase TreeVentas.
        /// </summary>
        /// <param name="mvVentas">Mv de ventas</param>
        /// <param name="mvClientes">Mv de clientes</param>
        public TreeVentas(MVVentas mvVentas, MVClientes mvClientes)
        {
            InitializeComponent();
            _mvVentas = mvVentas;
            _mvClientes = mvClientes;
            DataContext = _mvVentas;
        }

        
    }
}
