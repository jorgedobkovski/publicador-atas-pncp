using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.Dtos;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using RestSharp;
using PublicadorARP.Services.Interfaces;
using PublicadorARP.Models.Dtos;
using PublicadorARP.Models.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class ARPController : Controller
    {
        private readonly IPNCPService _pncpService;

        public ARPController(IPNCPService pncpService)
        {
            _pncpService = pncpService;
        }

        public IActionResult InserirAtaRegistroPreco()
        {
            return View();
        }

        public IActionResult AnexarArquivo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InserirAtaRegistroPreco(InserirAtaRegistroPrecoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _pncpService.InserirAtaRegistroPreco(dto);

            return Ok();    
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarAtasPorContratacao(ConsultarContratacaoDto dto)
        {
            var contratacaoViewModel = await _pncpService.ConsultarContratacaoComAtasDeRegistroDePreco(dto);

            if (contratacaoViewModel != null)
            {
                return View("ResultadoConsultaAtasPorContratacao", contratacaoViewModel);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GerenciarAtaRegistroPreco(string sequencialAta, string sequencialCompra, string anoCompra)
        {
            var consultaAtaDto = new ConsultarAtaRegistroPrecoDto()
            {
                anoCompra = anoCompra,
                sequencialAta = sequencialAta,
                sequencialCompra = sequencialCompra
            };

            var ataRegistroPrecoViewModel = await _pncpService.ConsultarAtaRegistroPreco(consultaAtaDto);

            return View("GerenciarAtaRegistroPreco", ataRegistroPrecoViewModel);
        }

    }
}