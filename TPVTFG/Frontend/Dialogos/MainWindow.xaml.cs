
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;
using TPVTFG.Frontend.ControlUser;
using TPVTFG.Frontend.Dialogos;
using TPVTFG.MVVM;
using TPVTFG.MVVM.Base;

namespace TPVTFG.Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private TpvbdContext _contexto;
        private Usuario _usuario;
        private MVProducto _mvProducto;
        private MVOfertas _mvOfertas;
        private MVCategoria _mvCategoria;
        private MVClientes _mvClientes;
        private MVVentas _mvVentas;
        private MVVentasProducto _mvVentasProducto;
        private string _tipoPago;
        private Cliente _clienteElegido;

        public MainWindow(TpvbdContext contexto, Usuario usuario)
        {
            InitializeComponent();
            _contexto = contexto;
            _ = Inicializa();
            _usuario = usuario;
            nombreUsuario.Text = _usuario.Nombre + " " + usuario.Apellidos;
        }


        public async Task Inicializa()
        {
            _mvProducto = new MVProducto(_contexto);
            await _mvProducto.Inicializa(panelMedio, panelTicket, precioTotal, panelCategorias, panelInferior, precioConIva, porcentajeIva);

            _mvOfertas = new MVOfertas(_contexto);
            await _mvOfertas.Inicializa();

            _mvCategoria = new MVCategoria(_contexto);
            await _mvCategoria.Inicializa();

            _mvClientes = new MVClientes(_contexto);
            await _mvClientes.Inicializa();


            _mvVentas = new MVVentas(_contexto);
            await _mvVentas.Inicializa();

            _mvVentasProducto = new MVVentasProducto(_contexto);
            await _mvVentasProducto.Inicializa();

            DataContext = _mvClientes;

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
            StockProductos stockProductos = new StockProductos(_mvProducto, _mvOfertas, _mvCategoria);
            stockProductos.ShowDialog();

        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            ListaClientes lc = new ListaClientes(_mvClientes);
            lc.ShowDialog();
        }

        private void btnVentas_Click(object sender, RoutedEventArgs e)
        {

        }

        private void efectivo_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton tipo && tipo.Content.Equals("Efectivo"))
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
                    precio = _mvProducto.CogerPrecioProducto(producto.Key)*cantidad;
                    
                    _mvVentasProducto.InsertarVenta(producto.Key, cantidad,precio,idVenta);
                }
                LimpiarVentana();
            }
            catch (Exception ex)
            {

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
            panelTicket.Children.Clear();
            porcentajeIva.Clear();
            precioConIva.Text = string.Empty;
            _tipoPago = string.Empty;
            _clienteElegido = new Cliente();
            totalADevolver.Text = string.Empty;
            precioTotal.Text = string.Empty;
            txtNombreCliente.Text = string.Empty;
            cbCliente.SelectedIndex = 0;
            cantidadRecibida.Text = "0";
            efectivo.IsChecked = false;
            tarjeta.IsChecked = false;
            cantidadRecibida.Visibility = Visibility.Hidden;
            mensaje.Visibility = Visibility.Hidden;
            totalADevolver.Visibility = Visibility.Hidden;
            _mvProducto.LimpiarStock();
            _ = Inicializa();
        }
    }
}
