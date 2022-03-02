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

        public bool Succeeded { get; set; } = false;

        public List<string> Messages { get; set; } = new List<string>();

        public void AddMessage(string message)
        {
            this.Messages.Add(message);
        }

        public int MessagesCount()
        {
            return Messages.Count;
        }
    }
}
