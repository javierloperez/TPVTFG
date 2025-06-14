﻿using System;
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
using NLog;
using TPVFarmacia.Backend.Modelos;
using TPVFarmacia.MVVM;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.MVVM.Base;

namespace TPVFarmacia.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para AgregarUsuario.xaml
    /// </summary>
    public partial class AgregarUsuario : MetroWindow
    {
        private TpvbdContext _contexto;
        private Usuario _usuario;
        private MVUsuario _mvUsuario;
        private Button _nombreUsuario;
        public AgregarUsuario(TpvbdContext contexto, MVUsuario mvUsuario, Button nombreUsuario)
        {
            InitializeComponent();
            _contexto = contexto;
            _mvUsuario = mvUsuario;
            _nombreUsuario = nombreUsuario;
            DataContext = _mvUsuario;
            _mvUsuario.btnGuardar = btnGuardar;
        }
        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de guardar, valida los campos del formulario y guarda el usuario si es válido.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_mvUsuario.IsValid(this))
            {

                if (_mvUsuario.actualizar)
                {
                    await this.ShowMessageAsync("Gestión usuarios", "El usuario se ha guardado correctamente");
                    DialogResult = true;
                    _nombreUsuario.Content = tbNombre.Text + " " + tbApellidos.Text;
                    _mvUsuario._crearUsuario = new Usuario();

                }
                else
                {
                    await this.ShowMessageAsync("Gestión usuarios", "Error, algun campo esta incompleto o no es válido");
                }
            }
            else
            {
                
                this.ShowMessageAsync("Gestión usuarios", "Tienes campos obligatorios sin rellenar correctamente");
            }
        }

        /// <summary>
        /// Evento que se ejecuta al pulsar el botón de cancelar, limpia el objeto usuario y cierra la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _mvUsuario._crearUsuario = new Usuario();
            this.Close();
        }
        /// <summary>
        /// Evento que se ejecuta al introducir texto en el campo de teléfono, permite solo la entrada de dígitos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTelefono_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.ToString().All(char.IsDigit) == false;
        }
    }
}
