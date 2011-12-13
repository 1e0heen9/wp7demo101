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
using System.ComponentModel;
using System.Globalization;

namespace _4_StopWatch
{
    public partial class TimeSpanDisplay : UserControl
    {
        int digitWidth;
        TimeSpan time;
        public TimeSpanDisplay()
        {
            InitializeComponent();
            if (DesignerProperties.IsInDesignTool)
            {
                this.LayoutRoot.Children.Add(new TextBlock { Text="0:00.0"});
            }
        }

        public int DigitWidth
        {
            get { return this.digitWidth; }
            set {
                this.digitWidth = value;
                this.Time = this.time;
            }
        }

        public TimeSpan Time
        {
            get { return this.time; }
            set
            {
                this.LayoutRoot.Children.Clear();
                string minutesString = value.Minutes.ToString();
                for (int i = 0; i < minutesString.Length; i++)
                {
                    AddDigitString(minutesString[i].ToString());
                }
                this.LayoutRoot.Children.Add(new TextBlock { Text=":"});
                AddDigitString((value.Seconds/10).ToString());
                AddDigitString((value.Seconds % 10).ToString());
                this.LayoutRoot.Children.Add(new TextBlock { 
                    Text= CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator});

                AddDigitString((value.Milliseconds/100).ToString());
                this.time = value;
            }
        }

        void AddDigitString(string str)
        {
            Border border = new Border { Width=this.DigitWidth};
            border.Child = new TextBlock { Text = str,HorizontalAlignment = HorizontalAlignment.Center};
            this.LayoutRoot.Children.Add(border);
        }
    }
}
