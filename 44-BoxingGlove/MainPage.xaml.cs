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
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using WindowsPhoneApp;
using Microsoft.Phone.Shell;

namespace _44_BoxingGlove
{
    public partial class MainPage : PhoneApplicationPage
    {
        Accelerometer acc;
        public static Setting<bool> IsRightHanded = new Setting<bool>("IsRightHanded",true);
        public static Setting<double> Threshold = new Setting<double>("Threshold",1.5);
        //
        IApplicationBarIconButton switchHandBtn;
        DateTimeOffset accQuicklyForwardTime = DateTimeOffset.MinValue;
        Random random = new Random();

        double currentThreshold;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //acc.CurrentValueChanged +=new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(acc_CurrentValueChanged);
            //if (!Accelerometer.IsSupported) { AccTxt.Text = "不支持~"; }
            this.switchHandBtn = this.ApplicationBar.Buttons[1] as IApplicationBarIconButton;

            this.acc = new Accelerometer();
            this.acc.CurrentValueChanged +=new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(acc_CurrentValueChanged);
            SoundEffects.Initialize();
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                this.acc.Start();
            }
            catch (InvalidOperationException ex) {
                MessageBox.Show("unable to start acc:"+ex.ToString(),"error",MessageBoxButton.OK);
            }
            UpdateForCurrentHandedness();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            try
            {
                this.acc.Start();
            }
            catch (InvalidOperationException ex)
            {
 
            }
        }
        
        void acc_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            //Dispatcher.BeginInvoke(() => UpdateUI(e.SensorReading));
           AccelerometerReading t =e.SensorReading;
            Vector3 a=t.Acceleration;
            if (Math.Abs(a.X) < Math.Abs(this.currentThreshold)) return;

            if (a.X * this.currentThreshold > 0)
            {
                this.accQuicklyForwardTime = t.Timestamp;
            }
            else if(t.Timestamp - this.accQuicklyForwardTime < TimeSpan.FromSeconds(.2))
            {
                this.accQuicklyForwardTime = DateTimeOffset.MinValue;
                this.Dispatcher.BeginInvoke(delegate()
                {
                    switch (this.random.Next(0, 4))
                    {
                        case 0: SoundEffects.Punch1.Play(); break;
                        case 1: SoundEffects.Punch2.Play(); break;
                        case 2: SoundEffects.Punch3.Play(); break;
                        case 3: SoundEffects.Punch4.Play(); break;
                    }

                    switch (this.random.Next(0, 10))
                    {
                        case 0: SoundEffects.Grunt1.Play(); break;
                        case 1: SoundEffects.Grunt2.Play(); break;
                        case 2: SoundEffects.Grunt3.Play(); break;
                    }
                });
            }
        }

        private void UpdateUI(AccelerometerReading accReading)
        {
            Vector3 acceleration = accReading.Acceleration;

           // AccTxt.Text = "[x:" + acceleration.X.ToString() + ",y:" + acceleration.Y.ToString() + ",z:" + acceleration.Z.ToString() + "]";
        }

        void UpdateForCurrentHandedness()
        {
            this.currentThreshold = IsRightHanded.Value ? Threshold.Value : -Threshold.Value;
            this.ImageTransform.ScaleX = (IsRightHanded.Value?1:-1);

            if (IsRightHanded.Value)
            {
                this.switchHandBtn.IconUri = new Uri("/Images/appbar.leftHand.png", UriKind.Relative);
            }
            else
            {
                this.switchHandBtn.IconUri = new Uri("/Images/appbar.rightHand.png",UriKind.Relative);
            }
        }

        private void AbBellBtn_Click(object sender, EventArgs e)
        {
            SoundEffects.DingDingDing.Play();
        }

        private void AbSwtichHandBtn_Click(object sender, EventArgs e)
        {
            IsRightHanded.Value = !IsRightHanded.Value;
            UpdateForCurrentHandedness();
        }

        private void AbmSettingBtn_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Settings.xaml",UriKind.Relative));
        }

        private void AbmAboutMeBtn_Click(object sender, EventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/About"));
        }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {

        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SoundEffects.DingDingDing.Play();
        }
    }
}