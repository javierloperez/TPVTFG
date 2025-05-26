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
using TVPFarmacia.Backend.Utiles;
using TVPFarmacia.MVVM;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.Frontend.Dialogos
{
    /// <summary>
    /// Clase que agrega un producto a la base de datos o lo edita si ya existe.
    /// </summary>
    public partial class AgregarProducto : MetroWindow
    {
        /// <summary>
        /// Declaración de variables privadas para almacenar el producto, el modelo de vista del producto, si se está editando y la ventana principal.
        /// </summary>
        private Producto _producto;
        private MVProducto _mvProducto;
        private bool _editar;
        private MainWindow _ventana;

        /// <summary>
        /// Constructor de la clase AgregarProducto.
        /// </summary>
        /// <param name="mv">Mv de producto</param>
        /// <param name="editar">Boolean para saber si hay que editar el producto</param>
        /// <param name="ventana">La ventana principal</param>
        public AgregarProducto(MVProducto mv, bool editar, MainWindow ventana)
        {
            InitializeComponent();
            _mvProducto = mv;
            DataContext = _mvProducto;
            _producto = new Producto();
            _mvProducto.btnGuardar = btnGuardar;
            _editar = editar;
            _ventana = ventana;
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de cancelar, cierra la ventana y resetea el producto a un nuevo objeto Producto vacío.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mvProducto._crearProducto = new Producto();

            this.Close();
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de guardar, valida los campos del producto y guarda o actualiza el producto según corresponda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            _mvProducto._crearProducto.Activado = "si";

            if (_mvProducto.IsValid(this))
            {
                if (!_editar)
                {
                    if (_mvProducto.guarda)
                    {
                        await this.ShowMessageAsync("Gestión crear prodcuto", "El producto se ha guardado correctamente");
                        DialogResult = true;
                        _mvProducto._crearProducto = new Producto();


                    }
                    else
                    {
                        await this.ShowMessageAsync("Gestión crear producto", "Error, algun campo esta incompleto o no es válido");


                    }
                }
                else
                {
                    if (_mvProducto.actualizar)
                    {
                        await this.ShowMessageAsync("Gestión actualizar prodcuto", "El producto se ha guardado correctamente");
                        DialogResult = true;

                    }
                    else
                    {
                        await this.ShowMessageAsync("Gestión actualizar producto", "Error, algun campo esta incompleto o no es válido");


                    }

                }
                //Limpiamos el panelMedio de la ventana principal para recargar la lista de productos
                _ventana.panelMedio.Children.Clear();
            }
            else
            {
                this.ShowMessageAsync("Gestión crear producto", "Tienes campos obligatorios sin rellenar correctamente");

            }

        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de seleccionar imagen, abre un diálogo para seleccionar una imagen y asigna la ruta de la imagen al producto.
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
                _mvProducto._crearProducto.RutaImagen = rutaImagen;
            }
        }

    }

}
