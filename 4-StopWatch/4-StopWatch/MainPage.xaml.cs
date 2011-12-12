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
using System.Windows.Threading;
using Microsoft.Phone.Shell;
using WindowsPhoneApp;
namespace _4_StopWatch
{
    public partial class MainPage : PhoneApplicationPage
    {
        Setting<TimeSpan> totalTime = new Setting<TimeSpan>("TotalTime",TimeSpan.Zero);
        Setting<TimeSpan> currentTime = new Setting<TimeSpan>("CurrentLapTime",TimeSpan.Zero);
        Setting<TimeSpan> previousLapTime = new Setting<TimeSpan>("PreviousLapTime",TimeSpan.Zero);
        Setting<List<TimeSpan>> lapList = new Setting<List<TimeSpan>>("LapList",new List<TimeSpan>());
        Setting<DateTime> previousTick = new Setting<DateTime>("PreviousTick",DateTime.MinValue);

        Setting<SupportedPageOrientation> savedSupportedOrientations =
            new Setting<SupportedPageOrientation>("SavedSupportOrientations",SupportedPageOrientation.PortraitOrLandscape);
        Setting<bool> wasRunning = new Setting<bool>("WasRunning",false);

        DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(.1)};
        IApplicationBarIconButton orientationLockBtn;

        public MainPage()
        {
            InitializeComponent();
            orientationLockBtn = ApplicationBar.Buttons[0] as IApplicationBarIconButton;
            this.timer.Tick += Timer_Tick;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ShowCurrentTime();
            foreach (TimeSpan lapTime in this.lapList.Value)
            {
                InsertLapList(lapTime);
            }
            if (this.totalTime.Value > TimeSpan.Zero)
            {
                this.ResetBtn.IsEnabled = true;
            }
            this.SupportedOrientations = this.savedSupportedOrientations.Value;
            if (this.SupportedOrientations != SupportedPageOrientation.PortraitOrLandscape)
            {
                this.orientationLockBtn.Text = "unlock";
                this.orientationLockBtn.IconUri = new Uri("/Shared/Images/appbar.orientationLocked.png");
            }
            if (this.wasRunning.Value) Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan delta = DateTime.UtcNow - this.previousTick.Value;
            this.previousTick.Value += delta;
            this.totalTime.Value += delta;
            this,currentLapTime.Value += delta;
            //TODO
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}