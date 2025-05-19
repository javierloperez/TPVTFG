using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;
using TPVTFG.Frontend;
using TPVTFG.MVVM.Base;

namespace TPVTFG.MVVM
{
    public class MVProducto : MVBaseCRUD<Producto>
    {
        TpvbdContext _contexto;
        Categoria _categoria;
        CategoriaServicio _categoriaServicio;
        Producto _producto;
        ProductoServicio _productoServicio;
        Oferta _oferta;
        OfertaServicio _ofertaServicio;
        WrapPanel _panelMedio;
        StackPanel _panelTicket;
        TextBlock _precioTotal;
        int it = 0;
        decimal? precioFinal = 0;
        int _cantidadItem;
        private Dictionary<int, int> _stockTemporal = new Dictionary<int, int>();
        public bool _actualizarCantidad { get; set; }

        public IEnumerable<Categoria> _listaCategorias { get { return Task.Run(_categoriaServicio.GetAllAsync).Result; } }
        public IEnumerable<Producto> _listaProductos { get { return Task.Run(_productoServicio.GetAllAsync).Result.Where(p => p.Activado.ToLower().Equals("si")); } }
        public IEnumerable<Oferta> _listaOfertas { get { return Task.Run(_ofertaServicio.GetAllAsync).Result; } }


        public Producto Clonar { get { return (Producto)_producto.Clone(); } }

        public bool guarda { get { return Task.Run(() => Add(_crearProducto)).Result; } }
        public bool borrar { get { return Task.Run(() => Delete(_crearProducto)).Result; } }
        public bool actualizar { get { return Task.Run(() => Update(_crearProducto)).Result; } }

        public Producto _crearProducto
        {
            get { return _producto; }
            set { _producto = value; OnPropertyChanged(nameof(_crearProducto)); }
        }


        public MVProducto(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public async Task Inicializa(WrapPanel panelMedio, StackPanel panelTicket, TextBlock precioTotal)
        {
            _categoria = new Categoria();
            _categoriaServicio = new CategoriaServicio(_contexto);
            _producto = new Producto();
            _productoServicio = new ProductoServicio(_contexto);
            _oferta = new Oferta();
            _ofertaServicio = new OfertaServicio(_contexto);
            _panelMedio = panelMedio;
            _panelTicket = panelTicket;
            _precioTotal = precioTotal;

            servicio = _productoServicio;
            
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
                        Source = new BitmapImage(new Uri(item.RutaImagen, UriKind.RelativeOrAbsolute)),
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
            string[] iconos = [];
            if (sender is Button btn && btn.Tag is Categoria categoria)
            {
                IEnumerable<Producto> productosFiltrados = _listaProductos.Where(p => p.Categoria == categoria.Id);

                ListarProductosCategoria(productosFiltrados, iconos);
            }

        }

        private void ListarProductosCategoria(IEnumerable<Producto> productosFiltrados, string[] iconos)
        {
            int i = 0;
            _panelMedio.Children.Clear();

            foreach (var item in productosFiltrados)
            {
                if (item.Activado.ToLower().Equals("no"))
                {
                    continue;
                }

                Button btn = new Button
                {
                    Width = 140,
                    Height = 140,
                    Margin = new Thickness(5),
                    BorderThickness = new Thickness(0),
                    Background = (Brush)new BrushConverter().ConvertFromString("#f9f1dc"),
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri(item.RutaImagen, UriKind.RelativeOrAbsolute)),

                    },
                    Tag = item
                };
                i++;
               
                btn.Click += (s, e) =>
                {
                    _cantidadItem = 1;
                    SeleccionarCantidad(s, e);
                };
                _panelMedio.Children.Add(btn);
            }
        }

        private void SeleccionarCantidad(object sender, RoutedEventArgs e)
        {

            VentanaCantidad cantidad = new VentanaCantidad(_contexto, sender, this, _cantidadItem);
            cantidad.ShowDialog();
        }

