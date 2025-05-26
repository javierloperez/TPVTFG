using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;

namespace TPVFarmacia.Backend.Servicios
{
    public class UsuarioRoleServicio : ServicioGenerico<UsuarioRole>
    {
        public UsuarioRoleServicio(TpvbdContext contexto) : base(contexto)
        {
        }
    }
}
