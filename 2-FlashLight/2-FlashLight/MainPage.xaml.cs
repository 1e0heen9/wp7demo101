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
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Reflection;
using WindowsPhoneApp;


namespace _2_FlashLight
{
    public partial class MainPage : PhoneApplicationPage
    {
        IApplicationBarIconButton sosBtn=null;
        IApplicationBarIconButton strobeBtn = null;
        SolidColorBrush onBrush;
        SolidColorBrush offBrush=new SolidColorBrush(Colors.Black);
        DispatcherTimer strobeTimer = new DispatcherTimer();
        DispatcherTimer sosTimer = new DispatcherTimer();
        int sosStep;

        Setting<Color> savedColor = new Setting<Color>("SavedColor",Colors.White);
        FlashlightMode mode = FlashlightMode.Solid;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.sosBtn = this.ApplicationBar.Buttons[0] as IApplicationBarIconButton;
            this.strobeBtn = this.ApplicationBar.Buttons[1] as IApplicationBarIconButton;

            this.strobeTimer.Interval = TimeSpan.FromSeconds(.1);
            this.strobeTimer.Tick += StrobeTimer_Tick;

            this.sosTimer.Interval = TimeSpan.Zero;
            this.sosTimer.Tick += SosTimer_Tick;

            foreach(IApplicationBarMenuItem menuItem in this.ApplicationBar.MenuItems){
                menuItem.Click += MenuItem_Click;
            }

            this.onBrush = new SolidColorBrush(this.savedColor.Value);
            this.BackgroundGrid.Background = onBrush;
        }

        void MenuItem_Click(object sender, EventArgs e)
        {
            string chosenColor = (sender as IApplicationBarMenuItem).Text;
            Color c = (Color)typeof(Colors).GetProperty(chosenColor,
                BindingFlags.Public|BindingFlags.Static|BindingFlags.IgnoreCase).GetValue(null,null);
            this.savedColor.Value = c;
            this.onBrush = new SolidColorBrush(this.savedColor.Value);
            this.BackgroundGrid.Background = onBrush;
        }

        void StrobeTimer_Tick(object sender, EventArgs e)
        {
            if (this.BackgroundGrid.Background == this.onBrush)
                this.BackgroundGrid.Background = this.offBrush;
            else
                this.BackgroundGrid.Background = this.onBrush;
        }

        void SosTimer_Tick(object sender, EventArgs e)
        {
            switch (this.sosStep)
            {
                case 1: case 3: case 5:
                case 13: case 15: case 17:
                    this.BackgroundGrid.Background = this.onBrush;
                    this.sosTimer.Interval = TimeSpan.FromSeconds(.2);
                    break;
                case 7: case 9: case 11:
                    this.BackgroundGrid.Background = this.onBrush;
                    this.sosTimer.Interval = TimeSpan.FromSeconds(1);
                    break;
                case 18:
                    this.BackgroundGrid.Background = this.offBrush;
                    this.sosTimer.Interval = TimeSpan.FromSeconds(1);
                    break;
                default:
                    this.BackgroundGrid.Background = this.offBrush;
                    this.sosTimer.Interval = TimeSpan.FromSeconds(.2);
                    break;
            }
            this.sosStep = (this.sosStep + 1) % 19;
        }

        private void sosBtn_Click(object sender, EventArgs e)
        {
            FlashlightMode mode = this.mode;
            RestoreSolidMode();

            if (mode == FlashlightMode.Sos)
            {
                return;
            }
            (sender as IApplicationBarIconButton).IconUri =
                new Uri("Images/cancel.png", UriKind.Relative);
            this.mode = FlashlightMode.Sos;
            this.sosStep = 0;
            this.sosTimer.Start();
        }

        private void strobeBtn_Click(object sender, EventArgs e)
        {
            FlashlightMode mode = this.mode;
            RestoreSolidMode();

            if (mode == FlashlightMode.Strobe) return;
            MessageBoxResult result = MessageBox.Show("你确定要点开闪屏？","警告",MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                (sender as IApplicationBarIconButton).IconUri =
                    new Uri("Images/cancel.png",UriKind.Relative);
                this.mode = FlashlightMode.Strobe;
                this.strobeTimer.Start();
            }
        }

        void RestoreSolidMode()
        {
            this.strobeTimer.Stop();
            this.sosTimer.Stop();
            this.BackgroundGrid.Background = onBrush;
            this.sosBtn.IconUri = new Uri("Images/sos.png",UriKind.Relative);
            this.strobeBtn.IconUri = new Uri("Images/strobe.png",UriKind.Relative);
            this.mode = FlashlightMode.Solid;
        }

        enum FlashlightMode
        {
            Solid,Sos,Strobe
        }
    }
}