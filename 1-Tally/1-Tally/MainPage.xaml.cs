using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WindowsPhoneApp;

namespace _1_Tally
{
    public partial class MainPage : PhoneApplicationPage
    {
        int count = 0;
        Setting<int> savedCount = new Setting<int>("SaveCount",0);
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            count++;
            CountTextBlock.Text = count.ToString("N0");
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            count = 0;
            CountTextBlock.Text = count.ToString("N0");
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            savedCount.Value = count;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            count = savedCount.Value;
            CountTextBlock.Text = count.ToString("N0");
        }
    }
}