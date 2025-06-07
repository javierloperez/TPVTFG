using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using NLog;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;
using TVPFarmacia.Frontend;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.MVVM
{
    public class MVVentas : MVBaseCRUD<Venta>
    {
        private TpvbdContext _contexto;
        private Venta _venta;
        private VentaServicio _ventaServicio;
        private ClienteServicio _clienteServicio;
        private UsuarioServicio _usuarioServicio;
        private Logger _logger; 
        public List<Venta> _listaVentas { get; set; } = new List<Venta>();
        public List<Usuario> _listaUsuarios { get; set; } = new List<Usuario>();
        public List<Cliente> _listaClientes { get; set; } = new List<Cliente>();


        /// <summary>
        /// Método que devuelve true si se ha podido guardar la venta, false en caso contrario.
        /// </summary>
        public bool guarda { get { return Task.Run(() => Add(_crearVenta)).Result; } }
        /// <summary>
        /// Método que devuelve true si se ha podido actualizar la venta, false en caso contrario.
        /// </summary>
        public bool borrar { get { return Task.Run(() => Delete(_crearVenta)).Result; } }

        /// <summary>
        /// Variable que recoge los datos para crear una venta o actualizarla
        /// </summary>
        public Venta _crearVenta
        {
            get { return _venta; }
            set { _venta = value; OnPropertyChanged(nameof(_crearVenta)); }
        }

        /// <summary>
        /// Carga los clientes activos desde el servicio de clientes.
        /// </summary>
        /// <returns></returns>
        public async Task CargarClientesAsync()
        {
            var clientes = await _clienteServicio.GetAllAsync();
            _listaClientes = clientes
                .Where(c => c.Activado.Equals("si", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        /// <summary>
        /// Carga las ventas desde el servicio de ventas y las almacena en la lista _listaVentas.
        /// </summary>
        /// <returns></returns>
        public async Task CargarVentasAsync()
        {
            _listaVentas = (await _ventaServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaVentas));
        }
        /// <summary>
        /// Carga los usuarios desde el servicio de usuarios y los almacena en la lista _listaUsuarios.
        /// </summary>
        /// <returns></returns>
        public async Task CargarUsuariosAsync()
        {
            _listaUsuarios = (await _usuarioServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaUsuarios));
        }
        public MVVentas(TpvbdContext contexto)
        {
            _contexto = contexto;

        }

        /// <summary>
        /// Método que inicializa el MVVentas, cargando los servicios y las listas de clientes, ventas y usuarios.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public async Task Inicializa(Logger logger)
        {
            _logger = logger;
            _venta = new Venta();
            _ventaServicio = new VentaServicio(_contexto);
            _clienteServicio = new ClienteServicio(_contexto);
            _usuarioServicio = new UsuarioServicio(_contexto);
            servicio = _ventaServicio;
            await CargarClientesAsync();
            await CargarVentasAsync();
            await CargarUsuariosAsync();
        }

        /// <summary>
        /// Método que agrega una venta a la base de datos.
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="total"></param>
        /// <param name="usuario"></param>
        /// <param name="tipoCobro"></param>
        /// <param name="iva"></param>
        /// <returns>Devuelve el identificador de la venta</returns>
        public int AgregarVenta(Cliente cliente, decimal total, Usuario usuario, string tipoCobro, decimal iva)
        {
            try
            {
                _crearVenta.Iva = (int)iva;
                _crearVenta.ClienteId = cliente.Dni;
                _crearVenta.Total = total;
                _crearVenta.EmpleadoId = usuario.Id;
                _crearVenta.TipoCobro = tipoCobro;
                _crearVenta.Fecha = DateTime.Now;

                if (guarda)
                {

                    _crearVenta = new Venta();

                    return _ventaServicio.GetLastId();
                }
                else
                {
                    _logger.Error("Error al crear la venta, faltan campos por rellenar");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("AgregarVenta. Error al agregar la venta: " + ex.Message);
                return -1;
            }
        }
    }
}
