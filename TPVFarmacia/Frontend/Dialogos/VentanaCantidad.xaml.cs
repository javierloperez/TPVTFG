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
using System.Windows.Threading;
using MahApps.Metro.Controls;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Frontend.Dialogos;
using TVPFarmacia.MVVM;

namespace TVPFarmacia.Frontend
{
    /// <summary>
    /// Lógica de interacción para VentanaCantidad.xaml
    /// </summary>
    public partial class VentanaCantidad : MetroWindow
    {
        private DispatcherTimer _holdTimer;
        private DispatcherTimer _repeatTimer;
        private bool _isIncrementing = false;
        public decimal CantidadSeleccionada => _cantidad;
        public bool _modificar { get; set; } = false;

        private decimal _cantidad = 1;
        private int _cantidadMax;
        MVProducto _mvCategorias;
        TpvbdContext _contexto;
        object _sender;
        private string _tipoUso = string.Empty;
        private MainWindow _ventana;

        public VentanaCantidad(TpvbdContext contexto, object sender, MVProducto mvCategorias, int cantidad)
        {
            InitializeComponent();
            _contexto = contexto;
            _mvCategorias = mvCategorias;
            _sender = sender;
            _cantidad = cantidad;
            txtCantidad.Text = _cantidad.ToString();
            if (sender is Button btn && btn.Tag is Producto producto)
            {
                _cantidadMax = _mvCategorias.ObtenerStockDisponible(producto.Id, producto.Cantidad);
            }
            if (sender is Button cant && cant.Tag is Producto producto1)
            {
                _cantidadMax = _mvCategorias.ObtenerStockDisponible(producto1.Id, producto1.Cantidad);
            }


            _holdTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _holdTimer.Tick += HoldTimer_Tick;

            _repeatTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _repeatTimer.Tick += RepeatTimer_Tick;

            btnSumar.PreviewMouseLeftButtonDown += (s, e) => StartHold(true);
            btnSumar.PreviewMouseLeftButtonUp += (s, e) => StopHold();
            btnSumar.Click += (s, e) => Increment();

            btnRestar.PreviewMouseLeftButtonDown += (s, e) => StartHold(false);
            btnRestar.PreviewMouseLeftButtonUp += (s, e) => StopHold();
            btnRestar.Click += (s, e) => Decrement();
        }

        public VentanaCantidad(TpvbdContext contexto, string tipo, MainWindow ventana)
        {
            InitializeComponent();
            _tipoUso = tipo;
            _ventana = ventana;
            _contexto = contexto;
            _cantidadMax = 9999;
            txtCantidad.Text = _cantidad.ToString();

            _holdTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _holdTimer.Tick += HoldTimer_Tick;

            _repeatTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _repeatTimer.Tick += RepeatTimer_Tick;

            btnSumar.PreviewMouseLeftButtonDown += (s, e) => StartHold(true);
            btnSumar.PreviewMouseLeftButtonUp += (s, e) => StopHold();
            btnSumar.Click += (s, e) => Increment();

            btnRestar.PreviewMouseLeftButtonDown += (s, e) => StartHold(false);
            btnRestar.PreviewMouseLeftButtonUp += (s, e) => StopHold();
            btnRestar.Click += (s, e) => Decrement();
        }
        private void StartHold(bool isIncrement)
        {
            _isIncrementing = isIncrement;
            _holdTimer.Start();
        }

        private void StopHold()
        {
            _holdTimer.Stop();
            _repeatTimer.Stop();
        }
        private void HoldTimer_Tick(object sender, EventArgs e)
        {
            _holdTimer.Stop();
            _repeatTimer.Start();
        }

        private void RepeatTimer_Tick(object sender, EventArgs e)
        {
            if (_isIncrementing)
            {
                Increment();
            }
            else
            {
                Decrement();
            }
        }

        private void Increment()
        {
            if (_cantidad < _cantidadMax)
            {
                _cantidad++;
                txtCantidad.Text = _cantidad.ToString();
            }
        }

        private void Decrement()
        {
            if (_cantidad > 1)
            {
                _cantidad--;
                txtCantidad.Text = _cantidad.ToString();
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_tipoUso.Equals("normal"))
            {
                try
                {
                    _ventana.totalADevolver.Text = (_cantidad - decimal.Parse(_ventana.precioConIva.Text.TrimEnd('€'))).ToString("0.00") + "€";
                    _ventana.cantidadRecibida.Text = _cantidad.ToString("0.00") + "€";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Aun no hay un ticket con precio");
                }

            }
            else if (_tipoUso.Equals("iva")){
                _ventana.porcentajeIva.Text = ((int)Math.Round(_cantidad)).ToString()+"%";
            }
            else
            {
                if (!_modificar)
                {
                    _mvCategorias.AnyadirTicket((int)Math.Round(_cantidad), _sender);
                }
                else
                {
                    _mvCategorias._actualizarCantidad = true;
                }
            }
            this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_tipoUso))
            {
                _mvCategorias._actualizarCantidad = false;
            }
            this.Close();
        }

        private void txtCantidad_GotFocus(object sender, RoutedEventArgs e)
        {
            TecladoNum teclado = new TecladoNum(txtCantidad, _cantidadMax);
            teclado.ShowDialog();
            _cantidad = decimal.Parse(txtCantidad.Text);
        }


    }
}
