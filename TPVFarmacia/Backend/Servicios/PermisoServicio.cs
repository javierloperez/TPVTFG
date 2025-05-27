using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;

namespace TPVFarmacia.Backend.Servicios
{
    public class PermisoServicio : ServicioGenerico<Permiso>
    {
        public PermisoServicio(TpvbdContext contexto) : base(contexto)
        {
        }
    }
}
