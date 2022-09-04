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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab4NatalieYu
{
    /// <summary>
    /// PROG1224 - Lab4
    /// Natalie Hyojung Yu
    /// Date: August 7, 2022
    /// </summary>
    public sealed partial class ProcessPayinfo : Page
    {
        private List<Employee> selectedEmps;

        public ProcessPayinfo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            selectedEmps = e.Parameter as List<Employee>; 
            if (selectedEmps == null)
            {
                MessageDialog msg = new MessageDialog("No employees are selected");
                msg.ShowAsync();
            }
            base.OnNavigatedTo(e);
            
        }

        //go back to the main page
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        
        private void btnProcessPayment_Click(object sender, RoutedEventArgs e)
        {
            DateTime d = dtpPeriod.Date.DateTime;
            d = (d.Year == 1600) ? DateTime.Now : dtpPeriod.Date.DateTime;

            ProcessEmployees<Employee> payroll = new ProcessEmployees<Employee>(d, selectedEmps);
            

            List<string> result = payroll.ProcessPayRoll();
            foreach (string s in result)
            {
                txtPaymentInfoOutput.Text += s;
            }
            dtpPeriod.Date = d;
            txtTotalEmps.Text = ProcessEmployees<Employee>.TotalCount;
            txtTotalPayment.Text = ProcessEmployees<Employee>.TotalPay;
            txtTotalBonus.Text = ProcessEmployees<Employee>.TotalBonus;
            txtTotalDeduction.Text = ProcessEmployees<Employee>.TotalDeduction;
        }
    }
}
