using AppProjekt.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AppProjekt.Views
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