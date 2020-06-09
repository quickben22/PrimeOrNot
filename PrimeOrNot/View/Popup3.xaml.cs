using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrimeOrNot.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Popup3 : ContentView
    {
        public Popup3(string title)
        {
          
            InitializeComponent();
            TitleLabel.Text = title;
        }


        public EventHandler CloseButtonEventHandler { get; set; }
        public EventHandler PlayButtonEventHandler { get; set; }
        public EventHandler ResultsButtonEventHandler { get; set; }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            // invoke the event handler if its being subscribed
            CloseButtonEventHandler?.Invoke(this, e);
        }

        private void Play_Clicked(object sender, EventArgs e)
        {
            PlayButtonEventHandler?.Invoke(this, e);
        }

        private void Results_Clicked(object sender, EventArgs e)
        {
            ResultsButtonEventHandler?.Invoke(this, e);
        }
    }
}