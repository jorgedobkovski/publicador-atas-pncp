using Microsoft.AspNetCore.Http;
using System;

namespace WebApp.Models.Dtos
{
    public class AtaRegistroPrecoDto
    {
        public string anoCompra { get; set; }
        public string sequencialCompra { get; set; }
        public string numeroAta { get; set; }
        public string anoAta { get; set; }
        public string dataAssinatura { get; set; }
        public DateTime dataInicioVigencia { get; set; }
        public DateTime dataFimVigencia { get; set; }
        public IFormFile arquivo { get; set; }
    }
}
