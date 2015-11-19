using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LunaAppWp8.ViewModels;
using System.Windows.Media;
using LunaAppWp8;

namespace LunaAppWp8
{
    public partial class InitialSetupPage : PhoneApplicationPage
    {

        public static SetupViewModel SetupViewModel
        {
            get
            {             
                return App.SetupViewModel;
            }
        }
        public InitialSetupPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #region Navigation

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!SetupViewModel.ShowInitialSetup)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }

            base.OnNavigatedTo(e);
        }

        #endregion

        #region Events
    
        private void setupBtn_Click(object sender, RoutedEventArgs e)
        {
            App.SetupViewModel.ShowInitialSetup = false;

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void tbPeriodCycle_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";

            if ((sender as TextBox).Name.Contains("Period"))

                (ContentPanel as Grid).RowDefinitions[1].Height = new GridLength(80);
            else
                (ContentPanel as Grid).RowDefinitions[3].Height = new GridLength(80);
        }

        private void tbPeriodCycle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
                (sender as TextBox).Text = (sender as TextBox).Name.Contains("Period") ? "6" : "28";

            if ((sender as TextBox).Name.Contains("Period"))
            {
                (ContentPanel as Grid).RowDefinitions[1].Height = new GridLength(0);
            }
            else
            {
                (ContentPanel as Grid).RowDefinitions[3].Height = new GridLength(0);
            }
        }

        private void pkLastCycle_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e.NewDateTime != DateTime.MinValue && e.NewDateTime != e.OldDateTime && e.NewDateTime.HasValue)
            {
                if (e.NewDateTime.Value > DateTime.Now)
                {
                    (sender as DatePicker).BorderBrush = Application.Current.Resources["PinkColor"] as SolidColorBrush;
                    (ContentPanel as Grid).RowDefinitions[5].Height = new GridLength(80);
                }
                else
                {
                    (ContentPanel as Grid).RowDefinitions[5].Height = new GridLength(0);
                    (sender as DatePicker).BorderBrush = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void pkLastCycle_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            SetupViewModel.LastDateSelected = true;
        }

        #endregion
    }
}