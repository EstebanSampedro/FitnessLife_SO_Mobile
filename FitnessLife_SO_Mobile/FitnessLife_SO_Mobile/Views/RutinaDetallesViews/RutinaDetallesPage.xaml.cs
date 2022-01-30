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

namespace FitnessLife_SO_Mobile.Views.RutinaDetallesViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RutinaDetallesPage : ContentPage
    {
        public RutinaDetallesPage()
        {
            InitializeComponent();
            RutinaDetallesListView.ItemTapped += RutinaDetallesListView_ItemTapped;
        }

        private void RutinaDetallesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var obj = (RutinaDetalles)e.Item;
            var rutinaDetalleID = obj.IdRutinaDetalle;
            try
            {
                Navigation.PushAsync(new VerRutinaDetallesPage(rutinaDetalleID));
            }
            catch (Exception)
            {
                DisplayAlert("Notificacion", "Error al conectar", "OK");
                Navigation.PopToRootAsync();
            }
        }

        private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CrearRutinaDetallesPage());
        }
        private void MenuItem2_Clicked(object sender, EventArgs e)
        {
            base.OnAppearing();
        }

        public static string RutinaDetallesUrl = $"http://10.0.2.2:44396/api/rutinadetalles";

        protected override void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
#if DEBUG

                    var httpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true };
#else
                     httpHandler = new HttpClientHandler();

#endif
                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri(RutinaDetallesUrl);
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("accept", "application/json");

                    var client = new HttpClient();

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<List<RutinaDetalles>>(content);
                        RutinaDetallesListView.ItemsSource = resultado;
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Notificación", "Error de conexión", "OK");
                    await Navigation.PopAsync();
                }
            });
        }
    }
}