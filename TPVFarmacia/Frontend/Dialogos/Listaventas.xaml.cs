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
using TVPFarmacia.MVVM;

namespace TVPFarmacia.Frontend.Dialogos
{
    /// <summary>
    /// Clase que muestra una ventana con un arbol de ventas y clientes asociados.
    /// </summary>
    public partial class ListaVentas : MetroWindow
    {
        /// <summary>
        /// Constructor de la clase ListaVentas.
        /// </summary>
        /// <param name="mvVentas">Mv de ventas</param>
        /// <param name="mvVentasProducto">Mv de ventasProducto</param>
        /// <param name="mvProducto">Mv de producto</param>
        public ListaVentas(MVVentas mvVentas, MVVentasProducto mvVentasProducto, MVProducto mvProducto)
        {
            InitializeComponent();
            //Llamamos al user de TreeVentas para mostrar las ventas y clientes asociados.
            TreeVentas tv = new TreeVentas(mvVentas,mvVentasProducto,mvProducto);
            panelPrincipal.Children.Clear();
            panelPrincipal.Children.Add(tv);
        }
    }
}
