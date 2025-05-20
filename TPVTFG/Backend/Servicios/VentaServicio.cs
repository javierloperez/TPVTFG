using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVTFG.Backend.Modelos;

namespace TPVTFG.Backend.Servicios
{
    public class VentaServicio : ServicioGenerico<Venta>
    {
        private TpvbdContext _contexto;
        public VentaServicio(TpvbdContext contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public int GetLastId()
        {
            int id = 0;
            Venta art = _contexto.Set<Venta>().OrderByDescending(a => a.Id).FirstOrDefault();
            if (art != null) { id = art.Id; }
            return id;
        }
    }
}
