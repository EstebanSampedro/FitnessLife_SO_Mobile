using FitnessLife_SO_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FitnessLife_SO_Mobile.ViewModels
{
    public class DietasDetailViewModel : BaseViewModel
    {
        public string idDieta;


        public string descripcion;

        public virtual ICollection<DietaDetalles> DietaDetalles { get; set; }

        public string Id { get; set; }

        public string Descripcion
        {
            get => descripcion;
            set => SetProperty(ref descripcion, value);
        }

        public string IdDieta
        {
            get
            {
                return idDieta;
            }
            set
            {
                idDieta = value;
                LoadDietaId(value);
            }
        }

        public async void LoadDietaId(string idDieta)
        {
            try
            {
                var dieta = await DataStore.GetItemAsync(idDieta);
                Id = dieta.Id;
                Descripcion = dieta.Text;
                
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
