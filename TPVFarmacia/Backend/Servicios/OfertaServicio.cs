using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Modelos;

namespace TVPFarmacia.Backend.Servicios
{
    public class OfertaServicio : ServicioGenerico<Oferta>
    {
        public OfertaServicio(TpvbdContext contexto) : base(contexto)
        {
        }
    }
}
