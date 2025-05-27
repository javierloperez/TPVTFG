using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using NLog.Fluent;
using TPVFarmacia.Backend.Servicios;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;
using TVPFarmacia.MVVM.Base;

namespace TPVFarmacia.MVVM
{
    public class MVUsuario : MVBaseCRUD<Usuario>
    {

        private TpvbdContext _contexto;
        private Usuario _usuario;
        private UsuarioServicio _usuarioServicio;
        private UsuarioRole _usuarioRole;
        private UsuarioRoleServicio _usuarioRoleServicio;
        private Permiso _permiso;
        private PermisoServicio _permisoServicio;
        private Role _rol;
        private RoleServicio _rolServicio;

        public MVUsuario(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public List<Usuario> _listaUsuario { get; set; } = new List<Usuario>();
        public List<Permiso> _listaPermisos { get; set; } = new List<Permiso>();
        public List<Role> _listaRoles { get; set; } = new List<Role>();
        public List<UsuarioRole> _listaUsuarioRol { get; set; } = new List<UsuarioRole>();
        public async Task CargarUsuarioAsync()
        {
            _listaUsuario = (await _usuarioServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaUsuario));
        }
        public async Task CargarUsuarioRoleAsync()
        {
            _listaUsuarioRol = (await _usuarioRoleServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaUsuarioRol));
        }
        public async Task CargarPermisoAsync()
        {
            _listaPermisos = (await _permisoServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaPermisos));
        }
        public async Task CargarRolAsync()
        {
            _listaRoles = (await _rolServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaRoles));
        }

        public async Task Inicializa()
        {
            _usuario = new Usuario();
            _usuarioServicio = new UsuarioServicio(_contexto);
            _usuarioRole = new UsuarioRole();
            _usuarioRoleServicio = new UsuarioRoleServicio(_contexto);
            _permiso = new Permiso();
            _permisoServicio = new PermisoServicio(_contexto);
            _rol = new Role();
            _rolServicio = new RoleServicio(_contexto);

            servicio = _usuarioServicio;
            await CargarUsuarioAsync();
            await CargarUsuarioRoleAsync();
            await CargarPermisoAsync();
            await CargarRolAsync();

        }


        public Usuario _crearUsuario
        {
            get { return _usuario; }
            set { _usuario = value; OnPropertyChanged(nameof(_crearUsuario)); }
        }

        public bool guarda { get { return Task.Run(() => Add(_crearUsuario)).Result; } }
        public bool actualizar { get { return Task.Run(() => Update(_crearUsuario)).Result; } }


    }
}
