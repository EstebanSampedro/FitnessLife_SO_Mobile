using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace FitnessLife_SO_Mobile.Views
{
    public partial class AboutPage : ContentPage
    {

        ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
        public AboutPage()
        {
            InitializeComponent();
            imageSources.Add("pantalla3.png");
            imageSources.Add("pantalla4.png");
            imageSources.Add("pantalla5.png");


            imgSlider.Images = imageSources;
        }
    }
}