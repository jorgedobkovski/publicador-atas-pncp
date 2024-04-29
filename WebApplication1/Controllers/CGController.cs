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

namespace WebApp.Controllers
{
    public class CGController : ControllerBase
    {
        
        private static readonly HttpClient client = new HttpClient();       
        
        [HttpPost]
        public async Task<IActionResult> VisualizarCertameAPI(ConsultarCertameDto dto)
        {
            if (!ModelState.IsValid)
            {
                return new ViewResult();
            }

            try
            {

                var token = await LoginAndGetTokenAsync("", "");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = $"https://treina.pncp.gov.br/api/pncp/v1/orgaos/04801221000110/compras/{dto.anoCompra}/{dto.sequencialCompra}/atas";
                var requestData = new
                {
                    numeroAtaRegistroPreco = dto.numeroAta,
                    anoAta = dto.anoAta,
                    dataAssinatura = dto.dataAssinatura,
                    dataVigenciaInicio = dto.dataInicioVigencia,
                    dataVigenciaFim = dto.dataFimVigencia
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var location = response.Headers.Location;
                        var urlUpload = $"{location}/arquivos";

                        dto.arquivo.Headers.Add("Titulo-Documento", "teste doc");
                        dto.arquivo.Headers.Add("Tipo-Documento", "11");


                        // Chame a função para fazer o upload do arquivo
                        var responseUpload = await UploadFileAsync(client, urlUpload, dto.arquivo, token);

                        if (responseUpload.IsSuccessStatusCode)
                        {
                            Console.WriteLine(responseUpload.StatusCode);
                        }
                        else
                        {
                            Console.WriteLine($"Failed to upload file: {responseUpload.StatusCode}");
                            Console.WriteLine(await responseUpload.Content.ReadAsStringAsync());
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to submit form: {response.StatusCode}");
                        // Handle error response
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error submitting form: {ex.Message}");
                    // Handle exception
                }

                return new ViewResult();
            }
            catch (Exception ex)
            {
                return new ViewResult();
            }
        }

        public async Task<HttpResponseMessage> UploadFileAsync(HttpClient client, string uploadUrl, IFormFile file, string token)
        {

            var restClient = new RestClient(uploadUrl);

            var arquivoContent = ConvertIFormFileToByteArray(file);
            var arquivo = new ByteArrayContent(arquivoContent);
            arquivo.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            // Crie uma solicitação HTTP POST multipart/form-data
            var request = new RestRequest(resource: "", method: Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Titulo-Documento", "teste doc");
            request.AddHeader("Tipo-Documento", "11");
            request.AddFile("arquivo", arquivoContent, file.FileName, "application/pdf");

            // Execute a solicitação
            var response = await restClient.ExecuteAsync(request);

            var reponseVar = response;

            return null;

            //// Crie o conteúdo do formulário multipart/form-data
            //var formDataContent = new MultipartFormDataContent();

            //// Adicione o arquivo ao conteúdo do formulário
            //var arquivoContent = ConvertIFormFileToByteArray(file);
            //var arquivo = new ByteArrayContent(arquivoContent);
            //arquivo.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");


            //var fileStreamContent = new StreamContent(file.OpenReadStream());
            //fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            //string base64String = Convert.ToBase64String(arquivoContent);
            //var stringContent = new StringContent(base64String);
            //stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            //formDataContent.Add(new StringContent(base64String), "arquivo", file.Name);

            //// Adicione os cabeçalhos necessários
            //formDataContent.Headers.Add("Titulo-Documento", "teste");
            //formDataContent.Headers.Add("Tipo-Documento", "11");


            ////// Adicione o cabeçalho de autorização
            ////client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //// Faça a solicitação de upload
            //var responseUpload = await client.PostAsync(uploadUrl, formDataContent);

            //responseUpload.EnsureSuccessStatusCode();
            //var responseContent = await responseUpload.Content.ReadAsStringAsync();
            //Console.WriteLine("response :" + responseContent);

            //return responseUpload;
        }

        public byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }        

        public async Task<string> LoginAndGetTokenAsync(string login, string senha)
        {
            // URL para o endpoint de login
            var loginUrl = "https://treina.pncp.gov.br/api/pncp/v1/usuarios/login";

            // Criar os dados do formulário JSON
            var requestData = new
            {
                login = login,
                senha = senha
            };

            // Serializar os dados do formulário para JSON
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

            // Criar o conteúdo da solicitação
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Fazer a solicitação POST para fazer login
                var response = await client.PostAsync(loginUrl, content);

                // Verificar se a solicitação foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    // Extrair o token de acesso do cabeçalho Authorization da resposta
                    string token = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");

                    return token;
                }
                else
                {
                    // Lidar com erros de solicitação
                    Console.WriteLine($"Failed to login: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Lidar com exceções
                Console.WriteLine($"Error logging in: {ex.Message}");
                return null;
            }
        }
                
    }
}