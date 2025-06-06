using System.ComponentModel;
using System.Windows;
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
        private MVProducto _mvProducto;
        public StockProductos(MVProducto mv, MVOfertas mvOfertas,MVCategoria mvCategoria,MainWindow ventana)
        {
            InitializeComponent();
            ControlStock controlStock = new ControlStock(mv, mvOfertas,mvCategoria,ventana);
            panelPrincipal.Children.Clear();
            panelPrincipal.Children.Add(controlStock);
            this.Closing += Ventana_Closing;
            _mvProducto = mv;
        }

        private async void Ventana_Closing(object sender, CancelEventArgs e)
        {
            await _mvProducto.RecargarListaProductosAsync();
        }
    }
}
