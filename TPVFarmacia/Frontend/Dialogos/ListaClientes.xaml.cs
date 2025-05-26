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
using TVPFarmacia.Frontend.ControlUser;
using TVPFarmacia.MVVM.Base;
using TVPFarmacia.MVVM;

namespace TVPFarmacia.Frontend.Dialogos
{
    /// <summary>
    /// Clase donde se muestra una lista de clientes y permite su gestión (añadir, editar, eliminar).
    /// </summary>
    public partial class ListaClientes : MetroWindow
    {
        /// <summary>
        /// Constructor de la clase ListaClientes
        /// </summary>
        /// <param name="mvClientes">El mv de cliente</param>
        public ListaClientes(MVClientes mvClientes)
        {
            InitializeComponent();
            //Llamamos al user control de clientes y lo añadimos al panel principal
            ControlClientes cl = new ControlClientes(mvClientes);
            panelPrincipal.Children.Clear();
            panelPrincipal.Children.Add(cl);
        }
    }
}
