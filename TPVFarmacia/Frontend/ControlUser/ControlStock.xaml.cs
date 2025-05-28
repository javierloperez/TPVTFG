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
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Frontend.Dialogos;
using TVPFarmacia.MVVM;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.Frontend.ControlUser
{
    /// <summary>
    /// Lógica de interacción para ControlStock.xaml, clase encargada de listar los productos y gestionarlos
    /// </summary>
    public partial class ControlStock : UserControl
    {
        /// <summary>
        /// Declaración de variables para los MVVM y la ventana principal.
        /// </summary>
        private MVProducto _mvProducto;
        private MVOfertas _mvOfertas;
        private MVCategoria _mvCategoria;
        private MainWindow _ventana;
        private string _tipoLista = "Activos";
        /// <summary>
        /// Constructor de la clase ControlStock
        /// </summary>
        /// <param name="mvProducto">MV de producto</param>
        /// <param name="mvOfertas">Mv de oferta</param>
        /// <param name="mvCategoria">Mv de categoria</param>
        /// <param name="ventana">La ventana principal MainWindow</param>
        public ControlStock(MVProducto mvProducto, MVOfertas mvOfertas, MVCategoria mvCategoria, MainWindow ventana)
        {
            InitializeComponent();
            _mvProducto = mvProducto;
            _mvOfertas = mvOfertas;
            _mvCategoria = mvCategoria;
            DataContext = _mvProducto;
            txtBuscarNombre.Clear();
            _ventana = ventana;
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón para agregar un nuevo producto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            AgregarProducto ap = new AgregarProducto(_mvProducto, false, _ventana);
            ap.ShowDialog();
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón para agregar una nueva oferta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarOferta_Click(object sender, RoutedEventArgs e)
        {
            AgregarOferta ao = new AgregarOferta(_mvOfertas);
            ao.ShowDialog();
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón para agregar una nueva categoría.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarCategoria_Click(object sender, RoutedEventArgs e)
        {
            AgregarCategoria ac = new AgregarCategoria(_mvCategoria, _mvProducto);
            ac.ShowDialog();
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón para borrar un producto seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Content is PackIcon icono)
            {
                if (_tipoLista.Equals("Eliminados"))
                {
                    _tipoLista = "Activos";
                    _mvProducto._crearProducto = (Producto)dgAñadirProducto.SelectedItem;
                    _mvProducto._crearProducto.Activado = "si";

                    if (_mvProducto.actualizar)
                    {
                        MessageBox.Show("Producto restaurado correctamente", "Gestión productos");
                    }
                    else
                    {
                        MessageBox.Show("Error al intentar restaurar producto", "Gestión productos");
                    }

                    _mvProducto._crearProducto = new Producto();
                    icono.Kind = PackIconKind.Delete;
                    icono.Foreground = Brushes.Red;
                    verEliminados.Content = "Eliminados";
                }
                else
                {

                    _mvProducto._crearProducto = (Producto)dgAñadirProducto.SelectedItem;

                    _mvProducto._crearProducto.Activado = "no";

                    if (_mvProducto.actualizar)
                    {
                        MessageBox.Show("Producto eliminado correctamente", "Gestión productos");
                    }
                    else
                    {
                        MessageBox.Show("Error al intentar eliminar producto", "Gestión productos");
                    }

                    _mvProducto._crearProducto = new Producto();
                   

                }
            }


        }
        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón para editar un producto seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            _mvProducto._crearProducto = (Producto)dgAñadirProducto.SelectedItem;

            Producto articuloAux = _mvProducto.Clonar;
            AgregarProducto ap = new AgregarProducto(_mvProducto, true, _ventana);
            ap.ShowDialog();

            if (ap.DialogResult.Equals(true))
            {
                dgAñadirProducto.Items.Refresh();
                _mvProducto._crearProducto = new Producto();

            }
            else
            {
                _mvProducto._crearProducto = articuloAux;
                dgAñadirProducto.SelectedItem = articuloAux;
                _mvProducto._crearProducto = new Producto();
            }
        }
        /// <summary>
        /// Evento que se ejecuta al cambiar el texto en el campo de búsqueda de productos para filtrar la lista de productos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBuscarNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            _mvProducto.Filtrar();
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la selección de la categoría para filtrar los productos por categoría.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mvProducto.Filtrar();
        }

        private void dgAñadirProducto_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                var row = e.Row;
                var btnBorrar = FindChildByName<Button>(row, "btnBorrar");
                if (btnBorrar != null)
                {
                    var icono = FindChildByName<PackIcon>(btnBorrar, "iconoBorrar");
                    if (icono != null)
                    {
                        if (_tipoLista.Equals("Activos"))
                        {
                            icono.Kind = PackIconKind.Delete;
                            icono.Foreground = Brushes.Red;
                        }
                        else
                        {
                            icono.Kind = PackIconKind.Restore;
                            icono.Foreground = Brushes.Green;
                        }
                    }
                }
            }, DispatcherPriority.Background);
        }


        public static T FindChildByName<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is FrameworkElement fe && fe.Name == childName)
                    return (T)child;

                var result = FindChildByName<T>(child, childName);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void verEliminados_Click(object sender, RoutedEventArgs e)
        {

            if (verEliminados.Content.Equals("Eliminados"))
            {
                verEliminados.Content = "Activos";
                _tipoLista = "Eliminados";
                _mvProducto.CambiarTipoLista("Eliminados");

            }
            else
            {
                verEliminados.Content = "Eliminados";
                _tipoLista = "Activos";
                _mvProducto.CambiarTipoLista("Activos");
            }

        }
    }
}
