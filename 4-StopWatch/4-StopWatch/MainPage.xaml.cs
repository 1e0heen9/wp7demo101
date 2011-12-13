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
        Setting<TimeSpan> currentLapTime = new Setting<TimeSpan>("CurrentLapTime",TimeSpan.Zero);
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
                InsertLapInList(lapTime);
            }
            if (this.totalTime.Value > TimeSpan.Zero)
            {
                this.ResetBtn.IsEnabled = true;
            }
            this.SupportedOrientations = this.savedSupportedOrientations.Value;
            if (this.SupportedOrientations != SupportedPageOrientation.PortraitOrLandscape)
            {
                this.orientationLockBtn.Text = "解锁";
                this.orientationLockBtn.IconUri = new Uri("/Shared/Images/appbar.orientationLocked.png",UriKind.Relative);
            }
            if (this.wasRunning.Value) Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan delta = DateTime.UtcNow - this.previousTick.Value;
            this.previousTick.Value += delta;
            this.totalTime.Value += delta;
            this.currentLapTime.Value += delta;

            ShowCurrentTime();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            this.previousTick.Value = DateTime.UtcNow;
            Start();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        void LapBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertLapInList(this.currentLapTime.Value);
            this.lapList.Value.Add(this.currentLapTime.Value);
            this.previousLapTime.Value = this.currentLapTime.Value;
            this.currentLapTime.Value = TimeSpan.Zero;
        }

        void Start()
        {
            this.ResetBtn.IsEnabled = true;
            this.StartBtn.Visibility = Visibility.Collapsed;
            this.StopBtn.Visibility = Visibility.Visible;
            this.ResetBtn.Visibility = Visibility.Collapsed;
            this.LapBtn.Visibility = Visibility.Visible;

            this.timer.Start();
            this.wasRunning.Value = true;
        }

        void Stop()
        {
            this.StartBtn.Visibility = Visibility.Visible;
            this.StopBtn.Visibility = Visibility.Collapsed;
            this.ResetBtn.Visibility = Visibility.Visible;
            this.LapBtn.Visibility = Visibility.Collapsed;
            this.timer.Stop();
            this.wasRunning.Value = false;
        }

        void Reset()
        {
            this.totalTime.Value = TimeSpan.Zero;
            this.currentLapTime.Value = TimeSpan.Zero;
            this.previousLapTime.Value = TimeSpan.Zero;
            this.lapList.Value.Clear();

            this.ResetBtn.IsEnabled = false;
            this.LapsStackPanel.Children.Clear();
            ShowCurrentTime();
        }

        void InsertLapInList(TimeSpan timeSpan)
        {
            int lapNumber = LapsStackPanel.Children.Count + 1;
            Grid grid = new Grid();
            grid.Children.Add(new TextBlock { Text="lap "+ lapNumber,
                Margin =new Thickness(24,0,0,0)});
            TimeSpanDisplay display = new TimeSpanDisplay
            {
                Time = timeSpan,
                DigitWidth = 18,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 24, 0)
            };
                grid.Children.Add(display);

            LapsStackPanel.Children.Insert(0,grid);
        }

        void ShowCurrentTime()
        {
            this.TotalTimeDisplay.Time = this.totalTime.Value;
            this.CurrentLapTimeDisplay.Time = this.currentLapTime.Value;
            this.LapProgressBar.Maximum = this.previousLapTime.Value.TotalSeconds;
            this.LapProgressBar.Value = this.currentLapTime.Value.TotalSeconds;
        }

        void OrienttationLockBtn_Click(object sender, EventArgs e)
        {
            if (this.SupportedOrientations != SupportedPageOrientation.PortraitOrLandscape)
            {
                this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
                this.orientationLockBtn.Text = "lock screen";
                this.orientationLockBtn.IconUri = new Uri("/Shared/Images/appbar.orientationUnlocked.png", UriKind.Relative);
            }
            else
            {
                if (IsMatchingOrientation(PageOrientation.Portrait))
                    this.SupportedOrientations = SupportedPageOrientation.Portrait;
                else
                    this.SupportedOrientations = SupportedPageOrientation.Landscape;

                this.orientationLockBtn.Text = "unlock";
                this.orientationLockBtn.IconUri = new Uri("/Shared/Images/appbar.orientationLocked.png",UriKind.Relative);
            }
            this.savedSupportedOrientations.Value = this.SupportedOrientations;
        }

        bool IsMatchingOrientation(PageOrientation orientation)
        {
            return((this.Orientation & orientation) == orientation);
        }

     
      
    }
}