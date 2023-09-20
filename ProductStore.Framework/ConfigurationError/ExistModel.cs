using System.Globalization;

namespace ProductStore.ConfigurationError
{
    public class ExistModel: Exception
    {
        public ExistModel(string model) : base($"The {model} is already exists ") { }

    }
}
