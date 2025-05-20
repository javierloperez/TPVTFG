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
using TPVTFG.MVVM;
using TPVTFG.MVVM.Base;

namespace TPVTFG.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para AgregarOferta.xaml
    /// </summary>
    public partial class AgregarOferta : MetroWindow
    {
        private MVOfertas _mvOfertas;
        
        public AgregarOferta(MVOfertas mv)
        {
            InitializeComponent();
            _mvOfertas= mv;
            DataContext = _mvOfertas;
            _mvOfertas.btnGuardar = btnGuardar;
            dpFechaFin.SelectedDate = DateTime.Today;
            dpFechaInicio.SelectedDate = DateTime.Today;

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mvOfertas._crearOferta = new Oferta();
            this.Close();
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_mvOfertas.IsValid(this))
            {

                if (_mvOfertas.guarda)
                {
                    await this.ShowMessageAsync("Gestión crear oferta", "La oferta se ha guardado correctamente");
                    DialogResult = true;
                    _mvOfertas._crearOferta = new Oferta();

                }
                else
                {
                    await this.ShowMessageAsync("Gestión crear oferta", "Error, algun campo esta incompleto o no es válido");


                }
            }
            else
            {
                this.ShowMessageAsync("Gestión crear oferta", "Tienes campos obligatorios sin rellenar correctamente");

            }
        }
    }
}
