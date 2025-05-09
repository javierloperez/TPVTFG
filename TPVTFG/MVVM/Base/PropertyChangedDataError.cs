using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TPVTFG.MVVM.Base
{
    public class PropertyChangedDataError : INotifyPropertyChanged, IDataErrorInfo
    {
        // Implementa la interfaz INotifyPropertyChanged
        // Permite tener sincronizados los valores de una propiedad con el lelemento correspondiente de la interfaz
        #region Property Changed
        /// <summary>
        /// evento que se activa al modificar una propiedad de la clase
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Manejador del evento que se activa al modificar una propiedad
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad que se modifica</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        // Implementa la interfaz IDataErrorInfo
        // Nos permite realizar comprobaciones de los errores que pueda tener la información introducida por el usuario
        #region DataErrorInfo

        // Mensaje del error
        public string Error { get { return null; } }

        // Comprueba los errores que pueda tener una propiedad
        public string this[string columnName]
        {
            get
            {
                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                if (Validator.TryValidateProperty(
                        GetType().GetProperty(columnName).GetValue(this)
                        , new ValidationContext(this)
                        {
                            MemberName = columnName
                        }
                        , validationResults))
                    return null;

                return validationResults.First().ErrorMessage;
            }
        }

        #endregion

        public PropertyChangedDataError Clone()
        {
            return MemberwiseClone() as PropertyChangedDataError;
        }
    }
}
