namespace EducationalPortal.Application.Exceptions
{
    public class NoResultException : Exception
    {
        public NoResultException() : base() { }

        public NoResultException(string message)
            : base(message) { }

        public NoResultException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
