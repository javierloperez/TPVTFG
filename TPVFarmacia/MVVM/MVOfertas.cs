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

        public MVOfertas(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public List<Oferta> _listaOfertas { get; set; } = new List<Oferta>();
        public async Task CargarOfertasAsync()
        {
            _listaOfertas = (await _ofertaServicio.GetAllAsync()).ToList();
            OnPropertyChanged(nameof(_listaOfertas));
        }

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


        public Oferta _crearOferta
        {
            get { return _oferta; }
            set { _oferta = value; OnPropertyChanged(nameof(_crearOferta)); }
        }

        public bool guarda { get { return Task.Run(() => Add(_crearOferta)).Result; } }

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
