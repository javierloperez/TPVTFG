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

        /// <summary>
        /// Constructor para la ventana de cantidad, utilizada en la venta normal y para modificar cantidades de productos. También se crean los controles para poder sumar y restar cantidad manteniendo pulsado
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="sender"></param>
        /// <param name="mvCategorias"></param>
        /// <param name="cantidad"></param>
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
        /// <summary>
        /// Constructor auxiliar para usar el iva en la ventana de cantidad, se usa para modificar el porcentaje de iva de un producto. También se crean los controles para poder sumar y restar cantidad manteniendo pulsado
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="tipo"></param>
        /// <param name="ventana"></param>
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
        /// <summary>
        /// Inicia el temporizador de pulsado para incrementar o decrementar la cantidad según el botón presionado.
        /// </summary>
        /// <param name="isIncrement"></param>
        private void StartHold(bool isIncrement)
        {
            _isIncrementing = isIncrement;
            _holdTimer.Start();
        }

        /// <summary>
        /// Detiene los temporizadores de pulsado y repetición cuando se suelta el botón del ratón.
        /// </summary>
        private void StopHold()
        {
            _holdTimer.Stop();
            _repeatTimer.Stop();
        }
        /// <summary>
        /// Evento que se dispara cuando el temporizador de pulsado se activa. Inicia el temporizador de repetición para incrementar o decrementar la cantidad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HoldTimer_Tick(object sender, EventArgs e)
        {
            _holdTimer.Stop();
            _repeatTimer.Start();
        }

        /// <summary>
        /// Evento que se dispara cuando el temporizador de repetición se activa. Incrementa o decrementa la cantidad según el estado del botón.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Incrementa la cantidad en 1, siempre que no supere la cantidad máxima permitida.
        /// </summary>
        private void Increment()
        {
            if (_cantidad < _cantidadMax)
            {
                _cantidad++;
                txtCantidad.Text = _cantidad.ToString();
            }
        }
        /// <summary>
        /// Decrementa la cantidad en 1, siempre que no sea menor a 1.
        /// </summary>
        private void Decrement()
        {
            if (_cantidad > 1)
            {
                _cantidad--;
                txtCantidad.Text = _cantidad.ToString();
            }
        }
        /// <summary>
        /// Evento que se dispara al hacer clic en el botón de guardar. Dependiendo del tipo de uso, actualiza la cantidad a devolver, el porcentaje de IVA o añade un ticket a la venta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                if((int)Math.Round(_cantidad) > 100)
                {
                    _ventana.porcentajeIva.Text = "100%";
                }
                else
                {
                    _ventana.porcentajeIva.Text = ((int)Math.Round(_cantidad)).ToString() + "%";
                }
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
        /// <summary>
        /// Evento que se dispara al hacer clic en el botón de cancelar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_tipoUso))
            {
                _mvCategorias._actualizarCantidad = false;
            }
            this.Close();
        }

        /// <summary>
        /// Evento que se dispara al obtener el foco en el campo de cantidad. Limpia el campo y muestra el teclado numérico para introducir la cantidad deseada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCantidad_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCantidad.Text = string.Empty;
            TecladoNum teclado = new TecladoNum(txtCantidad, _cantidadMax);
            teclado.ShowDialog();
            if (string.IsNullOrEmpty(txtCantidad.Text)) txtCantidad.Text = "1";

            _cantidad = decimal.Parse(txtCantidad.Text);
        }


    }
}
