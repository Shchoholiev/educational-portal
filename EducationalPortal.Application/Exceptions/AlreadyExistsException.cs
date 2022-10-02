namespace EducationalPortal.Application.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base() { }

        public AlreadyExistsException(string entityName)
            : base($"\"{entityName}\" already exists.") { }

        public AlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }

        public AlreadyExistsException(string paramName, string paramValue)
            : base($"Entity with {paramName}: \"{paramValue}\" already exists.") { }
    }
}
