using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;
using TVPFarmacia.Frontend;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.MVVM
{
    public class MVVentasProducto : MVBaseCRUD<VentaProducto>
    {
        TpvbdContext _contexto;
        VentaProducto _venta;
        VentaProductoServicio _ventaServicio;


        public IEnumerable<VentaProducto> _listaVentas { get { return Task.Run(_ventaServicio.GetAllAsync).Result; } }
        public bool guarda { get { return Task.Run(() => Add(_crearVenta)).Result; } }


        public VentaProducto _crearVenta
        {
            get { return _venta; }
            set { _venta = value; OnPropertyChanged(nameof(_crearVenta)); }
        }


        public MVVentasProducto(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public async Task Inicializa()
        {
            _venta = new VentaProducto();
            _ventaServicio = new VentaProductoServicio(_contexto);

            servicio = _ventaServicio;

        }

        public void InsertarVenta(int idProd, int cantidad, decimal precio, int idVenta)
        {

            _crearVenta.VentaId = idVenta;
            _crearVenta.ProductoId = idProd;
            _crearVenta.Cantidad = cantidad;
            _crearVenta.Precio = (double)precio;

            if (guarda)
            {

            }
            else
            {
                MessageBox.Show("Error al crear la venta del producto completa");
            }
        }

    }
}
