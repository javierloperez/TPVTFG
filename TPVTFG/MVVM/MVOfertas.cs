using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;

namespace TPVTFG.MVVM.Base
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

        public async Task Inicializa()
        {
            _oferta = new Oferta();
            _oferta.OfertaFin = DateTime.Now;
            _oferta.OfertaInicio = DateTime.Now;
            _ofertaServicio = new OfertaServicio(_contexto);

            servicio = _ofertaServicio;

        }


        public Oferta _crearOferta
        {
            get { return _oferta; }
            set { _oferta = value; OnPropertyChanged(nameof(_crearOferta)); }
        }

        public bool guarda { get { return Task.Run(() => Add(_crearOferta)).Result; } }

    }
}
