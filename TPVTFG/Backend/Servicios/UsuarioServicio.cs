
using TPVTFG.Backend.Modelos;

namespace TPVTFG.Backend.Servicios
{
     /// <summary>
     /// Clase que contiene las reglas de negocio propias de la tabla usuario
     /// </summary>
    public class UsuarioServicio : ServicioGenerico<Usuario>
    {
        /// <summary>
        /// Contexto de la BD
        /// </summary>
        private TpvbdContext contexto;
        /// <summary>
        /// Constante que identifica el objeto profesor
        /// </summary>
        private const int PROFESOR = 1;
        /// <summary>
        /// Se almacena el usuario que ha iniciado sesión
        /// </summary>
        public Usuario usuLogin { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Contexto de la BD</param>
        public UsuarioServicio(TpvbdContext context) : base(context)
        {
            contexto = context;
        }
        /// <summary>
        /// Método que comprueba las credenciales de la BD
        /// </summary>
        /// <param name="user">Usuario introducido</param>
        /// <param name="pass">Contraseña introducida</param>
        /// <returns>True en caso de que la validación sea correcta, False en caso contrario</returns>
        public async Task<Boolean> Login(String user, String pass)
        {
            Boolean correcto = false;
            try
            {
                // Obtenemos el óbjeto usuario
                usuLogin = await GetUsuarioPorNombre(user);
            } catch (Exception e)
            {
                logger.Error("Login. Error al obtener el usuario" + e.InnerException);
                logger.Error(e.StackTrace);
            }
            // Coprobamos si el objeto es distinto de null y su
            // usuario y contraseña son iguales a los introducidos
            // entonces devolvemos true, en cualquier otro caso devolvemos false
            if (usuLogin != null && usuLogin.Login.Equals(user) && usuLogin.Password.Equals(pass))
            {
                correcto = true;
            }
            return correcto;
        }       
        /// <summary>
        /// Comprueba si en la base de datos existe un usuario con ese login. 
        /// El login de un usuario debe de ser único
        /// </summary>
        /// <param name="usu">Nombre de usuario</param>
        /// <returns>True en caso de que sea único, false en caso de que ya exista</returns>
        public bool UsuarioUnico(string usu)
        {
            bool unico = true;
            // Buscamos el usuario, si la lista no está vacía, entonces
            // ponemos la variable a false
            if(contexto.Set<Usuario>().Where(u => u.Login.Equals(usu)).Count() > 0)
            {
                unico = false;
            }
            return unico;
        }
        /// <summary>
        /// Obtiene un usuario por su nombre
        /// </summary>
        /// <param name="nom">Nombre de usuario</param>
        /// <returns>El objeto usuario en caso de encontrarlo o null en casoo de no hacerlo</returns>
        public async Task<Usuario> GetUsuarioPorNombre(string nom)
        {
            IEnumerable<Usuario> usuarios;
            Usuario usu = null;
            usuarios = await FindAsync(u => u.Login == nom);
            if (usuarios !=  null)
            {
                usu = usuarios.FirstOrDefault();
            }
            return usu;
        }
        /// <summary>
        /// Obtiene los profesores de la tabla de usuarios
        /// </summary>
        /// <returns>Lista con los profesores</returns>
        

        
    }
}