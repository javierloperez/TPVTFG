using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
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


        public IEnumerable<Cliente> _listaClientes { get { return Task.Run(_clienteServicio.GetAllAsync).Result.Where(c => c.Activado.ToLower().Equals("si")); } }
        public bool guarda
        {
            get
            {
                var resultado = Task.Run(() => Add(_crearCliente)).Result;
                Task.Run(RecargarListaClientesAsync); 
                return resultado;
            }
        }
        public bool borrar { get { return Task.Run(() => Delete(_crearCliente)).Result; } }
        public bool actualizar { get { return Task.Run(() => Update(_crearCliente)).Result; } }
        public Cliente Clonar { get { return (Cliente)_cliente.Clone(); } }

        public async Task RecargarListaClientesAsync()
        {
            var productos = await _clienteServicio.GetAllAsync();
            var productosActivos = productos
                .Where(p => p.Activado.ToLower() == "si")
                .ToList();

            _listaClientesParaFiltro = new ListCollectionView(productosActivos);
            _listaClientesParaFiltro.Filter = predicadoFiltro;

            OnPropertyChanged(nameof(listaClientesFiltro));
        }


        private ListCollectionView _listaClientesParaFiltro;
        public ListCollectionView listaClientesFiltro => _listaClientesParaFiltro;
        private Predicate<object> predicadoFiltro;
        private List<Predicate<Cliente>> criterios;

        private Predicate<Cliente> criterioBusqueda;

        private string _nombreC;

        public string filtroNombre
        {
            get { return _nombreC; }
            set
            {
                _nombreC = value; OnPropertyChanged(nameof(filtroNombre));
                if (_nombreC == null)
                {

                    _nombreC = "";
                }
            }
        }
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
            _listaClientesParaFiltro = new ListCollectionView((await _clienteServicio.GetAllAsync()).ToList());
            criterios = new List<Predicate<Cliente>>();
            predicadoFiltro = new Predicate<object>(FiltroCriterios);
            InicializaCriterios();
        }

        private void AddCriterios()
        {
            criterios.Clear();

            if (!string.IsNullOrEmpty(filtroNombre))
            {
                criterios.Add(criterioBusqueda);
            }


        }
        private void InicializaCriterios()
        {
            criterioBusqueda = new Predicate<Cliente>(m => m.Nombre!= null && m.Nombre.ToLower().StartsWith(filtroNombre.ToLower()));
            
        }

        public void Filtrar()
        {
            AddCriterios();
            listaClientesFiltro.Filter = predicadoFiltro;
        }

        private bool FiltroCriterios(object item)
        {
            bool correcto = true;
            Cliente cliente = (Cliente)item;
            if (criterios != null)
            {
                correcto = criterios.TrueForAll(x => x(cliente));
            }
            return correcto;
        }
    }
}
