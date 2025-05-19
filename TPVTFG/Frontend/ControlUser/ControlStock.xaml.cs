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
using TPVTFG.Backend.Modelos;
using TPVTFG.Frontend.Dialogos;
using TPVTFG.MVVM;
using TPVTFG.MVVM.Base;

namespace TPVTFG.Frontend.ControlUser
{
    /// <summary>
    /// Lógica de interacción para ControlStock.xaml
    /// </summary>
    public partial class ControlStock : UserControl
    {

        private MVProducto _mvProducto;
        private MVOfertas _mvOfertas;
        private MVCategoria _mvCategoria;
        public ControlStock(MVProducto mvProducto, MVOfertas mvOfertas, MVCategoria mvCategoria)
        {
            InitializeComponent();
            _mvProducto = mvProducto;
            _mvOfertas = mvOfertas;
            _mvCategoria = mvCategoria;
            DataContext = _mvProducto;
        }

        private void AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            AgregarProducto ap = new AgregarProducto(_mvProducto, false);
            ap.ShowDialog();
        }

        private void AgregarOferta_Click(object sender, RoutedEventArgs e)
        {
            AgregarOferta ao = new AgregarOferta(_mvOfertas);
            ao.ShowDialog();
        }

        private void AgregarCategoria_Click(object sender, RoutedEventArgs e)
        {
            AgregarCategoria ac = new AgregarCategoria(_mvCategoria);
            ac.ShowDialog();
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            _mvProducto._crearProducto = (Backend.Modelos.Producto)dgAñadirProducto.SelectedItem;

            _mvProducto._crearProducto.Activado = "no";

            if (_mvProducto.actualizar)
            {
                MessageBox.Show("Producto eliminado correctamente", "Gestión productosW");
            }
            else
            {
                MessageBox.Show("Error al intentar eliminar producto", "Gestión productos");
            }

            
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            _mvProducto._crearProducto= (Producto)dgAñadirProducto.SelectedItem;

            Producto articuloAux = _mvProducto.Clonar;
            AgregarProducto ap = new AgregarProducto(_mvProducto,true);
            ap.ShowDialog();

            if (ap.DialogResult.Equals(true))
            {
                dgAñadirProducto.Items.Refresh();
                _mvProducto._crearProducto= new Producto();
            }
            else
            {
                _mvProducto._crearProducto= articuloAux;
                dgAñadirProducto.SelectedItem = articuloAux;

            }
        }
    }
}
