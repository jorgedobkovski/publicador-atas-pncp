using PublicadorARP.Services.Interfaces;
using RestSharp;
using WebApp.Models.Dtos;

namespace PublicadorARP.Services
{
    public class PNCPService : IPNCPService
    {
        private readonly RestClient _client;
        private readonly IConfiguration _configuration;
        public PNCPService(IConfiguration configuration)
        {
            _client = new RestClient("https://treina.pncp.gov.br/api/pncp/v1/");
            _configuration = configuration;
        }

        public async Task<RestResponse> InserirAtaRegistroPreco(InserirAtaRegistroPrecoDto dto)
        {
            try
            {
                var request = new RestRequest(resource: $"orgaos/04801221000110/compras/{dto.anoCompra}/{dto.sequencialCompra}/atas", method: Method.Post);

                string login = _configuration["AuthPNCP:Login"];
                string senha = _configuration["AuthPNCP:Senha"];
                var token = await LoginAndGetTokenAsync(login, senha);
                request.AddHeader("Authorization", $"Bearer {token}");
                request.AddHeader("Content-Type", "application/json");

                var requestData = new
                {
                    numeroAtaRegistroPreco = dto.numeroAta,
                    anoAta = dto.anoAta,
                    dataAssinatura = dto.dataAssinatura,
                    dataVigenciaInicio = dto.dataInicioVigencia,
                    dataVigenciaFim = dto.dataFimVigencia
                };
                request.AddJsonBody(requestData);

                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var responseData = response.Content;
                    var location = response.Headers.FirstOrDefault(h => h.Name == "Location")?.Value.ToString();
                    var urlUpload = $"{location}/arquivos";

                    dto.arquivo.Headers.Add("Titulo-Documento", "teste doc");
                    dto.arquivo.Headers.Add("Tipo-Documento", "11");

                    var responseUpload = await UploadFileAsync(urlUpload, dto.arquivo, token);

                    if (responseUpload.IsSuccessful)
                    {
                        Console.WriteLine(responseUpload.StatusCode);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to upload file: {responseUpload.StatusCode}");
                        Console.WriteLine(responseUpload.Content);
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to submit form: {response.StatusCode}");
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error submitting form: {ex.Message}");
                return null;
            }
        }

        private async Task<string?> LoginAndGetTokenAsync(string login, string senha)
        {
            try
            {
                var request = new RestRequest(resource: "usuarios/login", method: Method.Post);
                request.AddHeader("Content-Type", "application/json");

                var requestData = new
                {
                    login = login,
                    senha = senha
                };
                request.AddJsonBody(requestData);

                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    // Extrair o token de acesso do cabeçalho Authorization da resposta
                    string token = response.Headers.FirstOrDefault(h => h.Name == "Authorization")?.Value.ToString()?.Replace("Bearer ", "");

                    return token;
                }
                else
                {
                    Console.WriteLine($"Failed to login: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging in: {ex.Message}");
                return null;
            }
        }

        private async Task<RestResponse?> UploadFileAsync(string uploadUrl, IFormFile file, string token)
        {

            var restClient = new RestClient(uploadUrl);

            var arquivoContent = ConvertIFormFileToByteArray(file);

            var request = new RestRequest(resource: "", method: Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Titulo-Documento", "teste doc");
            request.AddHeader("Tipo-Documento", "11");
            request.AddFile("arquivo", arquivoContent, file.FileName, "application/pdf");

            var response = await restClient.ExecuteAsync(request);

            return response;
        }

        private byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
