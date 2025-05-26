using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVPFarmacia.Backend.Modelos;

namespace TVPFarmacia.Backend.Servicios
{
    public class ProductoServicio : ServicioGenerico<Producto>
    {
        public ProductoServicio(TpvbdContext contexto) :base(contexto) 
        {
        }
    }
}
