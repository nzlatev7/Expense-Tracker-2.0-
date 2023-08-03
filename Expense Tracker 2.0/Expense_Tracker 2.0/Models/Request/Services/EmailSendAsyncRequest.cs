namespace Expense_Tracker_2._0.Models.Request
{
    public class EmailSendAsyncRequest
    {
        public string RecipientEmail { get; set; }
        public string TokenValue { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
