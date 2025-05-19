
using System.Windows;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using TPVTFG.Backend.Modelos;
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
        private Usuario usuario;
        private MVProducto _mvCategorias;
        private MVOfertas _mvOfertas;
        private MVCategoria _mvCategoria;
        private MVClientes _mvClientes;
        public MainWindow(TpvbdContext contexto)
        {
            InitializeComponent();
            _contexto = contexto;
            _ = Inicializa();

            _mvCategorias.ListadoCategorias(panelCategorias);
        }


        public async Task Inicializa()
        {
            _mvCategorias = new MVProducto(_contexto);
            await _mvCategorias.Inicializa(panelMedio, panelTicket, precioTotal);

            _mvOfertas = new MVOfertas(_contexto);
            await _mvOfertas.Inicializa();

            _mvCategoria = new MVCategoria(_contexto);
            await _mvCategoria.Inicializa();

            _mvClientes = new MVClientes(_contexto);
            await _mvClientes.Inicializa();
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
            StockProductos stockProductos = new StockProductos(_mvCategorias, _mvOfertas,_mvCategoria);
            stockProductos.ShowDialog();

        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            ListaClientes lc = new ListaClientes(_mvClientes);
            lc.ShowDialog();
        }
    }
}