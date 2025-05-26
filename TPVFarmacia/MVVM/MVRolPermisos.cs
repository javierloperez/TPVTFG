using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVFarmacia.Backend.Servicios;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.MVVM.Base;

namespace TPVFarmacia.MVVM
{
    public class MVRolPermisos : MVBaseCRUD<UsuarioRole>
    {
        private TpvbdContext _contexto;
        private UsuarioRole _rolUsuario;
        private UsuarioRoleServicio _rolUsuarioServicio;


        public MVRolPermisos(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public async Task Inicializa()
        {
            _rolUsuario = new UsuarioRole();
            _rolUsuarioServicio = new UsuarioRoleServicio(_contexto);

            servicio = _rolUsuarioServicio;

        }



    }
}
