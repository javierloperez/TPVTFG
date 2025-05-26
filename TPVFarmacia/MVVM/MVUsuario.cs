using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Fluent;
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


        public MVUsuario(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public List<Usuario> _listaUsuario { get; set; } = new List<Usuario>();
        public async Task CargarUsuarioAsync()
        {
            _listaUsuario = (await _usuarioServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaUsuario));
        }

        public async Task Inicializa()
        {
            _usuario = new Usuario();
            _usuarioServicio = new UsuarioServicio(_contexto);
            await CargarUsuarioAsync();
            servicio = _usuarioServicio;

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
