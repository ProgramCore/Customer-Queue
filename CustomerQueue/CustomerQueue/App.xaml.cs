using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CustomerQueue.Views;

[assembly: ExportFont("FABrands.otf")]
[assembly: ExportFont("FARegular.otf")]
[assembly: ExportFont("FASolid.otf")]
namespace CustomerQueue
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
