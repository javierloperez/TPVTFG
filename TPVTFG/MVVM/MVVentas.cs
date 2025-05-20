using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;
using TPVTFG.Frontend;
using TPVTFG.MVVM.Base;

namespace TPVTFG.MVVM
{
    public class MVVentas : MVBaseCRUD<Venta>
    {
        TpvbdContext _contexto;
        Venta _venta;
        VentaServicio _ventaServicio;
        VentaProducto _ventaProducto;

        public IEnumerable<Venta> _listaVentas { get { return Task.Run(_ventaServicio.GetAllAsync).Result; } }
        public bool guarda { get { return Task.Run(() => Add(_crearVenta)).Result; } }


        public Venta _crearVenta
        {
            get { return _venta; }
            set { _venta = value; OnPropertyChanged(nameof(_crearVenta)); }
        }


        public MVVentas(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public async Task Inicializa()
        {
            _venta = new Venta();
            _ventaServicio = new VentaServicio(_contexto);

            servicio = _ventaServicio;

        }

        public int AgregarVenta(Cliente cliente, decimal total, Usuario usuario, string tipoCobro, decimal iva)
        {
            try
            {
                _crearVenta.Iva = (int)iva;
                _crearVenta.ClienteId = cliente.Dni;
                _crearVenta.Cliente = cliente;
                _crearVenta.Total = total;
                _crearVenta.Empleado = usuario;
                _crearVenta.EmpleadoId = usuario.Id;
                _crearVenta.TipoCobro = tipoCobro;
                _crearVenta.Fecha = DateTime.Now;



                if (guarda)
                {

                    MessageBox.Show("Cliente creado correctamente");
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
