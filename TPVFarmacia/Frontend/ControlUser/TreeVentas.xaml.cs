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
using TVPFarmacia.Backend.Modelos;
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
        private MVProducto _mvProducto;
        private MVVentasProducto _mvVentasProducto;

        /// <summary>
        /// Constructor de la clase TreeVentas.
        /// </summary>
        /// <param name="mvVentas">Mv de ventas</param>
        /// <param name="mvProducto">Mv de productos</param>
        /// <param name="mvVentasProducto">Mv de ventasProducto</param>
        public TreeVentas(MVVentas mvVentas, MVVentasProducto mvVentasProducto, MVProducto mvProducto)
        {
            InitializeComponent();
            _mvVentas = mvVentas;
            _mvProducto = mvProducto;
            _mvVentasProducto = mvVentasProducto;
            DataContext = _mvVentas;
        }

        private void treeVentas_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeVentas.SelectedItem != null && treeVentas.SelectedItem is Venta)
            {
                int idVenta = ((Venta)treeVentas.SelectedItem).Id;
                Dictionary<int, int> listaIDProductos = _mvVentasProducto.RecogerListaProductos(idVenta);

                // Creamos copias nuevas, sin modificar los originales
                List<Producto> productos = _mvProducto._listaProductos
                    .Where(p => listaIDProductos.Keys.Contains(p.Id))
                    .Select(p => new Producto
                    {
                        Id = p.Id,
                        Descripcion = p.Descripcion,
                        Precio = _mvProducto.CogerPrecioProducto(p.Id) * listaIDProductos[p.Id],
                        Cantidad = listaIDProductos[p.Id]
                    }).ToList();

                dgProductos.ItemsSource = productos;
                
            }
        }
        private async void EliminarVenta_Click(object sender, RoutedEventArgs e)
        {
            if (treeVentas.SelectedItem is Venta ventaSeleccionada)
            {
                var resultado = MessageBox.Show(
                    $"¿Estás seguro que deseas eliminar la venta?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {

                        _mvVentas._crearVenta = ventaSeleccionada;

                        _mvVentasProducto.BorrarVentasID(ventaSeleccionada.Id);
                        await _mvVentas.Delete(_mvVentas._crearVenta);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar la venta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    _mvVentas._crearVenta = new Venta();
                    await _mvVentas.CargarVentasAsync();
                    treeVentas.Items.Refresh();
                }
            }
        }


    }
}
