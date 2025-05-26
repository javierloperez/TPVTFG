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
    public class MVVentas : MVBaseCRUD<Venta>
    {
        TpvbdContext _contexto;
        Venta _venta;
        VentaServicio _ventaServicio;
        ClienteServicio _clienteServicio;
        UsuarioServicio _usuarioServicio;
        public List<Venta> _listaVentas { get; set; } = new List<Venta>();
        public List<Usuario> _listaUsuarios { get; set; } = new List<Usuario>();

        public IEnumerable<Cliente> _listaClientes { get { return Task.Run(_clienteServicio.GetAllAsync).Result.Where(c => c.Activado.ToLower().Equals("si")); } }

        public bool guarda { get { return Task.Run(() => Add(_crearVenta)).Result; } }


        public Venta _crearVenta
        {
            get { return _venta; }
            set { _venta = value; OnPropertyChanged(nameof(_crearVenta)); }
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

        public async Task Inicializa()
        {
            _venta = new Venta();
            _ventaServicio = new VentaServicio(_contexto);
            _clienteServicio = new ClienteServicio(_contexto);
            _usuarioServicio = new UsuarioServicio(_contexto);
            servicio = _ventaServicio;
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
                    MessageBox.Show("Error al crear al cliente, faltan campos por rellenar");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Faltan campos por rellenar ");
                return -1;
            }
        }
    }
}
