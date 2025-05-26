using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
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


        public MVOfertas(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public List<Oferta> _listaOfertas { get; set; } = new List<Oferta>();
        public async Task CargarOfertasAsync()
        {
            _listaOfertas = (List<Oferta>)await _ofertaServicio.GetAllAsync();
            OnPropertyChanged(nameof(_listaOfertas));
        }

        public async Task Inicializa()
        {
            _oferta = new Oferta();
            _oferta.OfertaFin = DateTime.Now;
            _oferta.OfertaInicio = DateTime.Now;
            _ofertaServicio = new OfertaServicio(_contexto);
            await CargarOfertasAsync();
            servicio = _ofertaServicio;

        }


        public Oferta _crearOferta
        {
            get { return _oferta; }
            set { _oferta = value; OnPropertyChanged(nameof(_crearOferta)); }
        }

        public bool guarda { get { return Task.Run(() => Add(_crearOferta)).Result; } }

        public int ComprobarOfertas(int idOferta)
        {
            DateTime fechaHoy = DateTime.Now;
            Oferta oferta = new Oferta();
            oferta = _ofertaServicio.GetByIdAsync(idOferta).Result;

            if(oferta.OfertaInicio<fechaHoy && oferta.OfertaFin > fechaHoy)
            {
            return (int)oferta.DescuentoPctj;

            }
            else
            {
                return 0;
            }
        }

    }
}
