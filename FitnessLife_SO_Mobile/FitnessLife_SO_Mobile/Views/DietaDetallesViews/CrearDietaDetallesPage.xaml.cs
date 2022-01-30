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
    public partial class CrearDietaDetallesPage : ContentPage
    {
        public CrearDietaDetallesPage()
        {
            InitializeComponent();
            btnGuardar.Clicked += BtnGuardar_Clicked;
        }


        List<string> listaDietas = new List<string>();
        List<int> listaDietasID = new List<int>();
        public int buscarIDDieta()
        {
            int x = PickerIdDieta.SelectedIndex;
            return listaDietasID[x];
        }
        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtDia.Text))
                {
                    await DisplayAlert("Advertencia", "El campo DIA es obligatorio", "OK");
                }
                else if (String.IsNullOrWhiteSpace(txtHora.Text))
                {
                    await DisplayAlert("Advertencia", "El campo HORA COMIDA es obligatorio", "OK");
                }
                else if (String.IsNullOrWhiteSpace(txtPlato.Text))
                {
                    await DisplayAlert("Advertencia", "El campo PLATO es obligatorio", "OK");
                }
                else if (String.IsNullOrWhiteSpace(txtPorcion.Text))
                {
                    await DisplayAlert("Advertencia", "El campo PORCION es obligatorio", "OK");
                }
                else
                {
                    var dietaDetalles = new DietaDetalles();
                    dietaDetalles.IdDietaDetalle = 0;
                    dietaDetalles.DiaSemana = txtDia.Text;
                    dietaDetalles.HoraComida = txtHora.Text;
                    dietaDetalles.Plato = txtPlato.Text;
                    dietaDetalles.Porcion = txtPorcion.Text;
                    dietaDetalles.IdDieta = buscarIDDieta();


                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("http://10.0.2.2:44396/api/dietadetalles");
                    request.Method = HttpMethod.Post;
                    request.Headers.Add("Accept", "application/json");
                    var payload = JsonConvert.SerializeObject(dietaDetalles);
                    HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
                    request.Content = c;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        await DisplayAlert("Notificacion", "La dieta detallada se ha creado exitosamente ", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Notificacion", "La dieta detallada se ha creado exitosamente ", "OK");
                        await Navigation.PopAsync();
                    }

                }
            }
            catch (Exception)
            {
                await DisplayAlert("Notificacion", "Error al conectar", "OK");
                await Navigation.PopToRootAsync();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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
                    request.RequestUri = new Uri($"http://10.0.2.2:44396/api/dietas");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("accept", "application/json");

                    var client = new HttpClient();

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<List<Dietas>>(content);
                        foreach (Dietas nuevaDieta in resultado)
                        {
                            //listaDietas.Add(nuevaDieta.Descripcion);
                            listaDietasID.Add(nuevaDieta.IdDieta);
                            PickerIdDieta.Items.Add(nuevaDieta.Descripcion);

                        }
                        //PickerIdDieta.ItemsSource = listaDietas;
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