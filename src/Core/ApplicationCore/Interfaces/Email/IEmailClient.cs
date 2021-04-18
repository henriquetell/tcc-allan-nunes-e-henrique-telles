using System.IO;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Email
{
    public interface IEmailClient
    {
        Task EnviarParaFilaAsync(DadosEnvioEmail dadosEmail);
        Task EnviarAsync(DadosEnvioEmail dadosEmail);
        Task EnviarParaFilaConfirmacaoAsync(int idPedido);
        Task EnviarParaFilaPagamentoAsync(int idPedido);
        Task EnviarComAnexoAsync(DadosEnvioEmail dadosEmail, (Stream content, string name) anexo);
        Task EnviarParaFilaCancelamentoAsync(int idPedido);
        Task EnviarParaFilaEstornoAsync(int idPedido);
    }
}
