using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;
using TPVTFG.MVVM.Base;

namespace TPVTFG.MVVM
{
    public class MVClientes : MVBaseCRUD<Cliente>
    {
        TpvbdContext _contexto;
        Cliente _cliente;
        ClienteServicio _clienteServicio;


        public IEnumerable<Cliente> _listaClientes { get { return Task.Run(_clienteServicio.GetAllAsync).Result; } }
        public bool guarda { get { return Task.Run(() => Add(_crearCliente)).Result; } }


        public Cliente _crearCliente
        {
            get { return _cliente; }
            set { _cliente = value; OnPropertyChanged(nameof(_crearCliente)); }
        }


        public MVClientes(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public async Task Inicializa()
        {
            _cliente = new Cliente();
            _clienteServicio = new ClienteServicio(_contexto);

            servicio = _clienteServicio;

        }
    }
}
