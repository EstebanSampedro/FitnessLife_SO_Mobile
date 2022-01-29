using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FitnessLife_SO_Mobile.Models;
using FitnessLife_SO_Mobile.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessLife_SO_Mobile.Views.DietasViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerDietasPage : ContentPage
    {

        public int id_dieta;
        public VerDietasPage(int dietaID)
        {
            InitializeComponent();
            id_dieta = dietaID;
            EditTextFalse();
            btnGuardar.Clicked += BtnGuardar_Clicked;
            btnEliminar.Clicked += BtnEliminar_Clicked;

        }

        private void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var resul = await this.DisplayAlert("Alerta", "¿Está seguro de eliminar?", "Si", "No");
                if (resul)
                {
                    try
                    {
                        var httpHandler = new HttpClientHandler();
                        var client = new HttpClient(httpHandler);
                        var json = JsonConvert.SerializeObject(id_dieta);
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.DeleteAsync($"http://10.0.2.2:44396/api/dietas/" + id_dieta);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            await DisplayAlert("Notificación", "El usuario se ha eliminado con exito: " + txtDescripcion.Text, "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Notificación", "El usuario se ha eliminado con exito: " + txtDescripcion.Text, "OK");
                            await Navigation.PopAsync();
                        }

                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Notificacion", "Error al conectar", "OK");
                        await Navigation.PopToRootAsync();
                    }
                }
                else
                {
                    EditTextFalse();
                }
            });
        }

        private void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Notificacion", "Realmente necesita modificar?", "Si", "No");
                if (result)
                {
                    try
                    {
                        if (String.IsNullOrWhiteSpace(txtDescripcion.Text))
                        {
                            await DisplayAlert("Advertencia", "El campo Descripcion es obligatorio", "OK");
                        }
                        else
                        {
                            var dietas = new Dietas();
                            dietas.IdDieta = id_dieta;
                            dietas.Descripcion = txtDescripcion.Text;

                            var httpHandler = new HttpClientHandler();
                            var client = new HttpClient(httpHandler);
                            var json = JsonConvert.SerializeObject(dietas);
                            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PutAsync($"http://10.0.2.2:44396/api/dietas/{dietas.IdDieta}", content);
                            if(response.StatusCode == HttpStatusCode.OK)
                            {
                                await DisplayAlert("Notificacion", "El usuario se ha modificado con exito: " +txtDescripcion.Text, "OK");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("Notificacion", "El usuario se ha modificado con exito: " + txtDescripcion.Text, "OK");
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
            });
        }

        private void EditTextFalse()
        {
            txtDescripcion.IsEnabled = false;
            btnGuardar.IsVisible = false;

        }

        private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            txtDescripcion.IsEnabled = true;
            btnGuardar.IsVisible = true;
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
                    request.RequestUri = new Uri($"http://10.0.2.2:44396/api/dietas" + id_dieta);
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("accept", "application/json");

                    var client = new HttpClient();

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<List<Dietas>>(content);
                        if(resultado.Count > 0)
                        {
                            txtDescripcion.Text = resultado[0].Descripcion;
                        }
                        else
                        {
                            await DisplayAlert("Notificación", "Error de conexión", "OK");
                            
                        }
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