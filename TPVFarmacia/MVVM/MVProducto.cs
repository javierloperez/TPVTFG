﻿using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using NLog;
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
        private Logger _logger;
        //Diccionario para crear las filas del ticket
        public Dictionary<int, (Grid fila, TextBlock txtCant, TextBlock txtPrecio)> _lineasTicket = new Dictionary<int, (Grid, TextBlock, TextBlock)>();


        private int it = 0;
        decimal? _precioFinal = 0.00m;
        int _cantidadItem;
        private Dictionary<int, int> _stockTemporal = new Dictionary<int, int>();
        public bool _actualizarCantidad { get; set; }
        private string _nombreP;    

        public List<Categoria> _listaCategorias { get; set; } = new List<Categoria>();
        public List<Categoria> _listaCategoriasAux { get; set; } = new List<Categoria>();


        public IEnumerable<Producto> _listaProductos { get { return Task.Run(_productoServicio.GetAllAsync).Result.Where(p => p.Activado.ToLower().Equals("si")); } }
        public IEnumerable<Oferta> _listaOfertas { get { return Task.Run(_ofertaServicio.GetAllAsync).Result.Where(o => o.OfertaFin>DateTime.Now); } }

        private ListCollectionView _listaProductosParaFiltro;
        public ListCollectionView listaProductosFiltro => _listaProductosParaFiltro;
        private Predicate<object> predicadoFiltro;
        private List<Predicate<Producto>> criterios;

        private Predicate<Producto> criterioBusqueda;
        private Predicate<Producto> criterioCategoria;


        /// <summary>
        /// Propiedad que devuelve una copia del producto actual, para clonar el producto antes de realizar cambios.
        /// </summary>
        public Producto Clonar { get { return (Producto)_producto.Clone(); } }

        /// <summary>
        /// Propiedad que guarda el producto actual, se usa para crear un producto.
        /// </summary>
        public bool guarda
        {
            get
            {
                var resultado = Task.Run(() => Add(_crearProducto)).Result;
                Task.Run(RecargarListaProductosAsync);
                return resultado;
            }
        }

        /// <summary>
        /// Propiedad que actualiza el producto actual, se usa para actualizar un producto existente.
        /// </summary>
        public bool actualizar
        {
            get
            {
                var resultado = Task.Run(() => Update(_crearProducto)).Result;
                Task.Run(RecargarListaProductosAsync);
                return resultado;
            }
        }

        /// <summary>
        /// Variable que almacena el producto que se está creando o actualizando, se usa para crear un producto o actualizar uno existente.
        /// </summary>
        public Producto _crearProducto
        {
            get { return _producto; }
            set { _producto = value; OnPropertyChanged(nameof(_crearProducto)); }
        }

        /// <summary>
        /// Propiedad que filtra los productos por nombre, se usa para buscar un producto por su nombre.
        /// </summary>
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
        /// <summary>
        /// Propiedad que almacena la categoría seleccionada, se usa para filtrar los productos por categoría.
        /// </summary>
        public Categoria categoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set { _categoriaSeleccionada = value; OnPropertyChanged(nameof(categoriaSeleccionada)); }
        }
        public MVProducto(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Método que inicializa las variables y carga las categorías y productos desde la base de datos.
        /// </summary>
        /// <param name="logger">El logger para crear los logs</param>
        /// <param name="mvOfertas">EL MV de ofertas</param>
        /// <param name="panelMedio">El panel medio de la ventana principal</param>
        /// <param name="panelTicket">El panel de ticket de la ventana principal</param>
        /// <param name="precioTotal">El precioTotal del ticket</param>
        /// <param name="panelCategorias">El panel de categorías de la ventana principal</param>
        /// <param name="panelInferior">El panel inferior de la ventan categorías</param>
        /// <param name="precioConIva">El precio con iva de la ventana principal</param>
        /// <param name="porcentajeIva">El porcentaje de iva a aplicar</param>
        /// <returns></returns>
        public async Task Inicializa(Logger logger,MVOfertas mvOfertas, WrapPanel panelMedio, StackPanel panelTicket, TextBlock precioTotal, StackPanel panelCategorias, Grid panelInferior, TextBlock precioConIva, System.Windows.Controls.TextBox porcentajeIva)
        {
            _logger = logger;
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
            servicio = _productoServicio;
            _listaProductosParaFiltro = new ListCollectionView((await _productoServicio.GetAllAsync()).ToList());
            criterios = new List<Predicate<Producto>>();
            predicadoFiltro = new Predicate<object>(FiltroCriterios);
            criterios.Clear();

            await CargarCategoriasAsync();
            await RecargarListaProductosAsync();
            ListadoCategorias(_panelCategorias);

            InicializaCriterios();
        }

        /// <summary>
        /// Método que actualiza el stock de un producto, se usa para actualizar la cantidad de un producto después de una venta.
        /// </summary>
        /// <param name="id">Id del producto</param>
        /// <param name="cantidad">Cantidad del producto</param>
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
                _logger.Error($"Error al actualizar el stock del producto con ID {id}. Cantidad: {cantidad}");
            }
            _crearProducto = new Producto();
        }

        /// <summary>
        /// Método que carga las categorías desde la base de datos de forma asíncrona y las muestra en el panel de categorías.
        /// </summary>
        /// <returns></returns>
        public async Task CargarCategoriasAsync()
        {
            _listaCategorias = (await _categoriaServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaCategorias));
            ListadoCategorias(_panelCategorias);
            await CargarCategoriasAux();
        }

        /// <summary>
        /// Método que carga las categorías auxiliares desde la base de datos de forma asíncrona y las muestra en el panel de categorías.
        /// </summary>
        /// <returns></returns>
        public async Task CargarCategoriasAux()
        {
            _listaCategoriasAux = (await _categoriaServicio.GetAllAsync()).ToList();
            var categoriaInicial = new Categoria
            {
                Id = -1,
                Categoria1 = "--Seleccionar categoría--"
            };

            _listaCategoriasAux.Insert(0, categoriaInicial);
            OnPropertyChanged(nameof(_listaCategoriasAux));
        }

        /// <summary>
        /// Método que recarga la lista de productos desde la base de datos de forma asíncrona y aplica el filtro definido.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método que recarga la lista de productos eliminados desde la base de datos de forma asíncrona y aplica el filtro definido.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método que lista las categorías en el panel de categorías, creando botones para cada categoría con su imagen y asignando un evento click para listar los productos de esa categoría.
        /// </summary>
        /// <param name="panelCategorias"></param>
        public void ListadoCategorias(StackPanel panelCategorias)
        {
            _panelCategorias.Children.Clear();
            try
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
            }catch(Exception ex)
            {
                _logger.Error(ex, "Error al cargar las categorías en el panel de categorías.");
            }

        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en un botón de categoría, filtra los productos por la categoría seleccionada y los lista en el panel medio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListarProductos_Click(object sender, RoutedEventArgs e)
        {
            string[] iconos = [];
            if (sender is Button btn && btn.Tag is Categoria categoria)
            {
                IEnumerable<Producto> productosFiltrados = _listaProductos.Where(p => p.Categoria == categoria.Id);

                ListarProductosCategoria(productosFiltrados, iconos);
            }

        }

        /// <summary>
        /// Método que lista los productos de una categoría filtrada, creando botones para cada producto con su imagen y asignando un evento click para seleccionar la cantidad del producto.
        /// </summary>
        /// <param name="productosFiltrados"></param>
        /// <param name="iconos"></param>
        private void ListarProductosCategoria(IEnumerable<Producto> productosFiltrados, string[] iconos)
        {
            int i = 0;
            _panelMedio.Children.Clear();
            try
            {
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
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al listar los productos de la categoría seleccionada.");
            }
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en un botón de producto, abre una ventana para seleccionar la cantidad del producto y añadirlo al ticket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionarCantidad(object sender, RoutedEventArgs e)
        {

            VentanaCantidad cantidad = new VentanaCantidad(_contexto, sender, this, _cantidadItem);
            cantidad.ShowDialog();
        }

        /// <summary>
        /// Método que añade un ticket al panel de ticket, actualizando la cantidad y el precio del producto si ya existe, o creando una nueva línea si no existe.
        /// </summary>
        /// <param name="cantidad"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método que devuelve el stock temporal de los productos añadidos al ticket, se usa para obtener la cantidad de cada producto en el ticket.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> CogerListaTicket()
        {
            return _stockTemporal;
        }

        /// <summary>
        /// Método que obtiene el precio de un producto por su ID, aplicando el descuento de la oferta si existe.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public decimal CogerPrecioProducto(int id)
        {
            decimal precio = 0;
            Producto prod = new Producto();
            prod = _productoServicio.GetByIdAsync(id).Result;
            precio = prod.Precio;
            precio = precio - (precio * _mvOfertas.ComprobarOfertas(prod.OfertaId ?? 0) / 100);
            return precio;
        }

        /// <summary>
        /// Método que obtiene el nombre de un producto por su ID, se usa para mostrar el nombre del producto en el ticket.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string CogerNombreProducto(int id)
        {

            Producto prod = new Producto();
            prod = _productoServicio.GetByIdAsync(id).Result;

            return prod.Descripcion;
        }

        /// <summary>
        /// Método que cambia el tipo de lista a mostrar, ya sea la lista de productos activos o la lista de productos eliminados.
        /// </summary>
        /// <param name="tipo"></param>
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

        /// <summary>
        /// Método que obtiene el stock disponible de un producto, restando el stock temporal del stock original del producto.
        /// </summary>
        /// <param name="idProducto"></param>
        /// <param name="stockOriginal"></param>
        /// <returns></returns>
        public int ObtenerStockDisponible(int idProducto, int stockOriginal)
        {
            if (_stockTemporal.ContainsKey(idProducto))
                return stockOriginal - _stockTemporal[idProducto];

            return stockOriginal;
        }

        /// <summary>
        /// Método que limpia el stock temporal, se usa para reiniciar el ticket después de una venta o al finalizar la sesión.
        /// </summary>
        public void LimpiarStock()
        {
            _stockTemporal.Clear();
            _precioFinal = 0;
        }

        /// <summary>
        /// Método que registra un stock temporal de un producto, se usa para añadir productos al ticket antes de confirmar la venta.
        /// </summary>
        /// <param name="idProducto"></param>
        /// <param name="cantidad"></param>
        public void RegistrarStockTemporal(int idProducto, int cantidad)
        {
            if (_stockTemporal.ContainsKey(idProducto))
                _stockTemporal[idProducto] += cantidad;
            else
                _stockTemporal[idProducto] = cantidad;
        }

        /// <summary>
        /// Método que modifica el total del ticket, actualizando el precio total y el precio con IVA, se usa para actualizar el total después de añadir o eliminar productos del ticket.
        /// </summary>
        /// <param name="precio"></param>
        private void ModificarTotal(decimal? precio)
        {
            try
            {
                _precioFinal += precio;

                _precioTotal.Text = _precioFinal.ToString() + "€";

                _precioConIva.Text = (decimal.Parse(_precioTotal.Text.TrimEnd('€')) * (1 + decimal.Parse(_iva.Text.TrimEnd('%')) / 100)).ToString("0.00") + "€";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al modificar el total del ticket. Precio: " + precio);
            }
        }

        /// <summary>
        /// Método que añade los criterios de búsqueda para filtrar los productos, se usa para aplicar los filtros de nombre y categoría al buscar productos
        /// </summary>
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

        /// <summary>
        /// Método que inicializa los criterios de búsqueda
        /// </summary>
        private void InicializaCriterios()
        {
            criterioBusqueda = new Predicate<Producto>(m => !string.IsNullOrEmpty(m.Descripcion) && m.Descripcion.ToLower().StartsWith(filtroNombre.ToLower()));
            criterioCategoria = new Predicate<Producto>(m => m.CategoriaNavigation != null && m.CategoriaNavigation.Equals(categoriaSeleccionada));

        }

        /// <summary>
        /// Método que filtra los productos según los criterios establecidos
        /// </summary>
        public void Filtrar()
        {
            AddCriterios();
            listaProductosFiltro.Filter = predicadoFiltro;
        }

        /// <summary>
        /// Método que aplica los criterios de filtro a un producto
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
