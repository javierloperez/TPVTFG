﻿using System.IO;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using NLog.Config;
using NLog;
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
        private string _tipoPago = "tarjeta"; 
        private Cliente _clienteElegido;
        private readonly string _nombreFarmacia = "Los geles hueles S.L";
        private Logger _logger;

        /// <summary>
        /// Constructor de la clase MainWindow.
        /// </summary>
        /// <param name="contexto">Contexto de la BD</param>
        /// <param name="usuario">Usuario con el que se ha registrado</param>
        public MainWindow(TpvbdContext contexto, Usuario usuario, Logger logger)
        {
            
            InitializeComponent();
            _contexto = contexto;
            _usuario = usuario;
            _ = Inicializa();
            _logger = logger;
            // Asignación del usuario a la ventana principal
            nombreUsuario.Content = _usuario.Nombre + " " + usuario.Apellidos;
            nombreFarmacia.Text = _nombreFarmacia;

        }


        public void CompruebaPermisos()
        {
            if (_usuario.UsuarioRole.Rol.Nombre.ToLower().Equals("empleado"))
            {

                btnStock.Visibility = Visibility.Hidden;
                btnVentas.Visibility = Visibility.Hidden;
                btnClientes.Visibility = Visibility.Hidden;
            }
            else
            {
                btnStock.Visibility = Visibility.Visible;
                btnVentas.Visibility = Visibility.Visible;
                btnClientes.Visibility = Visibility.Visible;
            }
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
                await _mvOfertas.Inicializa(_logger);

                _mvProducto = new MVProducto(_contexto);
                await _mvProducto.Inicializa(_logger,_mvOfertas, panelMedio, panelTicket, precioTotal, panelCategorias, panelInferior, precioConIva, porcentajeIva);

                _mvCategoria = new MVCategoria(_contexto);
                await _mvCategoria.Inicializa();

                _mvClientes = new MVClientes(_contexto);
                await _mvClientes.Inicializa();

                _mvUsuario = new MVUsuario(_contexto);
                await _mvUsuario.Inicializa();

                _mvVentas = new MVVentas(_contexto);
                await _mvVentas.Inicializa(_logger);

                _mvVentasProducto = new MVVentasProducto(_contexto);
                await _mvVentasProducto.Inicializa(_logger);

                DataContext = _mvClientes;
                CompruebaPermisos();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al inicializar la aplicación MainWindow.xaml.cs - Inicializa()");
            }
        }


        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de cerrar sesión, abre la ventana de login y cierra la ventana actual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de calculadora, abre la ventana de calculadora.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calculadora_Click(object sender, RoutedEventArgs e)
        {
            Calculadora calculadora = new Calculadora();
            calculadora.ShowDialog();

        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de stock, abre la ventana de stock de productos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            StockProductos stockProductos = new StockProductos(_mvProducto, _mvOfertas, _mvCategoria, this);
            stockProductos.ShowDialog();

        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de clientes, abre la ventana de lista de clientes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            ListaClientes lc = new ListaClientes(_mvClientes);
            lc.ShowDialog();
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de ventas, abre la ventana de lista de ventas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVentas_Click(object sender, RoutedEventArgs e)
        {
            ListaVentas lv = new ListaVentas(_mvVentas, _mvVentasProducto, _mvProducto);
            lv.ShowDialog();
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el tipo de pago efectivo, muestra los campos necesarios para introducir la cantidad recibida y el total a devolver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        cantidadRecibida.Text = "0€";
                        totalADevolver.Text = "0€";
                    }
                }

            }

        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el tipo de pago tarjeta, oculta los campos de cantidad recibida y total a devolver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCliente.SelectedItem is Cliente cliente)
            {
                _clienteElegido = cliente;
                txtNombreCliente.Text = "Cliente: " + cliente.Nombre;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el campo de cantidad recibida, abre una ventana para introducir la cantidad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cantidadRecibida_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            VentanaCantidad vc = new VentanaCantidad(_contexto, "normal", this);
            vc.ShowDialog();
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón de añadir venta, valida los datos introducidos y añade la venta a la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void añadirVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idVenta = 0;
                if (decimal.Parse(totalADevolver.Text.TrimEnd('€')) < 0)
                {
                    _logger.Warn("El total a devolver es negativo. Datos introducidos: {0}", totalADevolver.Text.TrimEnd('€'));
                    MessageBox.Show("El total a devolver no puede ser negativo. Por favor, compruebe los datos introducidos");
                    return;
                }
                idVenta = _mvVentas.AgregarVenta(_clienteElegido, decimal.Parse(precioConIva.Text.TrimEnd('€')), _usuario, _tipoPago, decimal.Parse(porcentajeIva.Text.TrimEnd('%')));
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
                    _mvProducto._lineasTicket.Remove(producto.Key);
                }
                GenerarTicket();
                LimpiarVentana();
                _logger.Info($"Venta añadida correctamente por el usuario {_usuario.Nombre} {_usuario.Apellidos} para el cliente {_clienteElegido.Nombre ?? "Estándar"} con ID de venta {idVenta}.");

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al añadir la venta en MainWindow.xaml.cs - añadirVenta_Click()");  
            }
        }

        /// <summary>
        ///  Método que genera un ticket en formato PDF con los datos de la venta actual, incluyendo productos, cantidades, precios y totales.
        /// </summary>
        private void GenerarTicket()
        {
            try
            {
                // Registrar fuente
                GlobalFontSettings.FontResolver = new CustomFontResolver();

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 11, XFontStyleEx.Regular);
                var fontTitle = new XFont("Arial", 20, XFontStyleEx.Underline);
                var boldFont = new XFont("Arial", 11, XFontStyleEx.Bold);
                int y = 0;

                // Título
                gfx.DrawString(_nombreFarmacia, fontTitle, XBrushes.Black, new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
                y += 50;

                // Datos
                gfx.DrawString($"Fecha: {DateTime.Now}", font, XBrushes.Black, 20, y);
                y += 20;

                gfx.DrawString($"Empleado: {_usuario.Nombre} {_usuario.Apellidos}", font, XBrushes.Black, 20, y);
                y += 20;

                gfx.DrawString($"Cliente: {_clienteElegido?.Nombre ?? "Sin cliente"}", font, XBrushes.Black, 20, y + 20);
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
                total = total + (total * int.Parse(porcentajeIva.Text.TrimEnd('%')) / 100);
                gfx.DrawString("IVA:", boldFont, XBrushes.Black, xPrecio - 100, y, XStringFormats.TopRight);
                gfx.DrawString($"{porcentajeIva.Text}", boldFont, XBrushes.Black, xPrecio, y, XStringFormats.TopRight);

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
                _logger.Info($"Ticket generado y guardado en: {guardarDialogo.FileName} por el usuario {_usuario.Nombre} {_usuario.Apellidos} para el cliente {_clienteElegido.Nombre ?? "Estándar"}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al generar el ticket en MainWindow.xaml.cs - GenerarTicket()");
            }
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el campo de porcentaje de IVA, abre una ventana para introducir el porcentaje de IVA.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void porcentajeIva_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            VentanaCantidad vc = new VentanaCantidad(_contexto, "iva", this);
            vc.ShowDialog();
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el texto del campo de porcentaje de IVA, actualiza el precio con IVA en función del porcentaje introducido.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void porcentajeIva_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                precioConIva.Text = (decimal.Parse(precioTotal.Text.TrimEnd('€')) * (1 + decimal.Parse(porcentajeIva.Text.TrimEnd('%')) / 100)).ToString("0.00") + "€";
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Método que limpia la ventana principal, reseteando todos los campos y valores a su estado inicial.
        /// </summary>
        public void LimpiarVentana()
        {
            try
            {
                _mvProducto.LimpiarStock();
                panelTicket.Children.Clear();
                porcentajeIva.Text = "10%";
                precioConIva.Text = string.Empty;
                _tipoPago = "tarjeta";
                _clienteElegido = new Cliente();
                totalADevolver.Text = string.Empty;
                precioTotal.Text = "0€";
                txtNombreCliente.Text = string.Empty;
                cbCliente.SelectedIndex = 0;
                cantidadRecibida.Text = "0€";
                efectivo.IsChecked = false;
                tarjeta.IsChecked = true;
                cantidadRecibida.Visibility = Visibility.Hidden;
                mensaje.Visibility = Visibility.Hidden;
                totalADevolver.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al limpiar la ventana en MainWindow.xaml.cs - LimpiarVentana()");
            }
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de añadir cliente, abre la ventana para agregar un nuevo cliente o editar uno existente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void añadirCliente_Click(object sender, RoutedEventArgs e)
        {
            AgregarCliente ac = new AgregarCliente(_mvClientes, false);
            ac.ShowDialog();
        }
        /// <summary>
        /// Evento que se ejecuta al hacer clic en el nombre de usuario, abre la ventana para editar los datos del usuario actual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nombreUsuario_Click(object sender, RoutedEventArgs e)
        {
            _mvUsuario._crearUsuario = _usuario;
            AgregarUsuario au = new AgregarUsuario(_contexto, _mvUsuario, nombreUsuario);
            au.ShowDialog();
        }


    }
}
