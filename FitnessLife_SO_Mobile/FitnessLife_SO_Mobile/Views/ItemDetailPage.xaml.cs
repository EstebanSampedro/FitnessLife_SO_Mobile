using FitnessLife_SO_Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FitnessLife_SO_Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}