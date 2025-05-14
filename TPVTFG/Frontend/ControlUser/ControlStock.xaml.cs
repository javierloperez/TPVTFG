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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TPVTFG.Frontend.Dialogos;
using TPVTFG.MVVM;

namespace TPVTFG.Frontend.ControlUser
{
    /// <summary>
    /// Lógica de interacción para ControlStock.xaml
    /// </summary>
    public partial class ControlStock : UserControl
    {

        private MVCategorias _mvCategorias;
        public ControlStock(MVCategorias mvCategorias)
        {
            InitializeComponent();
            _mvCategorias = mvCategorias;
            DataContext = _mvCategorias;
        }

        private void AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            AgregarProducto ap = new AgregarProducto(_mvCategorias);
            ap.ShowDialog();
        }
    }
}
