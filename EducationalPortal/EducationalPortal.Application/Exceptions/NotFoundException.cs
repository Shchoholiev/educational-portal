namespace EducationalPortal.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }

        public NotFoundException(string entityName) 
            : base($"\"{entityName}\" was not found.") { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
