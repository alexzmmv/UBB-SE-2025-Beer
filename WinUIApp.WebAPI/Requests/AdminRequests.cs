namespace WinUIApp.WebAPI.Requests
{
    public class SendNotificationRequest
    {
        public int SenderUserId { get; set; }
        public string UserModificationRequestType { get; set; } = string.Empty;
        public string UserModificationRequestDetails { get; set; } = string.Empty;
    }
} 
