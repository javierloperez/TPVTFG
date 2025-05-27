using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;

namespace TPVFarmacia.Backend.Servicios
{
    public class RoleServicio : ServicioGenerico<Role>
    {
        public RoleServicio(TpvbdContext contexto) : base(contexto)
        {
        }
    }
}
