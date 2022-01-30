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
    public partial class CrearRutinaDetallesPage : ContentPage
    {
        public CrearRutinaDetallesPage()
        {
            InitializeComponent();
            btnGuardar.Clicked += BtnGuardar_Clicked;
        }

        List<string> listaRutinas = new List<string>();
        List<int> listaRutinasID = new List<int>();
        public int buscarIDRutina()
        {
            int x = PickerIdRutina.SelectedIndex;
            return listaRutinasID[x];
        }
        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtDia.Text))
                {
                    await DisplayAlert("Advertencia", "El campo DIA es obligatorio", "OK");
                }
                else if (String.IsNullOrWhiteSpace(txtEjercicio.Text))
                {
                    await DisplayAlert("Advertencia", "El campo EJERCICIO es obligatorio", "OK");
                }
                else if (String.IsNullOrWhiteSpace(txtRepeticiones.Text))
                {
                    await DisplayAlert("Advertencia", "El campo REPETICIONES es obligatorio", "OK");
                }
                
                else
                {
                    var rutinaDetalles = new RutinaDetalles();
                    rutinaDetalles.IdRutinaDetalle = 0;
                    rutinaDetalles.DiaSemana = txtDia.Text;
                    rutinaDetalles.Ejercicio = txtEjercicio.Text;
                    rutinaDetalles.Repeticiones = txtRepeticiones.Text;
                    
                    rutinaDetalles.IdRutina = buscarIDRutina();


                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("http://10.0.2.2:44396/api/rutinadetalles");
                    request.Method = HttpMethod.Post;
                    request.Headers.Add("Accept", "application/json");
                    var payload = JsonConvert.SerializeObject(rutinaDetalles);
                    HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
                    request.Content = c;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        await DisplayAlert("Notificacion", "La rutina detallada se ha creado exitosamente ", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Notificacion", "La rutina detallada se ha creado exitosamente ", "OK");
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
                    request.RequestUri = new Uri($"http://10.0.2.2:44396/api/rutinas");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("accept", "application/json");

                    var client = new HttpClient();

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<List<Rutinas>>(content);
                        foreach (Rutinas nuevaRutina in resultado)
                        {
                            //listaDietas.Add(nuevaDieta.Descripcion);
                            listaRutinasID.Add(nuevaRutina.IdRutina);
                            PickerIdRutina.Items.Add(nuevaRutina.Descripcion);

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