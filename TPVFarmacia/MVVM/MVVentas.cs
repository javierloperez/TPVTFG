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


        
        public bool guarda { get { return Task.Run(() => Add(_crearVenta)).Result; } }
        public bool borrar { get { return Task.Run(() => Delete(_crearVenta)).Result; } }


        public Venta _crearVenta
        {
            get { return _venta; }
            set { _venta = value; OnPropertyChanged(nameof(_crearVenta)); }
        }

        public async Task CargarClientesAsync()
        {
            var clientes = await _clienteServicio.GetAllAsync();
            _listaClientes = clientes
                .Where(c => c.Activado.Equals("si", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task CargarVentasAsync()
        {
            _listaVentas = (await _ventaServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaVentas));
        }

        public async Task CargarUsuariosAsync()
        {
            _listaUsuarios = (await _usuarioServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaUsuarios));
        }
        public MVVentas(TpvbdContext contexto)
        {
            _contexto = contexto;

        }

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
