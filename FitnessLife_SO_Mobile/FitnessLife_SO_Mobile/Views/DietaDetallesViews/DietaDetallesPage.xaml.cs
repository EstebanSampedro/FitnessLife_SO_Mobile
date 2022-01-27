using FitnessLife_SO_Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessLife_SO_Mobile.Views.DietaDetallesViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DietaDetallesPage : ContentPage
    {
        public DietaDetallesPage()
        {
            InitializeComponent();
            mostrarDatos();
        }


        public static string DietaDetallesUrl = $"http://10.0.2.2:44396/api/dietadetalles";

        private async void mostrarDatos()
        {
        #if DEBUG

            var httpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true };

        #else

            var httpHandler = new HttpClientHandler();

        #endif
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(DietaDetallesUrl);
            request.Method = HttpMethod.Get;
            request.Headers.Add("accept", "application/json");

            var client = new HttpClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {

                string content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<DietaDetalles>>(content);
                DietaDetallesListView.ItemsSource = resultado;
            }



        }
    }
}