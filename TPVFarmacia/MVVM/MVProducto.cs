using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using TPVFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;
using TVPFarmacia.Frontend;
using TVPFarmacia.Frontend.Dialogos;
using TVPFarmacia.MVVM.Base;
using Button = System.Windows.Controls.Button;

namespace TVPFarmacia.MVVM
{
    public class MVProducto : MVBaseCRUD<Producto>
    {
        private TpvbdContext _contexto;
        private Categoria _categoria;
        private Categoria _categoriaSeleccionada;
        private MVOfertas _mvOfertas;
        private CategoriaServicio _categoriaServicio;
        private Producto _producto;
        private ProductoServicio _productoServicio;
        private Usuario _oferta;
        private OfertaServicio _ofertaServicio;
        private WrapPanel _panelMedio;
        private StackPanel _panelTicket;
        private StackPanel _panelCategorias;
        private TextBlock _precioTotal;
        private TextBlock _precioConIva;
        private Grid _panelInferior;
        private TextBox _iva;
        public Dictionary<int, (Grid fila, TextBlock txtCant, TextBlock txtPrecio)> _lineasTicket = new Dictionary<int, (Grid, TextBlock, TextBlock)>();


        private int it = 0;
        decimal? _precioFinal = 0.00m;
        int _cantidadItem;
        private Dictionary<int, int> _stockTemporal = new Dictionary<int, int>();
        public bool _actualizarCantidad { get; set; }
        private string _nombreP;
        public IEnumerable<Categoria> _listaCategorias { get { return Task.Run(_categoriaServicio.GetAllAsync).Result; } }
        private ObservableCollection<Categoria> _listaCategoriasAux;
        public ObservableCollection<Categoria> ListaCategoriasAux
        {
            get => _listaCategoriasAux;
            set
            {
                _listaCategoriasAux = value;
                OnPropertyChanged(nameof(ListaCategoriasAux));
            }
        }



        public IEnumerable<Producto> _listaProductos { get { return Task.Run(_productoServicio.GetAllAsync).Result.Where(p => p.Activado.ToLower().Equals("si")); } }
        public IEnumerable<Oferta> _listaOfertas { get { return Task.Run(_ofertaServicio.GetAllAsync).Result; } }

        private ListCollectionView _listaProductosParaFiltro;
        public ListCollectionView listaProductosFiltro => _listaProductosParaFiltro;
        private Predicate<object> predicadoFiltro;
        private List<Predicate<Producto>> criterios;

        private Predicate<Producto> criterioBusqueda;
        private Predicate<Producto> criterioCategoria;


        public void LimpiarStock()
        {
            _stockTemporal.Clear();
            _precioFinal = 0;
        }
        public Producto Clonar { get { return (Producto)_producto.Clone(); } }

        public bool guarda
        {
            get
            {
                var resultado = Task.Run(() => Add(_crearProducto)).Result;
                Task.Run(RecargarListaProductosAsync);
                return resultado;
            }
        }

        public bool actualizar
        {
            get
            {
                var resultado = Task.Run(() => Update(_crearProducto)).Result;
                Task.Run(RecargarListaProductosAsync);
                return resultado;
            }
        }


        public Producto _crearProducto
        {
            get { return _producto; }
            set { _producto = value; OnPropertyChanged(nameof(_crearProducto)); }
        }
        public string filtroNombre
        {
            get { return _nombreP; }
            set
            {
                _nombreP = value; OnPropertyChanged(nameof(filtroNombre));
                if (_nombreP == null)
                {

                    _nombreP = "";
                }
            }
        }

