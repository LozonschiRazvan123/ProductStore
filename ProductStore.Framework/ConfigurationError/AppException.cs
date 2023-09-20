using System.Globalization;

namespace ProductStore.ConfigurationError
{
    public class AppException: Exception
    {

        public AppException(string entityName, string id) 
            : base($"The {entityName} with ID {id} was not found.") 
        { }

    }
}
