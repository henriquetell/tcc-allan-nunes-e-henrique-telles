namespace ApplicationCore.Interfaces.Logging
{
    public enum ELoggingEvents : int
    {
        EfMigration = 1,
        EfSeed = 2,
        CancelarPedidos = 3,
        CapturarPagamentosCartoes = 4,
        CapturarPagamentosBoletos = 5,
        LimparLogAcesso = 6
    }
}
