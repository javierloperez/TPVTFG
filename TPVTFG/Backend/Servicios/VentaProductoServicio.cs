using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVTFG.Backend.Modelos;

namespace TPVTFG.Backend.Servicios
{
    public class VentaProductoServicio : ServicioGenerico<VentaProducto>
    {
        public VentaProductoServicio(TpvbdContext contexto) : base(contexto)
        {
        }
    }
}
