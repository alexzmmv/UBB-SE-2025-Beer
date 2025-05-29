namespace DataAccess.Service.Interfaces
{
    public interface ITemplateProvider
    {
        public string GetEmailTemplate();

        public string GetPlainTextTemplate();

        public string GetReviewRowTemplate();
    }
}
