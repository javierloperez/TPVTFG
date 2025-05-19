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
using TPVTFG.Frontend.ControlUser;
using TPVTFG.MVVM.Base;
using TPVTFG.MVVM;

namespace TPVTFG.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para ListaClientes.xaml
    /// </summary>
    public partial class ListaClientes : MetroWindow
    {
        public ListaClientes(MVClientes mvClientes)
        {
            InitializeComponent();
            ControlClientes cl = new ControlClientes(mvClientes);
            panelPrincipal.Children.Clear();
            panelPrincipal.Children.Add(cl);
        }
    }
}
