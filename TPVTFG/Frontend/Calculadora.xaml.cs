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

namespace TPVTFG.Frontend
{
    /// <summary>
    /// Lógica de interacción para Calculadora.xaml
    /// </summary>
    public partial class Calculadora : MetroWindow
    {
        private string input = "";
        private bool borrar = false;
        private bool isIcono = false;
        public Calculadora()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var content = (sender as Button).Content;

            // Si es PackIcon, agarrar el Kind
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
                        input = input.Substring(0, input.Length - 1);
                        UpdateDisplay();
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

        private void UpdateDisplay()
        {
            txtNumeros.Text = input;
        }

        private void Calculate()
        {
            try
            {
                // Usamos DataTable.Compute para evaluar (¡cuidado en apps serias!)
                var result = new DataTable().Compute(input.Replace(",", "."), null);
                input = Convert.ToString(result);
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
