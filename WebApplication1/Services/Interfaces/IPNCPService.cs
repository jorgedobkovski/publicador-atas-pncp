﻿using Microsoft.AspNetCore.Mvc;
using PublicadorARP.Models.Dtos;
using PublicadorARP.Models.ViewModels;
using RestSharp;
using WebApp.Models.Dtos;

namespace PublicadorARP.Services.Interfaces
{
    public interface IPNCPService
    {
        Task<RestResponse> InserirAtaRegistroPreco(InserirAtaRegistroPrecoDto dto);
        Task<IList<AtaRegistroPrecoViewModel>?> ConsultarAtasPorContratacao(ConsultarContratacaoDto dto);
        Task<ContratacaoViewModel?> ConsultarContratacao(ConsultarContratacaoDto dto);
        Task<ContratacaoViewModel?> ConsultarContratacaoComAtasDeRegistroDePreco(ConsultarContratacaoDto dto);
        Task<AtaRegistroPrecoViewModel?> ConsultarAtaRegistroPreco(ConsultarAtaRegistroPrecoDto dto);
    }
}
