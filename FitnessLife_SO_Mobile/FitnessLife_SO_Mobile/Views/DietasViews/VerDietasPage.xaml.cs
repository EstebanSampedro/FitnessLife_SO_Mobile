using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
                    
                    var httpHandler = new HttpClientHandler();
                    var client = new HttpClient(httpHandler);
                    var json = JsonConvert.SerializeObject(id_dieta);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.DeleteAsync($"http://10.0.2.2:44396/api/dietas/" + id_dieta);
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        await DisplayAlert("Notificación", "Error al conectar", "OK");
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
            throw new NotImplementedException();
        }

        private void EditTextFalse()
        {
            throw new NotImplementedException();
        }
    }
}