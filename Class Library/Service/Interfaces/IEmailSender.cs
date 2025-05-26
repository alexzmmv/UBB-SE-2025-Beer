namespace DataAccess.Service.Interfaces
{
    using System.Threading.Tasks;
    using MimeKit;

    public interface IEmailSender
    {
        Task SendEmailAsync(MimeMessage message, string smtpEmail, string smtpPassword);
    }
}
