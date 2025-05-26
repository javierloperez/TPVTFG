using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVPFarmacia.Backend.Modelos;

namespace TVPFarmacia.Backend.Servicios
{
    public class CategoriaServicio : ServicioGenerico<Categoria>
    {
        public CategoriaServicio(TpvbdContext contexto) :base(contexto) 
        {
        }
    }
}