        public Categoria categoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set { _categoriaSeleccionada = value; OnPropertyChanged(nameof(categoriaSeleccionada)); }
        }
        public MVProducto(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public async Task Inicializa(MVOfertas mvOfertas, WrapPanel panelMedio, StackPanel panelTicket, TextBlock precioTotal, StackPanel panelCategorias, Grid panelInferior, TextBlock precioConIva, System.Windows.Controls.TextBox porcentajeIva)
        {
            _mvOfertas = mvOfertas;
            _categoria = new Categoria();
            _categoriaServicio = new CategoriaServicio(_contexto);
            _producto = new Producto();
            _productoServicio = new ProductoServicio(_contexto);
            _oferta = new Usuario();
            _ofertaServicio = new OfertaServicio(_contexto);
            _panelMedio = panelMedio;
            _panelTicket = panelTicket;
            _precioTotal = precioTotal;
            _panelCategorias = panelCategorias;
            _panelInferior = panelInferior;
            _precioConIva = precioConIva;
            _iva = porcentajeIva;
            await CargarCategoriasAsync();
            servicio = _productoServicio;
            _listaProductosParaFiltro = new ListCollectionView((await _productoServicio.GetAllAsync()).ToList());
            criterios = new List<Predicate<Producto>>();
            predicadoFiltro = new Predicate<object>(FiltroCriterios);
            criterios.Clear();
            ListadoCategorias(_panelCategorias);

            InicializaCriterios();
            await RecargarListaProductosAsync();

        }

        public void ActualizarStock(int id, int cantidad)
        {
            Producto prod = new Producto();
            prod = _productoServicio.GetByIdAsync(id).Result;
            prod.Cantidad -= cantidad;
            _crearProducto = prod;

            if (actualizar)
            {

            }
            else
            {
                MessageBox.Show("Error al actualizar el stock");
            }
            _crearProducto = new Producto();
        }

        public async Task CargarCategoriasAsync()
        {
            var listaOriginal = await _categoriaServicio.GetAllAsync();

            var listaConExtra = new ObservableCollection<Categoria>
    {
        new Categoria { Id = -1, Categoria1 = "--Selecciona una categoría--" }
    };

            foreach (var cat in listaOriginal)
            {
                listaConExtra.Add(cat);
            }

            ListaCategoriasAux = listaConExtra;
        }
        public async Task RecargarListaProductosAsync()
        {
            var productos = await _productoServicio.GetAllAsync();
            var productosActivos = productos
                .Where(p => p.Activado.ToLower().Equals("si"))
                .ToList();

            _listaProductosParaFiltro = new ListCollectionView(productosActivos);
            _listaProductosParaFiltro.Filter = predicadoFiltro;

            OnPropertyChanged(nameof(listaProductosFiltro));
        }

        public async Task RecargarListaEliminados()
        {
            var productos = await _productoServicio.GetAllAsync();
            var productosActivos = productos
                .Where(p => p.Activado.ToLower().Equals("no"))
                .ToList();

            _listaProductosParaFiltro = new ListCollectionView(productosActivos);
            _listaProductosParaFiltro.Filter = predicadoFiltro;

            OnPropertyChanged(nameof(listaProductosFiltro));
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
                if (item.Cantidad > 0)
                {
                    btn.IsEnabled = true;
                }
                else
                {
                    btn.IsEnabled = false;
                }
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
            decimal? precioFinal;

            if (sender is Button btn && btn.Tag is Producto producto)
            {
                if (_lineasTicket.ContainsKey(producto.Id))
                {
                    // Ya existe: actualizamos
                    var (filaExistente, txtCant, txtPrecio) = _lineasTicket[producto.Id];
                    int cantidadAnterior = int.Parse(txtCant.Text);
                    int nuevaCantidad = cantidadAnterior + cantidad;

                    txtCant.Text = nuevaCantidad.ToString();

                    decimal? precioAnterior = CogerPrecioProducto(producto.Id) * cantidadAnterior;
                    decimal? precioNuevo = CogerPrecioProducto(producto.Id) * nuevaCantidad;

                    txtPrecio.Text = precioNuevo.ToString() + "€";
                    _stockTemporal.Remove(producto.Id);

                    RegistrarStockTemporal(producto.Id, nuevaCantidad);
                    ModificarTotal(precioNuevo - precioAnterior);
                    // Desactivar el botón si ya no se puede añadir más
                    btn.IsEnabled = (producto.Cantidad - nuevaCantidad) > 0;

                    return;
                }
                Grid fila = new Grid();
                ColumnDefinition izquierda = new ColumnDefinition();
                ColumnDefinition derecha = new ColumnDefinition();
                ColumnDefinition extraBtn = new ColumnDefinition();

                izquierda.Width = new GridLength(3, GridUnitType.Star);
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
                    Margin = new Thickness(0,0,40,0)

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
                    Margin = new Thickness(0, 0, 5, 0),
                    Tag = btn.Tag

                };

                PackIcon icono = new PackIcon
                {
                    Kind = PackIconKind.Edit,
                    Width = 22,
                    Height = 22,
                    Foreground = Brushes.LightSkyBlue,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, -10, 0)
                };
                precioFinal = CogerPrecioProducto(producto.Id) * cantidad;
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
                    ToolTip = "Editar",
                    Content = new PackIcon
                    {
                        Kind = PackIconKind.CloseCircle,
                        Width = 24,
                        Height = 24,
                        Foreground = Brushes.Red
                    },

                };

