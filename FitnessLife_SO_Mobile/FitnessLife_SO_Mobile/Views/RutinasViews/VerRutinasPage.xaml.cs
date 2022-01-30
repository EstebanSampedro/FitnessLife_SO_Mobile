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

namespace FitnessLife_SO_Mobile.Views.RutinasViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerRutinasPage : ContentPage
    {
        public int id_rutina;
        public VerRutinasPage(int rutinaID)
        {
            InitializeComponent();
            id_rutina = rutinaID;
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
                        var json = JsonConvert.SerializeObject(id_rutina);
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.DeleteAsync($"http://10.0.2.2:44396/api/rutinas/" + id_rutina);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            await DisplayAlert("Notificación", "La rutina se ha eliminado con exito", "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Notificación", "La rutina se ha eliminado con exito", "OK");
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
                var result = await this.DisplayAlert("Notificacion", "¿Realmente desea modificar?", "Si", "No");
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
                            var rutinas = new Rutinas();
                            rutinas.IdRutina = id_rutina;
                            rutinas.Descripcion = txtDescripcion.Text;

                            var httpHandler = new HttpClientHandler();
                            var client = new HttpClient(httpHandler);
                            var json = JsonConvert.SerializeObject(rutinas);
                            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PutAsync($"http://10.0.2.2:44396/api/rutinas/{rutinas.IdRutina}", content);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                await DisplayAlert("Notificacion", "La rutina se ha modificado con exito: " + txtDescripcion.Text, "OK");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("Notificacion", "La rutina se ha modificado con exito: " + txtDescripcion.Text, "OK");
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
                    request.RequestUri = new Uri($"http://10.0.2.2:44396/api/rutinas" + id_rutina);
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("accept", "application/json");

                    var client = new HttpClient();

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<List<Rutinas>>(content);
                        if (resultado.Count > 0)
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