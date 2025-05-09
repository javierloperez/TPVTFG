using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;
using TPVTFG.MVVM.Base;
using TPVTFG.Frontend;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TPVTFG.MVVM
{
    public class MVCategorias : MVBaseCRUD<Categoria>
    {
        TpvbdContext _contexto;
        Categoria _categoria;
        CategoriaServicio _categoriaServicio;
        Producto _producto;
        ProductoServicio _productoServicio;
        WrapPanel _panelMedio;
        StackPanel _panelTicket;
        TextBlock _precioTotal;
        int it = 0;
        decimal? precioFinal = 0;
        string[] listaIconos = new string[]
{
    "Analgesicos",
    "Jarabes",
    "Vitaminas",
    "Suplementos",
    "Cremas",
    "Antibioticos",
    "Antialergicos",
    "Antiinflamatorios",
    "Antisepticos",
    "HigienePersonal",
    "SaludSexual",
    "CuidadoInfantil"
};

        public IEnumerable<Categoria> _listaCategorias { get { return Task.Run(_categoriaServicio.GetAllAsync).Result; } }
        public IEnumerable<Producto> _listaProductos { get { return Task.Run(_productoServicio.GetAllAsync).Result; } }
        public MVCategorias(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public async Task Inicializa(WrapPanel panelMedio, StackPanel panelTicket, TextBlock precioTotal)
        {
            _categoria = new Categoria();
            _categoriaServicio = new CategoriaServicio(_contexto);
            _producto = new Producto();
            _productoServicio = new ProductoServicio(_contexto);
            _panelMedio = panelMedio;
            _panelTicket = panelTicket;
            _precioTotal = precioTotal;
            Separator separador = new Separator();

            servicio = _categoriaServicio;

        }

        public void ListadoCategorias(StackPanel panelCategorias)
        {
            foreach (var item in _listaCategorias)
            {

                Button btn = new Button
                {
                    Width = 140,
                    Height = 140,
                    Margin = new Thickness(5),
                    BorderThickness = new Thickness(0),
                    Background = (Brush)new BrushConverter().ConvertFromString("#f9f1dc"),
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("/Iconos/Categorias/" + listaIconos[it] + ".png", UriKind.RelativeOrAbsolute)),

                    },
                    Tag = item
                };


                btn.Click += ListarProductos_Click;
                it++;
                panelCategorias.Children.Add(btn);
            }

        }

        private void ListarProductos_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button btn && btn.Tag is Categoria categoria)
            {
                IEnumerable<Producto> productosFiltrados = _listaProductos.Where(p => p.Categoria == categoria.Id);

                ListarProductosCategoria(productosFiltrados);
            }

        }

        private void ListarProductosCategoria(IEnumerable<Producto> productosFiltrados)
        {
            _panelMedio.Children.Clear();
            foreach (var item in productosFiltrados)
            {

                Button btn = new Button
                {
                    Width = 140,
                    Height = 140,
                    Margin = new Thickness(5),
                    BorderThickness = new Thickness(0),
                    Background = Brushes.LightBlue,
                    Content = item.Descripcion,
                    Tag = item
                };

                btn.Click += SeleecionarCantidad;
                _panelMedio.Children.Add(btn);
            }
        }

        private void SeleecionarCantidad(object sender, RoutedEventArgs e)
        {
            VentanaCantidad cantidad = new VentanaCantidad(_contexto, sender, this);
            cantidad.Show();
        }

        public async Task AnyadirTicket(Decimal? cantidad, object sender)
        {


            if (sender is Button btn && btn.Tag is Producto producto)
            {

                Grid fila = new Grid();
                ColumnDefinition izquierda = new ColumnDefinition();
                ColumnDefinition derecha = new ColumnDefinition();

                izquierda.Width = new GridLength(3, GridUnitType.Star);
                derecha.Width = new GridLength(1, GridUnitType.Star);

                TextBlock nombre = new TextBlock()
                {
                    Text = producto.Descripcion,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 15
                };


                TextBlock cant = new TextBlock()
                {
                    Text = cantidad.ToString(),
                    FontSize = 15,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                decimal? precioFinal = producto.Precio * cantidad;
                TextBlock precio = new TextBlock()
                {
                    Text = precioFinal.ToString() + "€",
                    FontSize = 15,
                    HorizontalAlignment = HorizontalAlignment.Right
                };


                fila.ColumnDefinitions.Add(izquierda);
                fila.ColumnDefinitions.Add(derecha);
                fila.Children.Add(nombre);
                fila.Children.Add(precio);
                fila.Children.Add(cant);
                Grid.SetColumn(nombre, 0);
                Grid.SetColumn(cant, 0);
                Grid.SetColumn(precio, 1);
                _panelTicket.Children.Add(fila);

                ModificarTotal(precioFinal);
            }

        }

        private void ModificarTotal(decimal? precio)
        {
            precioFinal += precio;
            _precioTotal.Text = precioFinal.ToString()+"€";
        }
    }
}
