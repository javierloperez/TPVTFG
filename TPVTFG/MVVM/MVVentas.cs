using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;
using TPVTFG.MVVM.Base;

namespace TPVTFG.MVVM
{
    public class MVVentas : MVBaseCRUD<Venta>
    {
        TpvbdContext _contexto;
        Venta _venta;
        VentaServicio _ventaServicio;


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
    }
}
