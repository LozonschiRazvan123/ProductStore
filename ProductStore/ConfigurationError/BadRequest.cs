namespace ProductStore.ConfigurationError
{
    public class BadRequest: Exception
    {
        public BadRequest()
            : base("Something is wrong!")
        { }
    }
}
