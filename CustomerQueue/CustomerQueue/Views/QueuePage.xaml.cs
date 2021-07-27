using CustomerQueue.Models;
using CustomerQueue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace CustomerQueue.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueuePage : ContentPage
    {
        QueueVM vm;
        public QueuePage()
        {
            InitializeComponent();
            this.BindingContext = vm = new QueueVM();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}