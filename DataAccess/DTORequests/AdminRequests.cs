namespace WinUIApp.WebAPI.Requests
{
    public class SendNotificationRequest
    {
        public Guid SenderUserId { get; set; }
        public string UserModificationRequestType { get; set; } = string.Empty;
        public string UserModificationRequestDetails { get; set; } = string.Empty;
    }
}
