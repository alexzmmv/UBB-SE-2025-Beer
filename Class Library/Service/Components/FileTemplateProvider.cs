using DataAccess.Service.Interfaces;

namespace DataAccess.Service.Components
{
    public class FileTemplateProvider : ITemplateProvider
    {
        private static readonly string EmailPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "EmailContentTemplate.html");
        private static readonly string PlainTextPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "PlainTextContentTemplate.txt");
        private static readonly string ReviewPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "RecentReviewForReportTemplate.html");

        public string GetEmailTemplate()
        {
            return File.ReadAllText(EmailPath);
        }

        public string GetPlainTextTemplate()
        {
            return File.ReadAllText(PlainTextPath);
        }

        public string GetReviewRowTemplate()
        {
            return File.ReadAllText(ReviewPath);
        }
    }
}
