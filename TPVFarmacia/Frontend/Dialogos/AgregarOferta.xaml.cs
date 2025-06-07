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
using Microsoft.Win32;
using TPVFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.MVVM;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.Frontend.Dialogos
{
    /// <summary>
    /// Clase que agrega una oferta a la base de datos.
    /// </summary>
    public partial class AgregarOferta : MetroWindow
    {
        /// <summary>
        /// Declaración del MV para gestionar las ofertas.
        /// </summary>
        private MVOfertas _mvOfertas;

        /// <summary>
        /// Constructor de la ventana AgregarOferta.
        /// </summary>
        /// <param name="mv">Mv de oferta</param>
        public AgregarOferta(MVOfertas mv)
        {
            InitializeComponent();
            _mvOfertas = mv;
            DataContext = _mvOfertas;
            _mvOfertas.btnGuardar = btnGuardar;
            _mvOfertas._crearOferta.OfertaFin = DateTime.UtcNow;
            _mvOfertas._crearOferta.OfertaInicio = DateTime.UtcNow;

        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón Cancelar, cierra la ventana y resetea la oferta creada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mvOfertas._crearOferta = new Oferta();
            this.Close();
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón Guardar, valida los campos y guarda la oferta si es válida.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(_mvOfertas._crearOferta.DescuentoPctj==0)
            {
                MessageBox.Show("El descuento no puede ser 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }else if (_mvOfertas._crearOferta.DescuentoPctj > 100)
            {
                MessageBox.Show("El descuento no puede ser mayor de 100", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_mvOfertas.IsValid(this))
            {

                if (_mvOfertas.guarda)
                {
                    MessageBox.Show("Gestión crear oferta", "La oferta se ha guardado correctamente", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    _mvOfertas._crearOferta = new Oferta();
                    dpFechaFin.SelectedDate = DateTime.UtcNow;
                    dpFechaInicio.SelectedDate = DateTime.UtcNow;

                }
                else
                {
                    MessageBox.Show("Gestión crear oferta", "Error, algun campo esta incompleto o no es válido",MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Gestión crear oferta", "Tienes campos obligatorios sin rellenar correctamente", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón Imagen, abre un diálogo para seleccionar una imagen y asigna la ruta a la oferta creada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Seleccionar imagen",
                Filter = "Imágenes (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string rutaImagen = openFileDialog.FileName;
                _mvOfertas._crearOferta.Fichero = rutaImagen;
            }
        }
    }
}
