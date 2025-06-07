using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NLog;
using Org.BouncyCastle.Asn1.Mozilla;
using TPVFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;

namespace TVPFarmacia.MVVM.Base
{
    public class MVOfertas : MVBaseCRUD<Oferta>
    {
        private TpvbdContext _contexto;
        private Oferta _oferta;
        private OfertaServicio _ofertaServicio;
        private Logger _logger;
        public List<Oferta> _listaOfertas { get; set; } = new List<Oferta>();

        public MVOfertas(TpvbdContext contexto)
        {
            _contexto = contexto;
        }
        /// <summary>
        /// Método para cargar las ofertas desde la base de datos de forma asíncrona.
        /// </summary>
        /// <returns></returns>
        public async Task CargarOfertasAsync()
        {
            _listaOfertas = (await _ofertaServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaOfertas));
        }

        /// <summary>
        /// Método que inicializa las variables, determina las fechas de las ofertas  y carga las ofertas desde la base de datos.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public async Task Inicializa(Logger logger)
        {
            _oferta = new Oferta();
            _oferta.OfertaFin = DateTime.Now;
            _oferta.OfertaInicio = DateTime.Now;
            _ofertaServicio = new OfertaServicio(_contexto);
            servicio = _ofertaServicio;
            _logger = logger;
            await CargarOfertasAsync();

        }

        /// <summary>
        /// Variable que recoge los datos para crear una oferta o actualizarla
        /// </summary>
        public Oferta _crearOferta
        {
            get { return _oferta; }
            set { _oferta = value; OnPropertyChanged(nameof(_crearOferta)); }
        }

        /// <summary>
        /// Método que guarda la oferta en la base de datos.
        /// </summary>
        public bool guarda { get { return Task.Run(() => Add(_crearOferta)).Result; } }

        /// <summary>
        /// Método que comprueba si una oferta es válida y está activa, devolviendo el porcentaje de descuento si es así, o 0 si no lo es.
        /// </summary>
        /// <param name="idOferta"></param>
        /// <returns></returns>
        public int ComprobarOfertas(int idOferta)
        {
            try
            {
                if(idOferta <= 0)
                {
                    return 0;
                }
                DateTime fechaHoy = DateTime.Now;
                Oferta oferta = new Oferta();
                oferta = _ofertaServicio.GetByIdAsync(idOferta).Result;

                if (oferta.OfertaInicio < fechaHoy && oferta.OfertaFin > fechaHoy)
                {
                    return (int)oferta.DescuentoPctj;

                }
                else
                {
                    _logger.Info($"Oferta con ID {idOferta} no está activa, la oferta expiró");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error al comprobar ofertas: {ex.Message}");
                return 0;
            }
        }

    }
}
