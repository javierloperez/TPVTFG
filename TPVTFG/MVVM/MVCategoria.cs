using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TPVTFG.Backend.Modelos;
using TPVTFG.Backend.Servicios;
using TPVTFG.MVVM.Base;

namespace TPVTFG.MVVM
{
    public class MVCategoria : MVBaseCRUD<Categoria>
    {

        TpvbdContext _contexto;
        Categoria _categoria;
        CategoriaServicio _categoriaServicio;
     
        public bool guarda { get { return Task.Run(() => Add(_crearCategoria)).Result; } }


        public Categoria _crearCategoria
        {
            get { return _categoria; }
            set { _categoria = value; OnPropertyChanged(nameof(_crearCategoria)); }
        }


        public MVCategoria(TpvbdContext contexto)
        {
            _contexto = contexto;
        }

        public async Task Inicializa()
        {
            _categoria = new Categoria();
            _categoriaServicio = new CategoriaServicio(_contexto);

            servicio = _categoriaServicio;

        }
    }
}
