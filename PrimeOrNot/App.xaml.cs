using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrimeOrNot
{
    public partial class App : Application
    {
        public static double ScreenHeight;
        public static double ScreenWidth;
        public App()
        {
            InitializeComponent();
          
            MainPage = new NavigationPage(new MainPage());
        
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
