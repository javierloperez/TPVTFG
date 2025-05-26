using MahApps.Metro.Controls;
using TVPFarmacia.Frontend;
using TVPFarmacia.Frontend.ControlUser;
using TVPFarmacia.MVVM;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia
{
    /// <summary>
    /// Lógica de interacción para StockProductos.xaml
    /// </summary>
    public partial class StockProductos : MetroWindow
    {
        
        public StockProductos(MVProducto mv, MVOfertas mvOfertas,MVCategoria mvCategoria,MainWindow ventana)
        {
            InitializeComponent();
            ControlStock controlStock = new ControlStock(mv, mvOfertas,mvCategoria,ventana);
            panelPrincipal.Children.Clear();
            panelPrincipal.Children.Add(controlStock);
        }
    }
}
