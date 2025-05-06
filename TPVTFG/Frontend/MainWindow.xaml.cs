
using MahApps.Metro.Controls;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TPVTFG.Backend.Modelos;

namespace TPVTFG.Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private TpvbdContext _contexto;
        private Usuario usuario;
        
        public MainWindow(TpvbdContext contexto)
        {
            InitializeComponent();
            _contexto = contexto;
            //mvmodeloArticulo = new MVModeloArticulo(contexto);
        }
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool ConectarBD()
        {
            bool correcto = true;
            _contexto = new TpvbdContext();
            try
            {
                _contexto.Database.OpenConnection();
            }
            catch (Exception ex)
            {
                correcto = false;
                MessageBox.Show("Conexion de la base de datos", "Ups!!!");
            }
            return correcto;
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
    }
}