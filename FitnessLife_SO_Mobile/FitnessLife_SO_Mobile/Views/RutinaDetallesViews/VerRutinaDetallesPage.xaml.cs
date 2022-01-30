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
    public partial class VerRutinaDetallesPage : ContentPage
    {
        public int id_rutinaDetalle;
        public VerRutinaDetallesPage(int rutinaDetalleID)
        {
            InitializeComponent();
            id_rutinaDetalle = rutinaDetalleID;
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
                        var json = JsonConvert.SerializeObject(id_rutinaDetalle);
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.DeleteAsync($"http://10.0.2.2:44396/api/rutinadetalles/" + id_rutinaDetalle);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            await DisplayAlert("Notificación", "La rutina detallada se ha eliminado con exito", "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Notificación", "La rutina detallada se ha eliminado con exito", "OK");
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

        List<string> listaRutinas = new List<string>();
        List<int> listaRutinasID = new List<int>();
        public int buscarIDRutina()
        {
            int x = PickerIdRutina.SelectedIndex;
            return listaRutinasID[x];
        }

        private void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Notificacion", "Realmente desea modificar?", "Si", "No");
                if (result)
                {
                    try
                    {
                        if (String.IsNullOrWhiteSpace(txtDia.Text))
                        {
                            await DisplayAlert("Advertencia", "El campo DIA SEMANA es obligatorio", "OK");
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
                            rutinaDetalles.IdRutinaDetalle = id_rutinaDetalle;
                            rutinaDetalles.DiaSemana = txtDia.Text;
                            rutinaDetalles.Ejercicio = txtEjercicio.Text;
                            rutinaDetalles.Repeticiones = txtRepeticiones.Text;
                            
                            rutinaDetalles.IdRutina = buscarIDRutina();

                            var httpHandler = new HttpClientHandler();
                            var client = new HttpClient(httpHandler);
                            var json = JsonConvert.SerializeObject(rutinaDetalles);
                            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PutAsync($"http://10.0.2.2:44396/api/rutinadetalles/{rutinaDetalles.IdRutinaDetalle}", content);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                await DisplayAlert("Notificacion", "La rutina detallada se ha modificado con exito: " + txtEjercicio.Text, "OK");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("Notificacion", "La rutina se ha modificado con exito: " + txtEjercicio.Text, "OK");
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
            txtDia.IsEnabled = false;
            txtEjercicio.IsEnabled = false;
            txtRepeticiones.IsEnabled = false;
            


            btnGuardar.IsVisible = false;

        }

        private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            txtDia.IsEnabled = true;
            txtEjercicio.IsEnabled = true;
            txtRepeticiones.IsEnabled = true;
            

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