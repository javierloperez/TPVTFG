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
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.MVVM;

namespace TVPFarmacia.Frontend.Dialogos
{
    /// <summary>
    /// Clase que representa la ventana de diálogo para agregar una nueva categoría.
    /// </summary>
    public partial class AgregarCategoria : MetroWindow
    {
        /// <summary>
        /// Declaración de variables para manejar la lógica de la categoría y los productos.
        /// </summary>
        private MVCategoria _mvCategorias;
        private MVProducto _mvProductos;

        /// <summary>
        /// Constructor de la clase AgregarCategoria.
        /// </summary>
        /// <param name="mvCategorias">Mv de categoria</param>
        /// <param name="mvProductos"> Mv Producto</param>
        public AgregarCategoria(MVCategoria mvCategorias, MVProducto mvProductos)
        {
            InitializeComponent();
            _mvCategorias = mvCategorias;
            DataContext = _mvCategorias;
            _mvCategorias.btnGuardar = btnGuardar;
            _mvProductos = mvProductos;
        }

        /// <summary>
        /// Manejador del evento Click del botón Guardar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            //Comprobar si los campos son válidos antes de guardar la categoría.
            if (_mvCategorias.IsValid(this))
            {

                if (_mvCategorias.guarda)
                {
                    await this.ShowMessageAsync("Gestión crear categoria", "La categoria se ha guardado correctamente");
                    _mvCategorias._crearCategoria = new Categoria();
                    await _mvProductos.CargarCategoriasAsync();
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

        /// <summary>
        /// Manejador del evento Click del botón Cancelar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mvCategorias._crearCategoria = new Categoria();
            this.Close();
        }

        /// <summary>
        /// Manejador del evento Click del botón para selecionar una imagen desde el equipo y recoger la ruta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Seleccionar imagen",
                //Filtro para seleccionar imágenes de tipo PNG, JPG o JPEG
                Filter = "Imágenes (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string rutaImagen = openFileDialog.FileName;
                _mvCategorias._crearCategoria.RutaImagen = rutaImagen;
            }
        }
    }
}
