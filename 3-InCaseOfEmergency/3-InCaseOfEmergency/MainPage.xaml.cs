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
using Microsoft.Phone.Tasks;

namespace _3_InCaseOfEmergency
{
    public partial class MainPage : PhoneApplicationPage
    {
        Setting<string> savedContactName = new Setting<string>("ContactName","");
        Setting<string> savedPhoneNumber = new Setting<string>("PhoneNumber","");
        Setting<string> savedOwnerName = new Setting<string>("OwnerName","");
        Setting<string> savedMedicalNotes = new Setting<string>("MedicalNotes","");
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.savedContactName.Value = ContactNameTextBox.Text;
            this.savedPhoneNumber.Value = PhoneNumberTextBox.Text;
            this.savedOwnerName.Value = OwnNameTextBox.Text;
            this.savedMedicalNotes.Value = MedicalNotesTextBox.Text;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ContactNameTextBox.Text = this.savedContactName.Value;
            PhoneNumberTextBox.Text = this.savedPhoneNumber.Value;
            OwnNameTextBox.Text = this.savedOwnerName.Value;
            MedicalNotesTextBox.Text = this.savedMedicalNotes.Value;
        }
         

        void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
        void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == this.ContactNameTextBox)
                    this.PhoneNumberTextBox.Focus();
                else if (sender == this.PhoneNumberTextBox)
                    this.OwnNameTextBox.Focus();
                else if (sender == this.OwnNameTextBox)
                    this.MedicalNotesTextBox.Focus();

                e.Handled = true;
            }
        }
        void TagHereToCall_MouseLeftBtnUp(object sender, EventArgs e)
        {
            PhoneCallTask phoneLauncher = new PhoneCallTask();
            phoneLauncher.DisplayName = this.ContactNameTextBox.Text;
            phoneLauncher.PhoneNumber = this.PhoneNumberTextBox.Text;

            if (phoneLauncher.PhoneNumber.Length == 0)
            {
                MessageBox.Show("没有联系人电话可以拨打");
            }
            else
            {
                phoneLauncher.Show();
            }
        }
    }
}