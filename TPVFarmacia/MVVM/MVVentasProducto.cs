using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;
using TVPFarmacia.Frontend;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.MVVM
{
    public class MVVentasProducto : MVBaseCRUD<VentaProducto>
    {
        private TpvbdContext _contexto;
        private VentaProducto _venta;
        private VentaProductoServicio _ventaServicio;
        private Logger _logger;

        public IEnumerable<VentaProducto> _listaVentas { get { return Task.Run(_ventaServicio.GetAllAsync).Result; } }
        public bool guarda { get { return Task.Run(() => Add(_crearVentaP)).Result; } }

        public bool borrar;
        public VentaProducto _crearVentaP
        {
            get { return _venta; }
            set { _venta = value; OnPropertyChanged(nameof(_crearVentaP)); }
        }


        public MVVentasProducto(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public void BorrarVentasID(int ids)
        {
            foreach (var ventaProducto in _listaVentas)
            {
                if (ventaProducto.VentaId == ids)
                {
                    borrar = Task.Run(() => _ventaServicio.DeleteAsync(ventaProducto)).Result;
                }
            }
        }

        public async Task Inicializa(Logger logger)
        {
            _logger = logger;
            _venta = new VentaProducto();
            _ventaServicio = new VentaProductoServicio(_contexto);

            servicio = _ventaServicio;

        }

        public void InsertarVenta(int idProd, int cantidad, decimal precio, int idVenta)
        {

            _crearVentaP.VentaId = idVenta;
            _crearVentaP.ProductoId = idProd;
            _crearVentaP.Cantidad = cantidad;
            _crearVentaP.Precio = (double)precio;

            if (guarda)
            {

            }
            else
            {
                _logger.Error("Error al crear la venta del producto completa");
            }
        }

        /// <summary>
        /// Método que recoge la lista de productos de una venta concreta.
        /// </summary>
        /// <param name="idVenta"></param>
        /// <returns></returns>
        public Dictionary<int,int> RecogerListaProductos(int idVenta)
        {
            Dictionary<int,int> idProductos = new Dictionary<int, int>();

            foreach(var ventaProducto in _listaVentas)
            {
                if (ventaProducto.VentaId == idVenta)
                {
                    idProductos.Add(ventaProducto.ProductoId, ventaProducto.Cantidad);
                }
            }

            return idProductos;
        }
    }
}
