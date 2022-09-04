using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EmployeeLibrary;
using Windows.UI.Popups;
using System.ComponentModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab4NatalieYu
{
    /// <summary>
    /// PROG1224 - Lab4
    /// Natalie Hyojung Yu
    /// Date: August 7, 2022
    /// </summary>
    public sealed partial class HourlyEmployees : Page
    {

        private List<Hourly> hourlyEmps;
        public HourlyEmployees()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            hourlyEmps = e.Parameter as List<Hourly>;
           
            if (hourlyEmps == null)
            {
                MessageDialog msg = new MessageDialog("No Hourly Employees exist");
                msg.ShowAsync();
            }
            base.OnNavigatedTo(e);
            lvwHourlyEmployees.ItemsSource = hourlyEmps;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        //TODO
        private void btnUpdateHourlyEmps_Click(object sender, RoutedEventArgs e)
        {
            ListView parent = (ListView)lvwHourlyEmployees;

            string s = "";
            foreach (Hourly h in parent.Items)
            {
                s += $"{h.FirstName}\trate: {h.Rate.ToString()}\thours: {h.Hours.ToString()}\n";
            }
            MessageDialog msg = new MessageDialog(s);
            msg.ShowAsync();

            //foreach (Hourly h in hourlyEmps)
            //{
            //    s += $"{h.FirstName}\trate: {h.Rate.ToString()}\thours: {h.Hours.ToString()}\n";
            //}
        }

        
        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
