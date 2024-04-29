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

namespace WebApp.Controllers
{
    public class ARPController : ControllerBase
    {
        private readonly IPNCPService _pncpService;

        public ARPController(IPNCPService pncpService)
        {
            _pncpService = pncpService;
        }

        [HttpPost]
        public async Task<IActionResult> InserirAtaRegistroPreco(AtaRegistroPrecoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return new ViewResult();
            }

            await _pncpService.InserirAtaRegistroPreco(dto);

            return Ok();    
        }       
                
    }
}