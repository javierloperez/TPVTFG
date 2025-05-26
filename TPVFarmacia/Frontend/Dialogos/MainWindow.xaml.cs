using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using TPVFarmacia.Backend.Utiles;
using TPVFarmacia.Frontend.Dialogos;
using TPVFarmacia.MVVM;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Frontend.Dialogos;
using TVPFarmacia.MVVM;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.Frontend
{
    /// <summary>
    /// Clase principal de la aplicación que gestiona la ventana principal del TPV.
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Declaración de variables necesarias para la gestión del TPV.
        /// </summary>
        private TpvbdContext _contexto;
        private Usuario _usuario;
        private MVProducto _mvProducto;
        private MVOfertas _mvOfertas;
        private MVCategoria _mvCategoria;
        private MVClientes _mvClientes;
        private MVVentas _mvVentas;
        private MVVentasProducto _mvVentasProducto;
        private MVUsuario _mvUsuario;
        private string _tipoPago; // Tipo de pago seleccionado (efectivo o tarjeta)
        private Cliente _clienteElegido;

        /// <summary>
        /// Constructor de la clase MainWindow.
        /// </summary>
        /// <param name="contexto">Contexto de la BD</param>
        /// <param name="usuario">Usuario con el que se ha registrado</param>
        public MainWindow(TpvbdContext contexto, Usuario usuario)
        {
            InitializeComponent();
            _contexto = contexto;
            _ = Inicializa();
            _usuario = usuario;
            // Asignación del usuario a la ventana principal
            nombreUsuario.Content = _usuario.Nombre + " " + usuario.Apellidos;

            
        }

        /// <summary>
        /// Método que inicializa los MVVM necesarios para la aplicación.
        /// </summary>
        /// <returns></returns>
        public async Task Inicializa()
        {
            try
            {
                _mvOfertas = new MVOfertas(_contexto);
                await _mvOfertas.Inicializa();

                _mvProducto = new MVProducto(_contexto);
                await _mvProducto.Inicializa(_mvOfertas, panelMedio, panelTicket, precioTotal, panelCategorias, panelInferior, precioConIva, porcentajeIva);

                _mvCategoria = new MVCategoria(_contexto);
                await _mvCategoria.Inicializa();

                _mvClientes = new MVClientes(_contexto);
                await _mvClientes.Inicializa();

                _mvUsuario = new MVUsuario(_contexto);
                await _mvUsuario.Inicializa();

                _mvVentas = new MVVentas(_contexto);
                await _mvVentas.Inicializa();

                _mvVentasProducto = new MVVentasProducto(_contexto);
                await _mvVentasProducto.Inicializa();

                DataContext = _mvClientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar la aplicación: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void cerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }



        private void Calculadora_Click(object sender, RoutedEventArgs e)
        {
            Calculadora calculadora = new Calculadora();
            calculadora.ShowDialog();

        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            StockProductos stockProductos = new StockProductos(_mvProducto, _mvOfertas, _mvCategoria, this);
            stockProductos.ShowDialog();

        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            ListaClientes lc = new ListaClientes(_mvClientes);
            lc.ShowDialog();
        }

        private void btnVentas_Click(object sender, RoutedEventArgs e)
        {
            ListaVentas lv = new ListaVentas(_mvVentas, _mvClientes);
            lv.ShowDialog();
        }

        private void efectivo_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton tipo)
            {
                if (tipo.Content is StackPanel panel)
                {
                    var textBlock = panel.Children.OfType<TextBlock>().FirstOrDefault();
                    if (textBlock != null && textBlock.Text == "Efectivo")
                    {

                        _tipoPago = "efectivo";
                        cantidadRecibida.Visibility = Visibility.Visible;
                        totalADevolver.Visibility = Visibility.Visible;
                        mensaje.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _tipoPago = "tarjeta";
                        cantidadRecibida.Visibility = Visibility.Hidden;
                        totalADevolver.Visibility = Visibility.Hidden;
                        mensaje.Visibility = Visibility.Hidden;
                    }
                }

            }

        }

        private void cbCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCliente.SelectedItem is Cliente cliente)
            {
                _clienteElegido = cliente;
                txtNombreCliente.Text = "Cliente: " + cliente.Nombre;
            }
        }

        private void cantidadRecibida_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            VentanaCantidad vc = new VentanaCantidad(_contexto, "normal", this);
            vc.ShowDialog();
        }

        private void añadirVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idVenta = 0;
                idVenta = _mvVentas.AgregarVenta(_clienteElegido, decimal.Parse(precioConIva.Text.TrimEnd('€')), _usuario, _tipoPago, decimal.Parse(porcentajeIva.Text));
                if (idVenta <= 0)
                {
                    return;
                }
                foreach (var producto in _mvProducto.CogerListaTicket())
                {
                    decimal precio = 0;
                    int cantidad = 0;
                    cantidad = producto.Value;
                    _mvProducto.ActualizarStock(producto.Key, cantidad);
                    precio = _mvProducto.CogerPrecioProducto(producto.Key) * cantidad;

                    _mvVentasProducto.InsertarVenta(producto.Key, cantidad, precio, idVenta);
                }
                GenerarTicket();
                LimpiarVentana();

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se ha podido realizar la venta. Por favor, compruebe los datos introducidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void GenerarTicket()
        {
            try
            {
                // Registrar fuente
                GlobalFontSettings.FontResolver = new CustomFontResolver();

                PdfDocument document = new PdfDocument();
                document.Info.Title = "Ticket de venta";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 11, XFontStyleEx.Regular);
                var fontTitle = new XFont("Arial", 20, XFontStyleEx.Underline);
                var boldFont = new XFont("Arial", 11, XFontStyleEx.Bold);
                int y = 0;

                // Título
                gfx.DrawString("FARMACIA TPV", fontTitle, XBrushes.Black, new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
                y += 50;

                // Datos
                gfx.DrawString($"Fecha: {DateTime.Now}", font, XBrushes.Black, 20, y);
                y += 20;

                gfx.DrawString($"Empleado: {_usuario.Nombre} {_usuario.Apellidos}", font, XBrushes.Black, 20, y);
                y += 20;

                gfx.DrawString($"Cliente: {_clienteElegido?.Nombre ?? "Sin cliente"}", font, XBrushes.Black, 20, y+20);
                y += 30;

                // Coordenadas columna
                int xProducto = 20;
                int xCantidad = 300;
                int xPrecio = 450;

                // Encabezado tabla
                gfx.DrawString("Producto", boldFont, XBrushes.Black, xProducto, y);
                gfx.DrawString("Cantidad", boldFont, XBrushes.Black, xCantidad, y);
                gfx.DrawString("Precio", boldFont, XBrushes.Black, xPrecio, y, XStringFormats.TopRight);
                y += 10;

                gfx.DrawLine(XPens.Black, 20, y, page.Width - 20, y); // Línea separadora
                y += 20;

                // Productos
                decimal total = 0;
                foreach (var producto in _mvProducto.CogerListaTicket())
                {
                    string nombre = _mvProducto.CogerNombreProducto(producto.Key);
                    int cantidad = producto.Value;
                    decimal precioUnitario = _mvProducto.CogerPrecioProducto(producto.Key);
                    decimal precioTotal = cantidad * precioUnitario;
                    total += precioTotal;

                    gfx.DrawString(nombre, font, XBrushes.Black, xProducto, y, XStringFormats.TopLeft);
                    gfx.DrawString($"{cantidad}", font, XBrushes.Black, xCantidad, y, XStringFormats.TopCenter);
                    gfx.DrawString($"{precioTotal:C}", font, XBrushes.Black, xPrecio, y, XStringFormats.TopRight);
                    y += 20;
                }

                y += 10;
                gfx.DrawLine(XPens.Black, 20, y, page.Width - 20, y);
                y += 30;
                // Total alineado con valor
                gfx.DrawString("TOTAL SIN IVA:", boldFont, XBrushes.Black, xPrecio - 100, y, XStringFormats.TopRight);
                gfx.DrawString($"{total:C}", boldFont, XBrushes.Black, xPrecio, y, XStringFormats.TopRight);
                y += 20;
                total = total + (total * int.Parse(porcentajeIva.Text)/100);
                gfx.DrawString("IVA:", boldFont, XBrushes.Black, xPrecio - 100, y, XStringFormats.TopRight);
                gfx.DrawString($"{porcentajeIva.Text}%", boldFont, XBrushes.Black, xPrecio, y, XStringFormats.TopRight);

                y += 20;

                gfx.DrawString("TOTAL CON IVA:", boldFont, XBrushes.Black, xPrecio - 100, y, XStringFormats.TopRight);
                gfx.DrawString($"{total:C}", boldFont, XBrushes.Black, xPrecio, y, XStringFormats.TopRight);
                string nombreArchivo = $"ticket_{_clienteElegido.Nombre ?? "Estándar"}.pdf";

                SaveFileDialog guardarDialogo = new SaveFileDialog
                {
                    FileName = nombreArchivo,
                    DefaultExt = ".pdf",
                    Filter = "Archivo PDF (*.pdf)|*.pdf"
                };

                bool? resultado = guardarDialogo.ShowDialog();
                if (resultado == true)
                {
                    document.Save(guardarDialogo.FileName);
                    MessageBox.Show("Ticket guardado correctamente en:\n" + guardarDialogo.FileName, "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el ticket: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void porcentajeIva_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            VentanaCantidad vc = new VentanaCantidad(_contexto, "iva", this);
            vc.ShowDialog();
        }

        private void porcentajeIva_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                precioConIva.Text = (decimal.Parse(precioTotal.Text.TrimEnd('€')) * (1 + decimal.Parse(porcentajeIva.Text) / 100)).ToString("0.00") + "€";
            }
            catch (Exception ex)
            {

            }
        }

        public void LimpiarVentana()
        {
            _mvProducto.LimpiarStock();
            panelTicket.Children.Clear();
            porcentajeIva.Text = "0";
            precioConIva.Text = string.Empty;
            _tipoPago = string.Empty;
            _clienteElegido = new Cliente();
            totalADevolver.Text = string.Empty;
            precioTotal.Text = "0";
            txtNombreCliente.Text = string.Empty;
            cbCliente.SelectedIndex = 0;
            cantidadRecibida.Text = "0";
            efectivo.IsChecked = false;
            tarjeta.IsChecked = false;
            cantidadRecibida.Visibility = Visibility.Hidden;
            mensaje.Visibility = Visibility.Hidden;
            totalADevolver.Visibility = Visibility.Hidden;
        }

        private void añadirCliente_Click(object sender, RoutedEventArgs e)
        {
            AgregarCliente ac = new AgregarCliente(_mvClientes, false);
            ac.ShowDialog();
        }

        private void nombreUsuario_Click(object sender, RoutedEventArgs e)
        {
            _mvUsuario._crearUsuario = _usuario;
            AgregarUsuario au = new AgregarUsuario(_contexto, _mvUsuario, nombreUsuario);
            au.ShowDialog();
        }


    }
}
