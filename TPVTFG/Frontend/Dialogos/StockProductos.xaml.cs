using MahApps.Metro.Controls;
using TPVTFG.Frontend.ControlUser;
using TPVTFG.MVVM;

namespace TPVTFG
{
    /// <summary>
    /// Lógica de interacción para StockProductos.xaml
    /// </summary>
    public partial class StockProductos : MetroWindow
    {
        public StockProductos(MVCategorias mv)
        {
            InitializeComponent();
            ControlStock controlStock = new ControlStock(mv);
            panelPrincipal.Children.Clear();
            panelPrincipal.Children.Add(controlStock);
        }
    }
}
