
using System.Windows;
using MahApps.Metro.Controls;
using TPVTFG.Backend.Modelos;
using TPVTFG.MVVM;

namespace TPVTFG.Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private TpvbdContext _contexto;
        private Usuario usuario;
        private MVCategorias _mvCategorias;
        public MainWindow(TpvbdContext contexto)
        {
            InitializeComponent();
            _contexto = contexto;
            _ = Inicializa();

            _mvCategorias.ListadoCategorias(panelCategorias);
        }


        public async Task Inicializa()
        {
            _mvCategorias = new MVCategorias(_contexto);
            await _mvCategorias.Inicializa(panelMedio, panelTicket, precioTotal);
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
            calculadora.Show();

        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            StockProdcutos stockProdcutos = new StockProdcutos();
            stockProdcutos.Show();

        }
    }
}