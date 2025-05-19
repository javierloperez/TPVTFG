using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVTFG.Backend.Modelos;

namespace TPVTFG.Backend.Servicios
{
    public class ClienteServicio : ServicioGenerico<Cliente>
    {
        public ClienteServicio(TpvbdContext contexto) : base(contexto)
        {
        }
    }
}
