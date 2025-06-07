using System;
using System.Collections.Generic;
using System.Data;
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

namespace TVPFarmacia.Frontend
{
    /// <summary>
    /// Clase que representa una calculadora simple.
    /// </summary>
    public partial class Calculadora : MetroWindow
    {
        
        private string input = "";// Cadena que almacena la entrada de numeros
        private bool borrar = false; // Indica si se debe borrar la entrada al siguiente número
        private bool isIcono = false; // Indica si el último botón presionado es un icono
      
        public Calculadora()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Manejador de eventos para el clic en los botones de la calculadora, comprueba el tipo de icono y añade al input el símbolo de operación correspondiente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var content = (sender as Button).Content;

            if (content is MaterialDesignThemes.Wpf.PackIcon icon)
            {
                isIcono = true;
                switch (icon.Kind.ToString().ToLower())
                {
                    case "slashforward":
                        input += "/";
                        break;
                    case "multiplication":
                        input += "*";
                        break;
                    case "horizontalline":
                        input += "-";
                        break;
                    case "plus":
                        input += "+";
                        break;
                    case "equal":
                        isIcono = false;
                        Calculate();
                        return;
                    case "erase":
                        //Comprobamos si hay caráceres en la entrada antes de borrar    
                        if (!string.IsNullOrEmpty(input))
                        {
                            input = input.Substring(0, input.Length - 1);
                            UpdateDisplay();
                        }
                        return;
                    case "comma":
                        input += ",";
                        break;
                }
            }
            else
            {
                string value = content.ToString();
                if (value == "C")
                {
                    input = "";
                }
                else
                {
                    if (borrar == true && isIcono == false)
                    {
                        input = "";
                        borrar = false;
                    }
                    input += value;
                    isIcono = false;
                }
            }

            UpdateDisplay();
        }

        /// <summary>
        /// Actualiza la visualización de la calculadora con el valor actual de la entrada.
        /// </summary>
        private void UpdateDisplay()
        {
            txtNumeros.Text = input;
        }

        /// <summary>
        /// Método que calcula el resultado de la expresión matemática ingresada en la calculadora.
        /// </summary>
        private void Calculate()
        {
            try
            {
                var result = new DataTable().Compute(input.Replace(",", "."), null);
                input = result.ToString();
                borrar = true;
            }
            catch (Exception)
            {
                input = "Error";
                borrar = true;
            }

            UpdateDisplay();
        }
    }

}