        public async Task AnyadirTicket(int cantidad, object sender)
        {

            if (sender is Button btn && btn.Tag is Producto producto)
            {

                Grid fila = new Grid();
                ColumnDefinition izquierda = new ColumnDefinition();
                ColumnDefinition derecha = new ColumnDefinition();
                ColumnDefinition extraBtn = new ColumnDefinition();

                izquierda.Width = new GridLength(2.8, GridUnitType.Star);
                derecha.Width = new GridLength(1.2, GridUnitType.Star);
                extraBtn.Width = new GridLength(0.9, GridUnitType.Star);

                TextBlock nombre = new TextBlock()
                {
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Normal,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = producto.Descripcion,
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    FontSize = 15,


                };

                Button cant = new Button()
                {
                    BorderBrush = Brushes.Transparent,
                    Background = Brushes.Transparent,
                    Content = new TextBlock()
                    {
                        Text = cantidad.ToString(),
                        Foreground = Brushes.Black,
                        FontWeight = FontWeights.Normal
                    },
                    FontSize = 15,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 0),
                    Tag = btn.Tag

                };

                decimal? precioFinal = producto.Precio * cantidad;
                TextBlock precio = new TextBlock()
                {
                    Text = precioFinal.ToString() + "€",
                    FontSize = 15,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                Button btnEliminar = new Button
                {

                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    ToolTip = "Eliminar",
                    Content = new PackIcon
                    {
                        Kind = PackIconKind.CloseCircle,
                        Width = 24,
                        Height = 24,
                        Foreground = Brushes.Red
                    },
                    Tag = btn.Tag,
                };

                btnEliminar.Click += (sender, e) =>
                {
                    
                    _panelTicket.Children.Remove(fila);
                    _stockTemporal.Remove(producto.Id);
                    ModificarTotal(precioFinal * -1);
                    btn.IsEnabled = true;
                };

                cant.Click += async (sender, e) =>
                {
                    if (sender is Button btnClick && btnClick.Tag is Producto producto)
                    {
                        TextBlock txtCant = (TextBlock)btnClick.Content;
                        int cantidadAnterior = int.Parse(txtCant.Text);

                        // Mostrar ventana para seleccionar nueva cantidad
                        VentanaCantidad ventanaCantidad = new VentanaCantidad(_contexto, btnClick, this, cantidadAnterior);
                        ventanaCantidad._modificar = true;
                        ventanaCantidad.ShowDialog();

                        if (_actualizarCantidad)
                        {
                            int nuevaCantidad = ventanaCantidad.CantidadSeleccionada;
                            if (nuevaCantidad != cantidadAnterior)
                            {
                                _stockTemporal.Remove(producto.Id);
                                txtCant.Text = nuevaCantidad.ToString();

                                decimal? precioAnterior = producto.Precio * cantidadAnterior;
                                decimal? precioNuevo = producto.Precio * nuevaCantidad;
                                precio.Text = precioNuevo.ToString() + "€";

                                RegistrarStockTemporal(producto.Id, nuevaCantidad);
                                ModificarTotal(precioNuevo - precioAnterior);
                                if(nuevaCantidad < cantidadAnterior)
                                {
                                    btn.IsEnabled = true;
                                }

                            }
                        }
                    }
                };

                fila.ColumnDefinitions.Add(izquierda);
                fila.ColumnDefinitions.Add(derecha);
                fila.ColumnDefinitions.Add(extraBtn);
                fila.Children.Add(nombre);
                fila.Children.Add(precio);
                fila.Children.Add(cant);
                fila.Children.Add(btnEliminar);
                Grid.SetColumn(nombre, 0);
                Grid.SetColumn(cant, 0);
                Grid.SetColumn(precio, 1);
                Grid.SetColumn(btnEliminar, 2);
                _panelTicket.Children.Add(fila);
                RegistrarStockTemporal(producto.Id, cantidad);
                ModificarTotal(precioFinal);

                if((producto.Cantidad - _stockTemporal[producto.Id]) == 0)
                {
                    btn.IsEnabled = false;
                }
                else
                {
                    btn.IsEnabled = true;
                }

            }

        }

        public int ObtenerStockDisponible(int idProducto, int stockOriginal)
        {
            if (_stockTemporal.ContainsKey(idProducto))
                return stockOriginal - _stockTemporal[idProducto];

            return stockOriginal;
        }

        public void RegistrarStockTemporal(int idProducto, int cantidad)
        {
            if (_stockTemporal.ContainsKey(idProducto))
                _stockTemporal[idProducto] += cantidad;
            else
                _stockTemporal[idProducto] = cantidad;
        }

        private void ModificarTotal(decimal? precio)
        {
            precioFinal += precio;
            _precioTotal.Text = precioFinal.ToString() + "€";
        }
    }
}
