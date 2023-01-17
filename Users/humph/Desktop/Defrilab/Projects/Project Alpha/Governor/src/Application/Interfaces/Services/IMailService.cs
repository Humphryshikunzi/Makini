using _.Application.Requests.Mail;
using System.Threading.Tasks;

namespace _.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}