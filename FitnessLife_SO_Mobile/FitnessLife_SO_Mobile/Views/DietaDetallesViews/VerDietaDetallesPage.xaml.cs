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
    public partial class VerDietaDetallesPage : ContentPage
    {
        public int id_dietaDetalle;
        public VerDietaDetallesPage(int dietaDetalleID)
        {
            InitializeComponent();
            id_dietaDetalle = dietaDetalleID;
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
                        var json = JsonConvert.SerializeObject(id_dietaDetalle);
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.DeleteAsync($"http://10.0.2.2:44396/api/dietadetalles/" + id_dietaDetalle);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            await DisplayAlert("Notificación", "La dieta detallada se ha eliminado con exito", "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Notificación", "La dieta detallada se ha eliminado con exito", "OK");
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

        List<string> listaDietas = new List<string>();
        List<int> listaDietasID = new List<int>();
        public int buscarIDDieta()
        {
            int x = PickerIdDieta.SelectedIndex;
            return listaDietasID[x];  
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
                        if (String.IsNullOrWhiteSpace(txtDia.Text))
                        {
                            await DisplayAlert("Advertencia", "El campo DIA SEMANA es obligatorio", "OK");
                        }
                        else if(String.IsNullOrWhiteSpace(txtHora.Text))
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
                            dietaDetalles.IdDietaDetalle = id_dietaDetalle;
                            dietaDetalles.DiaSemana = txtDia.Text;
                            dietaDetalles.HoraComida = txtHora.Text;
                            dietaDetalles.Plato = txtPlato.Text;
                            dietaDetalles.Porcion = txtPorcion.Text;
                            dietaDetalles.IdDieta = buscarIDDieta();

                            var httpHandler = new HttpClientHandler();
                            var client = new HttpClient(httpHandler);
                            var json = JsonConvert.SerializeObject(dietaDetalles);
                            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PutAsync($"http://10.0.2.2:44396/api/dietadetalles/{dietaDetalles.IdDietaDetalle}", content);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                await DisplayAlert("Notificacion", "La dieta detallada se ha modificado con exito: " + txtPlato.Text, "OK");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("Notificacion", "La dieta detallada se ha modificado con exito: " + txtPlato.Text, "OK");
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
            txtHora.IsEnabled = false;
            txtPlato.IsEnabled = false;
            txtPorcion.IsEnabled = false;
            

            btnGuardar.IsVisible = false;

        }

        private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            txtDia.IsEnabled = true;
            txtHora.IsEnabled = true;
            txtPlato.IsEnabled = true;
            txtPorcion.IsEnabled = true;

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