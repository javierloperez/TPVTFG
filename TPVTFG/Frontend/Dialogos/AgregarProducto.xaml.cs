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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Utiles;
using TPVTFG.MVVM;

namespace TPVTFG.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    public partial class AgregarProducto : MetroWindow
    {

        private Producto _producto;
        private MVCategorias _mvCategorias;
        public AgregarProducto(MVCategorias mv)
        {
            InitializeComponent();
            _mvCategorias = mv;
            DataContext = _mvCategorias;
            _producto = new Producto();
            _mvCategorias.btnGuardar = btnGuardar;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_mvCategorias.IsValid(this))
            {
                
                if (_mvCategorias.guarda)
                {
                    await this.ShowMessageAsync("Gestión crear" +
                        " artículo", "El modelo artículo se ha guardado correctamente");
                    DialogResult = true;

                }
                else
                {
                    await this.ShowMessageAsync("Gestión crear artículo", "Error, algun campo esta incompleto o no es válido");


                }
            }
            else
            {
                this.ShowMessageAsync("Gestión crear artículo", "Tienes campos obligatorios sin rellenar correctamente");

            }
        }

        private void btnCrearOferta_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
