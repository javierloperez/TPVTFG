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


        public List<Usuario> _listaUsuario { get; set; } = new List<Usuario>();
        public List<Permiso> _listaPermisos { get; set; } = new List<Permiso>();
        public List<Role> _listaRoles { get; set; } = new List<Role>();
        public List<UsuarioRole> _listaUsuarioRol { get; set; } = new List<UsuarioRole>();
       
        
        public MVUsuario(TpvbdContext contexto)
        {
            _contexto = contexto;
        }
        /// <summary>
        /// Carga los usuarios de la base de datos y los almacena en la lista _listaUsuario
        /// </summary>
        /// <returns></returns>
        public async Task CargarUsuarioAsync()
        {
            _listaUsuario = (await _usuarioServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaUsuario));
        }
        /// <summary>
        /// Carga los roles de usuario y los almacena en la lista _listaUsuarioRol
        /// </summary>
        /// <returns></returns>
        public async Task CargarUsuarioRoleAsync()
        {
            _listaUsuarioRol = (await _usuarioRoleServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaUsuarioRol));
        }
        /// <summary>
        /// Carga los permisos de la base de datos y los almacena en la lista _listaPermisos
        /// </summary>
        /// <returns></returns>
        public async Task CargarPermisoAsync()
        {
            _listaPermisos = (await _permisoServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaPermisos));
        }
        /// <summary>
        /// Carga los roles de la base de datos y los almacena en la lista _listaRoles
        /// </summary>
        /// <returns></returns>
        public async Task CargarRolAsync()
        {
            _listaRoles = (await _rolServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaRoles));
        }

        /// <summary>
        /// Inicializa los servicios y las entidades necesarias para el manejo de usuarios, roles y permisos.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Variable que recoge los datos para crear un usuario o actualizarlo
        /// </summary>
        public Usuario _crearUsuario
        {
            get { return _usuario; }
            set { _usuario = value; OnPropertyChanged(nameof(_crearUsuario)); }
        }

        /// <summary>
        /// Método que guarda un usuario en la base de datos
        /// </summary>
        public bool guarda { get { return Task.Run(() => Add(_crearUsuario)).Result; } }
        /// <summary>
        /// Método que actualiza un usuario en la base de datos
        /// </summary>
        public bool actualizar { get { return Task.Run(() => Update(_crearUsuario)).Result; } }


    }
}
