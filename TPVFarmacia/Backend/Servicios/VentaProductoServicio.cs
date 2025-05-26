using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVPFarmacia.Backend.Modelos;

namespace TVPFarmacia.Backend.Servicios
{
    public class VentaProductoServicio : ServicioGenerico<VentaProducto>
    {
        public VentaProductoServicio(TpvbdContext contexto) : base(contexto)
        {
        }
    }
}
