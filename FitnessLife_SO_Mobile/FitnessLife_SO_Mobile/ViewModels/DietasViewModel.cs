using FitnessLife_SO_Mobile.Models;
using FitnessLife_SO_Mobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using FitnessLife_SO_Mobile.Views.DietasViews;

namespace FitnessLife_SO_Mobile.ViewModels
{
    public class DietasViewModel : BaseViewModel
    {
        private Dietas _selectedDieta;

        public ObservableCollection<Dietas> Dietas { get; }
        public Command LoadDietasCommand { get; }
        public Command AddDietasCommand { get; }
        public Command<Dietas> DetailDietasCommand { get; }

        public DietasViewModel()
        {
            Title = "Dietas";
            Dietas = new ObservableCollection<Dietas>();
            

            DetailDietasCommand = new Command<Dietas>(OnDietasSelected);

            //AddItemCommand = new Command(OnAddItem);
        }

        public void OnAppearing()
        {
            IsBusy = true;
            Dietas SelectedDieta;
        }
        public Dietas SelectedDieta
        {
            get => _selectedDieta;
            set
            {
                SetProperty(ref _selectedDieta, value);
                OnDietasSelected(value);
            }
        }



        async void OnDietasSelected(Dietas dietaID)
        {
            await Shell.Current.GoToAsync($"{nameof(VerDietasPage)}?id={dietaID.IdDieta}", true);
            //await Shell.Current.GoToAsync($"{nameof(VerDietasPage)}?{nameof(DietasDetailViewModel.IdDieta)}={dietaID.IdDieta}");
        }
    }


}
