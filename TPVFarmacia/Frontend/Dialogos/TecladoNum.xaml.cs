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

namespace TVPFarmacia.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para TecladoNum.xaml
    /// </summary>
    public partial class TecladoNum : MetroWindow
    {
        private TextBox _textBoxDestino;
        private int maxCantidad;
        public TecladoNum(TextBox destino, int maxCant)
        {
            InitializeComponent();
            _textBoxDestino = destino;
            maxCantidad = maxCant;
        }

        private void Boton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button boton)
            {
                _textBoxDestino.Text += boton.Content.ToString();
            }
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            if (_textBoxDestino.Text.Length > 0)
                _textBoxDestino.Text = _textBoxDestino.Text.Substring(0, _textBoxDestino.Text.Length - 1);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            
            if((int)Math.Round(decimal.Parse(_textBoxDestino.Text))>maxCantidad)
            {
                _textBoxDestino.Text = maxCantidad.ToString();
            }
            else if (decimal.Parse(_textBoxDestino.Text)<1)
            {
                _textBoxDestino.Text = "1";
            }
                this.Close();
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (_textBoxDestino.Text.Length > 0)
            {
                if (!_textBoxDestino.Text.Contains(","))
                {
                _textBoxDestino.Text+=",";
                }
            }
            
        }
    }
}
