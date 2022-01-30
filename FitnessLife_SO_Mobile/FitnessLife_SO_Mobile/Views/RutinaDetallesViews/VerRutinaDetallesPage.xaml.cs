using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessLife_SO_Mobile.Views.RutinaDetallesViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerRutinaDetallesPage : ContentPage
    {
        public int id_dietaDetalle;
        public VerRutinaDetallesPage()
        {
            InitializeComponent();
        }
    }
}