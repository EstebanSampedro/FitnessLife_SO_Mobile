using FitnessLife_SO_Mobile.Models;
using FitnessLife_SO_Mobile.ViewModels;
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

namespace FitnessLife_SO_Mobile.Views.DietasViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DietasPage : ContentPage
    {

        public DietasPage()
        {
            InitializeComponent();
            mostrarDatos();
            DietasListView.ItemTapped += DietasListView_ItemTapped;

        }

        private void DietasListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var obj = (Dietas)e.Item;
            var dietaID = obj.IdDieta;
            try
            {
                Navigation.PushAsync(new VerDietasPage(dietaID));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CrearDietasPage());
        }
        private void MenuItem2_Clicked(object sender, EventArgs e)
        {
            base.OnAppearing();
        }

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
                    request.RequestUri = new Uri(DietasUrl);
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("accept", "application/json");

                    var client = new HttpClient();

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<List<Dietas>>(content);
                        DietasListView.ItemsSource = resultado;
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Notificación", "Error de conexión", "OK");
                    await Navigation.PopToRootAsync();
                }
            });
        }
            
        


        public static string DietasUrl = $"http://10.0.2.2:44396/api/dietas";
        

        private async void mostrarDatos()
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

            if (response.StatusCode == HttpStatusCode.OK)
            {

                string content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Dietas>>(content);
                DietasListView.ItemsSource = resultado;
            }

            

        }

        async void OnDeleteDietas(object sender, EventArgs e)
        {
            Dietas dieta = (Dietas)DietasListView.SelectedItem;
            if (dieta == null || IsBusy)
                return;

            if (await this.DisplayAlert("Delete Dieta?",
                "Are you sure you want to delete the dieta '"
                    + dieta.Descripcion + "'?", "Yes", "Cancel") == true)
            {
                try
                {
                    dieta.DietaDetalles = null;
                    var httpHandler = new HttpClientHandler();
                    var client = new HttpClient(httpHandler);
                    var json = JsonConvert.SerializeObject(dieta);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.DeleteAsync($"http://10.0.2.2:44396/api/dietas/{dieta.IdDieta}");
                    
                }
                catch (Exception ex)
                {
                    await this.DisplayAlert("Error",
                            ex.Message,
                            "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}