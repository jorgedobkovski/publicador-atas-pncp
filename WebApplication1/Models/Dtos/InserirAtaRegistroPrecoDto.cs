using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Dtos
{
    public class InserirAtaRegistroPrecoDto
    {
        [Display(Name = "Ano da contratação")]
        public string anoCompra { get; set; }
        [Display(Name = "Sequencial da contratação")]
        public string sequencialCompra { get; set; }
        [Display(Name = "Numero da ata")]
        public string numeroAta { get; set; }
        [Display(Name = "Ano da ata")]
        public string anoAta { get; set; }
        [Display(Name = "Data da assinatura")]
        public string dataAssinatura { get; set; }
        [Display(Name = "Data inicial da vigência")]
        public DateTime dataInicioVigencia { get; set; }
        [Display(Name = "Data final da vigência")]
        public DateTime dataFimVigencia { get; set; }
        [Display(Name = "Anexo da ata de registro de preço")]
        public IFormFile arquivo { get; set; }
    }
}
