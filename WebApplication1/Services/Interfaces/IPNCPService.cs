using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApp.Models.Dtos;

namespace PublicadorARP.Services.Interfaces
{
    public interface IPNCPService
    {
        Task<RestResponse> InserirAtaRegistroPreco(InserirAtaRegistroPrecoDto dto);
    }
}
