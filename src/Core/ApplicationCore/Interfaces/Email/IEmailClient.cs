using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Email
{
    public interface IEmailClient
    {
        Task EnviarAsync(DadosEnvioEmail dadosEmail);
    }
}
