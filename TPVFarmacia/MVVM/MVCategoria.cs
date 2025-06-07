using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.Backend.Servicios;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.MVVM
{
    public class MVCategoria : MVBaseCRUD<Categoria>
    {

        TpvbdContext _contexto;
        Categoria _categoria;
        CategoriaServicio _categoriaServicio;

        /// <summary>
        /// Método que guarda una categoría en la base de datos.
        ///<returns> Devuelve true o false en función de si se ha guardado o no</returns> 
        /// </summary>
        public bool guarda { get { return Task.Run(() => Add(_crearCategoria)).Result; } }

        /// <summary>
        /// Variable que recoge los datos para crear una categoría o actualizarla
        /// </summary>
        public Categoria _crearCategoria
        {
            get { return _categoria; }
            set { _categoria = value; OnPropertyChanged(nameof(_crearCategoria)); }
        }


        public MVCategoria(TpvbdContext contexto)
        {
            _contexto = contexto;
        }
        /// <summary>
        /// Método que inicializa las variables y el servicio.
        /// </summary>
        /// <returns></returns>
        public async Task Inicializa()
        {
            _categoria = new Categoria();
            _categoriaServicio = new CategoriaServicio(_contexto);

            servicio = _categoriaServicio;

        }
    }
}
