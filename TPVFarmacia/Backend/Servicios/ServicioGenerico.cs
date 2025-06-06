using TVPFarmacia.Backend.Modelos;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Linq.Expressions;

namespace TVPFarmacia.Backend.Servicios
{
    public class ServicioGenerico<T> : IServicioGenerico<T> where T : class
    {
        /// <summary>
        /// Contexto de conexión con la base de datos
        /// </summary>
        private readonly TpvbdContext _context;
        /// <summary>
        /// Objeto que nos permite acceder a los objetos que representan las
        /// tablas de la base de datos
        /// </summary>
        private readonly DbSet<T> _dbSet;
        /// <summary>
        /// Obtenemos el objeto para guardar información
        /// de las operaciones de la BD
        /// </summary>
        internal static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">contexto de la base de datos</param>
        public ServicioGenerico(TpvbdContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Inserta un objeto en la BD de forma asíncrona
        /// </summary>
        /// <param name="entidad">Objeto a guardar</param>
        /// <returns>Devuelve el resultado de la operación</returns>
        public async Task<bool> AddAsync(T entidad)
        {
            bool resultado = true;
            try
            {
                // Inserta la entidad
                await _dbSet.AddAsync(entidad);
                // Finaliza la transacción con la BD
                await _context.SaveChangesAsync();
                // Guarda la información en el Log
                logger.Info("Objeto " + entidad.GetType() + " insertado correctamente.");
            }
            catch (Exception ex)
            {
                // Guarda el error en el Log
                ErrorLog("Error al insertar la entidad.",ex);
                resultado = false;
            } 
            // Devuelve el resultado
            return resultado;
        }

        /// <summary>
        /// Realiza una consulta de los elementos de una tabla
        /// </summary>
        /// <returns>Devuelve la lista con los registros de la tabla.
        /// En caso de error, devuelve una lista vacía</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                // Realiza la consulta
                var entities = await _dbSet.ToListAsync();  
                // Devuelve la lista de entidades
                return entities;
            }
            catch (Exception ex)
            {
                // Captura el error y lo guarda en el Log
                ErrorLog("Error al leer las entidades.",ex);
                // Devuelve una lista vacía en caso de error
                return Enumerable.Empty<T>();
            }
        }
        /// <summary>
        /// Consulta si un objeto existe por su identificador
        /// </summary>
        /// <param name="id">Identificador del objeto</param>
        /// <returns>Devuelve el objeto en caso de encontrarlo, o null en caso contrario</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                // Realizamos la consulta
                var entity = await _dbSet.FindAsync(id);
                // Si no es nula entonces guardamos la información en el log
                if (entity != null)
                {
                    logger.Info("Entidad encontrada.");
                } //  En caso de que no sea nula, añadimos una advertencia en el log
                else
                {
                    logger.Warn("Entidad no encontrada.");
                }
                return entity;
            } // En caso de error lo reflejamos en el Log
            catch (Exception ex)
            {
                ErrorLog("Error al buscar la entidad.",ex);
                return null;
            }
        }

        /// <summary>
        /// Actualiza un registro en la base de datos de forma asíncrona
        /// </summary>
        /// <param name="entity">Registro o entidad a actualizar</param>
        /// <returns>Devuelve true si la operación ha ido correctamente
        /// false en caso de que haya habido algún problema</returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            // Variable donde guardamos el resultado de la operación
            bool resultado = true;
            try
            {
                // Realizamos el update
                _dbSet.Attach(entity);
                // Finalizamos la transacción
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                // Guardamos la información en el Log
                logger.Info("Entidad actualizada con éxito.");
            }
            catch (Exception ex)
            {
                // Reflejamos el error en el Log
                ErrorLog("Error al actualizar la entidad.",ex);
                resultado = false;
            }
            // Devolvemos el resultado
            return resultado;
        }

        /// <summary>
        /// Borramos uns entidad de forma asíncrona
        /// </summary>
        /// <param name="id">Identificador de la entidad que queremos borrar</param>
        /// <returns>Estado de la operación, true en caso correcto, false en caso de error</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            bool resultado = true;
            try
            {
                // Buscamos la entidad
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                { // En caso de encontrarla
                    // Borramos la entidad
                    _dbSet.Remove(entity);
                    // Finalizamos la transcción
                    await _context.SaveChangesAsync();
                    // Guardamos el resultado en el Log
                    logger.Info("Entidad borrada con éxito.");
                }
                else
                {
                    // No hemos encontrado la entidad
                    logger.Warn("Entidad no encontrada.");
                    resultado = false;
                }
            }
            catch (Exception ex)
            {
                // En caso de error, lo guardamos en el Log
                ErrorLog("Error al borrar la entidad", ex);
                resultado = false;
            }
            return resultado;
        }
        public async Task<bool> DeleteAsync(T id)
        {
            bool resultado = true;
            try
            {
                if (id != null)
                { // En caso de encontrarla
                    // Borramos la entidad
                    _dbSet.Remove(id);
                    // Finalizamos la transcción
                    await _context.SaveChangesAsync();
                    // Guardamos el resultado en el Log
                    logger.Info("Entidad borrada con éxito.");
                }
                else
                {
                    // No hemos encontrado la entidad
                    logger.Warn("Entidad no encontrada.");
                    resultado = false;
                }
            }
            catch (Exception ex)
            {
                // En caso de error, lo guardamos en el Log
                ErrorLog("Error al borrar la entidad", ex);
                resultado = false;
            }
            return resultado;
        }

        /// <summary>
        /// Filtramos una consulta de una tabla según un criterio.
        /// </summary>
        /// <param name="predicate">Criterio de filtrado</param>
        /// <returns>Aquellos registros que lo cumplan
        /// Serán devueltos en forma de lista</returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                // Realizamos el filtrado en función de la condición/es
                var entities = await _dbSet.Where(predicate).ToListAsync();
                logger.Info("Entidades filtradas con éxito.");
                return entities;
            }
            catch (Exception ex)
            {
                // En caso de error, lo guardamos en el Log
                ErrorLog("Error al filtrar las entidades",ex);
                return Enumerable.Empty<T>();
            }
        }
        private void ErrorLog(string mensaje, Exception ex)
        {
            logger.Error(mensaje + "\n" + ex.Message);
            logger.Error(ex.InnerException);
            logger.Error(ex.StackTrace);
        }
    }
}
