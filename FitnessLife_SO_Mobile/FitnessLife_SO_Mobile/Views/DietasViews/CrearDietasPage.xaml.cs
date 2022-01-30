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

namespace FitnessLife_SO_Mobile.Views.DietasViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearDietasPage : ContentPage
    {
        public CrearDietasPage()
        {
            InitializeComponent();
            btnGuardar.Clicked += BtnGuardar_Clicked;
        }



        private async void BtnGuardar_Clicked(object sender, EventArgs e)
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
                    dietas.IdDieta = 0;
                    dietas.Descripcion = txtDescripcion.Text;


                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("http://10.0.2.2:44396/api/dietas");
                    request.Method = HttpMethod.Post;
                    request.Headers.Add("Accept", "application/json");
                    var payload = JsonConvert.SerializeObject(dietas);
                    HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
                    request.Content = c;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        await DisplayAlert("Notificacion", "La dieta " +txtDescripcion.Text + " se ha creado exitosamente ", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Notificacion", "La dieta " + txtDescripcion.Text + " se ha creado exitosamente ", "OK");
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
    }
}