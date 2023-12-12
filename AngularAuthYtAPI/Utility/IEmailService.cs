using AngularAuthYtAPI.Models;

namespace AngularAuthYtAPI.Utility
{
    public interface IEmailService
    {
        void SendEmail(EmailModel model);
    }
}
