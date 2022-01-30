using FitnessLife_SO_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FitnessLife_SO_Mobile.ViewModels
{
    public class DietaDetallesViewModel : BaseViewModel
    {
        private DietaDetalles _selectedDietaDetalles;

        public ObservableCollection<DietaDetalles> DietaDetalles { get; }
        public Command LoadDietaDetallesCommand { get; }
        public Command AddDietaDetallesCommand { get; }
        public Command<DietaDetalles> DietaDetallesTapped { get; }

        public DietaDetallesViewModel()
        {
            Title = "DietaDetalles";
            DietaDetalles = new ObservableCollection<DietaDetalles>();


            //DietasTapped = new Command<Item>(OnDietaSelected);

            //AddItemCommand = new Command(OnAddItem);
        }
    }
}
