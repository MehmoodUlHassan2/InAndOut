using InAndOut.Utility;

namespace InAndOut.Services

{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    };
}
