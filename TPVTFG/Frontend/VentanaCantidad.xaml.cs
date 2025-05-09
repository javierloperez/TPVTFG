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
using TPVTFG.Backend.Modelos;
using TPVTFG.MVVM;

namespace TPVTFG.Frontend
{
    /// <summary>
    /// Lógica de interacción para VentanaCantidad.xaml
    /// </summary>
    public partial class VentanaCantidad : MetroWindow
    {
        private DispatcherTimer _holdTimer;
        private DispatcherTimer _repeatTimer;
        private bool _isIncrementing = false;

        public int cantidad { get; private set; } = 1;
        MVCategorias _mvCategorias;
        TpvbdContext _contexto;
        object _sender;
        public VentanaCantidad(TpvbdContext contexto, object sender,MVCategorias mvCategorias)
        {
            InitializeComponent();
            _contexto = contexto;
            _mvCategorias = mvCategorias;
            _sender = sender;

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
            cantidad++;
            TxtCantidad.Text = cantidad.ToString();
        }

        private void Decrement()
        {
            if (cantidad > 1)
            {
                cantidad--;
                TxtCantidad.Text = cantidad.ToString();
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
             _mvCategorias.AnyadirTicket(cantidad, _sender);
            this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