                btnEliminar.Click += (sender, e) =>
                {
                    var (filaExistente, txtCant, txtPrecio) = _lineasTicket[producto.Id];
                    _panelTicket.Children.Remove(fila);
                    _stockTemporal.Remove(producto.Id);
                    _lineasTicket.Remove(producto.Id);


                    ModificarTotal(decimal.Parse(txtPrecio.Text.TrimEnd('€')) * -1);
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
                            int nuevaCantidad = (int)ventanaCantidad.CantidadSeleccionada;
                            if (nuevaCantidad != cantidadAnterior)
                            {
                                _stockTemporal.Remove(producto.Id);
                                txtCant.Text = nuevaCantidad.ToString();

                                decimal? precioAnterior = CogerPrecioProducto(producto.Id) * cantidadAnterior;
                                decimal? precioNuevo = CogerPrecioProducto(producto.Id) * nuevaCantidad;
                                precio.Text = precioNuevo.ToString() + "€";

                                RegistrarStockTemporal(producto.Id, nuevaCantidad);
                                ModificarTotal(precioNuevo - precioAnterior);
                                precioFinal = precioNuevo;
                                _lineasTicket[producto.Id] = (fila, txtCant, precio);
                                //En caso de que previamente se haya seleccionado el máximo de cantidad de un producto, se vuelve a habilitar el botón si la cantidad es menor
                                if (nuevaCantidad < cantidadAnterior)
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
                fila.Children.Add(icono);
                fila.Children.Add(btnEliminar);
                Grid.SetColumn(nombre, 0);
                Grid.SetColumn(cant, 0);
                Grid.SetColumn(icono, 0);
                Grid.SetColumn(precio, 1);
                Grid.SetColumn(btnEliminar, 2);
                _panelTicket.Children.Add(fila);
                _lineasTicket[producto.Id] = (fila, (TextBlock)cant.Content, precio);

                RegistrarStockTemporal(producto.Id, cantidad);
                ModificarTotal(precioFinal);

                if (producto.Cantidad > 0)
                {
                    btn.IsEnabled = true;
                }
                else
                {
                    btn.IsEnabled = false;
                }
                if ((producto.Cantidad - _stockTemporal[producto.Id]) == 0)
                {
                    btn.IsEnabled = false;
                }
                else
                {
                    btn.IsEnabled = true;
                }

            }

        }

        public Dictionary<int, int> CogerListaTicket()
        {
            return _stockTemporal;
        }

        public decimal CogerPrecioProducto(int id)
        {
            decimal precio = 0;
            Producto prod = new Producto();
            prod = _productoServicio.GetByIdAsync(id).Result;
            precio = prod.Precio;
            precio = precio - (precio * _mvOfertas.ComprobarOfertas(prod.OfertaId ?? 0) / 100);
            return precio;
        }

        public string CogerNombreProducto(int id)
        {

            Producto prod = new Producto();
            prod = _productoServicio.GetByIdAsync(id).Result;

            return prod.Descripcion;
        }

        public async void CambiarTipoLista(string tipo)
        {
            if (tipo.Equals("Eliminados"))
            {
                await RecargarListaEliminados();
            }
            else
            {
                await RecargarListaProductosAsync();
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
            try
            {
                _precioFinal += precio;

                _precioTotal.Text = _precioFinal.ToString() + "€";

                _precioConIva.Text = (decimal.Parse(_precioTotal.Text.TrimEnd('€')) * (1 + decimal.Parse(_iva.Text) / 100)).ToString("0.00") + "€";
            }
            catch (Exception ex)
            {

            }
        }

        private void AddCriterios()
        {
            criterios.Clear();

            if (!string.IsNullOrEmpty(filtroNombre))
            {
                criterios.Add(criterioBusqueda);
            }
            if (categoriaSeleccionada != null)
            {
                if (categoriaSeleccionada.Id == -1)
                {
                    criterios.Remove(criterioCategoria);
                }
                else
                {
                    criterios.Add(criterioCategoria);
                }
            }

        }
        private void InicializaCriterios()
        {
            criterioBusqueda = new Predicate<Producto>(m => !string.IsNullOrEmpty(m.Descripcion) && m.Descripcion.ToLower().StartsWith(filtroNombre.ToLower()));
            criterioCategoria = new Predicate<Producto>(m => m.CategoriaNavigation != null && m.CategoriaNavigation.Equals(categoriaSeleccionada));

        }

        public void Filtrar()
        {
            AddCriterios();
            listaProductosFiltro.Filter = predicadoFiltro;
        }

        private bool FiltroCriterios(object item)
        {
            bool correcto = true;
            Producto producto = (Producto)item;
            if (criterios != null)
            {
                correcto = criterios.TrueForAll(x => x(producto));
            }
            return correcto;
        }
    }
}
