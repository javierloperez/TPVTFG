using MahApps.Metro.Controls;
using TPVTFG.Frontend.ControlUser;
using TPVTFG.MVVM;
using TPVTFG.MVVM.Base;

namespace TPVTFG
{
    /// <summary>
    /// Lógica de interacción para StockProductos.xaml
    /// </summary>
    public partial class StockProductos : MetroWindow
    {
        public StockProductos(MVProducto mv, MVOfertas mvOfertas,MVCategoria mvCategoria)
        {
            InitializeComponent();
            ControlStock controlStock = new ControlStock(mv, mvOfertas,mvCategoria);
            panelPrincipal.Children.Clear();
            panelPrincipal.Children.Add(controlStock);
        }
    }
}
