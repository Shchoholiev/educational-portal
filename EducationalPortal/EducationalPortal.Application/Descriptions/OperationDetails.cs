namespace EducationalPortal.Application.Descriptions
{
    public class OperationDetails
    {
        public OperationDetails()
        {
        }

        public OperationDetails(string message)
        {
            this.AddMessage(message);
        }

        public bool Succeeded { get; set; } = true;

        public List<string> Messages { get; set; } = new List<string>();

        public void AddMessage(string message)
        {
            this.Messages.Add(message);
        }

        public void AddError(string message)
        {
            this.Succeeded = false;
            this.Messages.Add(message);
        }

        public int MessagesCount()
        {
            return Messages.Count;
        }
    }
}
