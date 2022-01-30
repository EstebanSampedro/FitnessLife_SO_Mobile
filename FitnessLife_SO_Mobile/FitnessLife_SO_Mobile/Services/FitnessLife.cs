using FitnessLife_SO_Mobile.Helpers;
using FitnessLife_SO_Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FitnessLife_SO_Mobile.Services
{
    public sealed class FitnessLife : ObservableObject
    {
        private static HttpClient client;

        private static readonly FitnessLife instance = new FitnessLife();
        static FitnessLife()
        {
            client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:44396/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private FitnessLife() { }

        public static FitnessLife Instance
        {
            get
            {
                return instance;
            }
        }

        //GET API: Dietas 
        public static string DietasUrl = $"http://10.0.2.2:44396/api/dietas";
        public async Task<List<Dietas>> GetDietas()
        {
        #if DEBUG
            var httpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true };
        #else

                var httpHandler = new HttpClientHandler();

        #endif
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(DietasUrl);
            request.Method = HttpMethod.Get;
            request.Headers.Add("accept", "application/json");

            var client = new HttpClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {

                throw new Exception("ERROR");

            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Dietas>>(content);
                return resultado;
                
            }

        }

        //POST(CREAR): Dietas
        public async Task CrearDietas(int dietasID, string descripcion)
        {
            // Validar información
            if (descripcion == null)
            {
                throw new Exception("Debe crear una descripcion");
            }

            Dietas nuevaDieta = new Dietas()
            {
                IdDieta = dietasID,
                Descripcion = descripcion
                
            };

            var json = JsonConvert.SerializeObject(nuevaDieta);
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            await httpClient.PostAsync(DietasUrl, content);
            
        }
    }

    ////PUT(EDITAR): Dietas
    //public async Task<Dietas> EditarDietas(int dietasID, string descripcion)
    //{
    //    if (descripcion == null)
    //    {
    //        throw new Exception("Descripción NULL");
    //    }

    //    Dietas modificarDieta = new Dietas()
    //    {
    //        IdDieta = dietasID,
    //        Descripcion = descripcion
            
    //    };

    //    var httpHandler = new HttpClientHandler();
    //    var client = new HttpClient(httpHandler);
    //    var json = JsonConvert.SerializeObject(modificarDieta);
    //    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
    //    HttpResponseMessage response = await client.PutAsync($"http://10.0.2.2:44396/api/dietas/{modificarDieta.IdDieta}", content);
        
    //}
}
