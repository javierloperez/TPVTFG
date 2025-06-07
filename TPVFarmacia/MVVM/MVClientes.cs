using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.MVVM
{
    public class MVClientes : MVBaseCRUD<Cliente>
    {
        TpvbdContext _contexto;
        Cliente _cliente;
        ClienteServicio _clienteServicio;
        private ListCollectionView _listaClientesParaFiltro;
        public ListCollectionView listaClientesFiltro => _listaClientesParaFiltro;
        private Predicate<object> predicadoFiltro;
        private List<Predicate<Cliente>> criterios;

        private Predicate<Cliente> criterioBusqueda;

        private string _nombreC;

        /// <summary>
        /// Lista de clientes que se cargan al iniciar la aplicación.
        /// </summary>
        public IEnumerable<Cliente> _listaClientes { get { return Task.Run(_clienteServicio.GetAllAsync).Result.Where(c => c.Activado.ToLower().Equals("si")); } }

        /// <summary>
        /// Método que guarda  un cliente en la base de datos y recarga la lista de clientes.
        /// </summary>
        public bool guarda
        {
            get
            {
                var resultado = Task.Run(() => Add(_crearCliente)).Result;
                Task.Run(RecargarListaClientesAsync);
                return resultado;
            }
        }
        /// <summary>
        /// Método que borra un cliente de la base de datos
        /// </summary>
        public bool borrar { get { return Task.Run(() => Delete(_crearCliente)).Result; } }

        /// <summary>
        /// Método que actualiza un cliente en la base de datos
        /// </summary>
        public bool actualizar { get { return Task.Run(() => Update(_crearCliente)).Result; } }

        /// <summary>
        /// Método que clona un cliente para poder editarlo sin modificar el original.
        /// </summary>
        public Cliente Clonar { get { return (Cliente)_cliente.Clone(); } }

        /// <summary>
        /// Método que recarga la lista de clientes filtrados según los criterios establecidos.
        /// </summary>
        /// <returns></returns>
        public async Task RecargarListaClientesAsync()
        {
            var productos = await _clienteServicio.GetAllAsync();
            var productosActivos = productos
                .Where(p => p.Activado.ToLower().Equals("si"))
                .ToList();

            _listaClientesParaFiltro = new ListCollectionView(productosActivos);
            _listaClientesParaFiltro.Filter = predicadoFiltro;

            OnPropertyChanged(nameof(listaClientesFiltro));
        }



        /// <summary>
        /// Propiedad que filtra los clientes por nombre.
        /// </summary>
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

        /// <summary>
        /// Variable que recoge los datos para crear un cliente o actualizarlo
        /// </summary>
        public Cliente _crearCliente
        {
            get { return _cliente; }
            set { _cliente = value; OnPropertyChanged(nameof(_crearCliente)); }
        }


        public MVClientes(TpvbdContext contexto)
        {
            _contexto = contexto;

        }

        /// <summary>
        /// Método que inicializa el servicio de cliente y la lista de clientes para filtrar
        /// </summary>
        /// <returns></returns>
        public async Task Inicializa()
        {
            _cliente = new Cliente();
            _clienteServicio = new ClienteServicio(_contexto);

            servicio = _clienteServicio;
            _listaClientesParaFiltro = new ListCollectionView((await _clienteServicio.GetAllAsync()).ToList());
            criterios = new List<Predicate<Cliente>>();
            predicadoFiltro = new Predicate<object>(FiltroCriterios);
            InicializaCriterios();
            Task.Run(RecargarListaClientesAsync);

        }

        /// <summary>
        /// Método que añade los criterios de búsqueda a la lista de criterios para filtrar los clientes.
        /// </summary>
        private void AddCriterios()
        {
            criterios.Clear();

            if (!string.IsNullOrEmpty(filtroNombre))
            {
                criterios.Add(criterioBusqueda);
            }


        }

        /// <summary>
        /// Método que inicializa los criterios de búsqueda para filtrar los clientes.
        /// </summary>
        private void InicializaCriterios()
        {
            criterioBusqueda = new Predicate<Cliente>(m => m.Nombre != null && m.Nombre.ToLower().StartsWith(filtroNombre.ToLower()));

        }

        /// <summary>
        /// Método que añade los criterios de búsqueda a la lista de criterios y filtra la lista de clientes según esos criterios.
        /// </summary>
        public void Filtrar()
        {
            AddCriterios();
            listaClientesFiltro.Filter = predicadoFiltro;
        }
        /// <summary>
        /// Método que comprueba los criterios de búsqueda para filtrar los clientes.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
