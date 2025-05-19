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
using TPVTFG.MVVM;

namespace TPVTFG.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para AgregarCategoria.xaml
    /// </summary>
    public partial class AgregarCategoria : MetroWindow
    {
        private MVCategoria _mvCategorias;
        public AgregarCategoria(MVCategoria mvCategorias)
        {
            InitializeComponent();
            _mvCategorias = mvCategorias;
            DataContext = _mvCategorias;
            _mvCategorias.btnGuardar = btnGuardar;
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_mvCategorias.IsValid(this))
            {

                if (_mvCategorias.guarda)
                {
                    await this.ShowMessageAsync("Gestión crear categoria", "La categoria se ha guardado correctamente");
                    DialogResult = true;

                }
                else
                {
                    await this.ShowMessageAsync("Gestión crear categoria", "Error, algun campo esta incompleto o no es válido");


                }
            }
            else
            {
                this.ShowMessageAsync("Gestión crear categoria", "Tienes campos obligatorios sin rellenar correctamente");

            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
