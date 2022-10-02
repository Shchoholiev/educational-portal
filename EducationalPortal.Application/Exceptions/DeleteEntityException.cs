namespace EducationalPortal.Application.Exceptions
{
    public class DeleteEntityException : Exception
    {
        public DeleteEntityException() : base() { }

        public DeleteEntityException(string message) 
            : base($"Entity can not be deleted, because: {message}") { }

        public DeleteEntityException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
