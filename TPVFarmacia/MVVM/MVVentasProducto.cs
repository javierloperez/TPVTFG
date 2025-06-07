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

        /// <summary>
        /// Propiedad que guarda una venta de producto en la base de datos. 
        /// </summary>
        public bool guarda { get { return Task.Run(() => Add(_crearVentaP)).Result; } }
        /// <summary>
        /// Propiedad que indica si se va a borrar una venta de producto.
        /// </summary>
        public bool borrar;

        /// <summary>
        /// Variable que recoge los datos para crear una ventaProductos o actualizarla
        /// </summary>
        public VentaProducto _crearVentaP
        {
            get { return _venta; }
            set { _venta = value; OnPropertyChanged(nameof(_crearVentaP)); }
        }


        public MVVentasProducto(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Método que borra una venta de productos de la base de datos a partir del ID de la venta.
        /// </summary>
        /// <param name="ids"></param>
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

        /// <summary>
        /// Método que inicializa el MVVentasProducto con un logger y un contexto de base de datos.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public async Task Inicializa(Logger logger)
        {
            _logger = logger;
            _venta = new VentaProducto();
            _ventaServicio = new VentaProductoServicio(_contexto);

            servicio = _ventaServicio;

        }

        /// <summary>
        /// Método que inserta una venta de producto en la base de datos.
        /// </summary>
        /// <param name="idProd">ID del producto</param>
        /// <param name="cantidad">Cantidad del producto</param>
        /// <param name="precio">Precio de los productos</param>
        /// <param name="idVenta">ID de la venta</param>
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
