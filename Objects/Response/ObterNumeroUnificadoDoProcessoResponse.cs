namespace IntegradorIdea.Objects.Response
{
    public class ObterNumeroUnificadoDoProcessoResponse
    {
        public ProcessoUnificado Processo { get; set; }
    }

    public class ProcessoUnificado
    {
        public ulong OutroNumero { get; set; }
        public string NumeroUnificado { get; set; }
    }
}
