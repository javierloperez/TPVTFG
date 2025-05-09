using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVTFG.Backend.Modelos;

namespace TPVTFG.Backend.Servicios
{
    public class ProductoServicio : ServicioGenerico<Producto>
    {
        public ProductoServicio(TpvbdContext contexto) :base(contexto) 
        {
        }
    }
}
