using System;

namespace Framework.Data
{
    public class PaginadorInfo
    {
        public int NumeroPagina { get; }
        public int TamanhoPagina { get; }
        public int TotalExibidos { get; private set; }
        public long TotalRegistros { get; private set; }
        public int TotalPaginas { get; private set; }
        public string CustomAction { get; set; }

        public PaginadorInfo(int? numeroPagina = null, int? tamanhoPagina = null)
        {
            NumeroPagina = numeroPagina.GetValueOrDefault(0);
            TamanhoPagina = tamanhoPagina.GetValueOrDefault(10);
            TotalExibidos = NumeroPagina * TamanhoPagina;
        }
        public void SetTotalRegistro(long totalRegistros)
        {
            TotalRegistros = totalRegistros;
            TotalPaginas = (int)Math.Ceiling((decimal)totalRegistros / TamanhoPagina);
        }
    }
}
