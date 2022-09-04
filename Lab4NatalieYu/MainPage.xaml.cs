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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lab4NatalieYu
{
    /// <summary>
    /// PROG1224 - Lab4
    /// Natalie Hyojung Yu
    /// Date: August 7, 2022
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //declare a base reference
        internal List<Employee> employees;
        internal List<Employee> selectedEmployees = new List<Employee>();
        internal List<Hourly> hourlyEmployees = new List<Hourly>();
        internal List<Employee> checkedPayrollEmployees = new List<Employee>();
        private Employee current;

        public MainPage()
        {
            this.InitializeComponent();

            //retrieve employees and convert to list 
            this.employees = Data.GetEmployees().ToList();
            GetEmpList("All");

            //comboBox 
            cboEmpType.Items.Add("All");
            cboEmpType.Items.Add("Hourly");
            cboEmpType.Items.Add("Salary");
            cboEmpType.Items.Add("Manager");
            cboEmpType.Items.Add("Contract");
            cboEmpType.SelectedIndex = 0;
        }

        //combo box changed
        private void cboEmpType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetEmpList((string)cboEmpType.SelectedItem);
        }

        //clear the listview
        private void ClearSelectedEmployees()
        {
            selectedEmployees.Clear();
            lvwEmployees.ItemsSource = null;
        }

        //sort list by ascending alphabetical order
        private List<Employee> OrderByAsecding(List<Employee> sortingList)
        {
            List<Employee> sortedList = sortingList.OrderBy(emp => emp.FirstName).ToList();
            return sortedList;
        }

        //filter employees by combobox
        private void GetEmpList(string empType)
        {
            ClearSelectedEmployees();

            if (empType == "Hourly")
            {
                foreach (Employee emp in employees)
                {
                    if (emp is Hourly)
                        selectedEmployees.Add(emp);
                }
            } 
            else if (empType == "Salary")
            {
                foreach (Employee emp in employees)
                {
                    if (emp is Salary)
                        selectedEmployees.Add(emp);
                }
            }
            else if (empType == "Manager")
            {
                foreach (Employee emp in employees)
                {
                    if (emp is Manager)
                        selectedEmployees.Add(emp);
                }
            }
            else if (empType == "Contract")
            {
                foreach (Employee emp in employees)
                {
                    if (emp is Contract)
                        selectedEmployees.Add(emp);
                }
            }
            else
            {
                //'all' is selected in combo box
                foreach (Employee emp in employees)
                    selectedEmployees.Add(emp);
            }

            //sort list
            selectedEmployees = OrderByAsecding(selectedEmployees);
            lvwEmployees.ItemsSource = selectedEmployees;
        }

        //filter employees list according textbox input
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            selectedEmployees.Clear();
            string search = txtSearch.Text.ToLower();
            foreach (Employee emp in employees)
            {
                if (emp.FirstName.ToLower().Contains(search) || emp.LastName.ToLower().Contains(search) || emp.Sin.Contains(search))
                    selectedEmployees.Add(emp);
            }

            selectedEmployees = OrderByAsecding(selectedEmployees);
            lvwEmployees.ItemsSource = selectedEmployees;
        }
        
        //employee is selected in listview, display employee's info in the info section
        private void lvwEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            current = (Employee)lvwEmployees.SelectedItem;

            if (current is null) return;

            txtFirstName.Text = current.FirstName;
            txtLastName.Text = current.LastName;
            txtSin.Text = current.Sin;
            txtPhone.Text = current.Phone;
            txtEmail.Text = current.Email;
            txtAddress.Text = current.Address.ToString();
            dtpBirth.Date = current.BirthDate;
            dtpHire.Date = current.HireDate;

            ResetPaySection();
            
            if (current is Hourly)
            {
                Hourly hourlyEmp = (Hourly)current;
                rdoHourly.IsChecked = true;
                txtRate.Text = hourlyEmp.Rate.ToString();
                txtHour.Text = hourlyEmp.Hours.ToString();
            }
            else if (current is Salary)
            {
                Salary salaryEmp = (Salary)current;
                rdoSalary.IsChecked = true;
                txtSalary.Text = salaryEmp.Amount.ToString();
            }
            else if (current is Manager)
            {
                Manager managerEmp = (Manager)current;
                rdoSalary.IsChecked = true;
                txtSalary.Text = managerEmp.Amount.ToString();
            }
            else if (current is Contract)
            {
                Contract contractEmp = (Contract)current;
                rdoContract.IsChecked = true;
                txtContract.Text = contractEmp.AmountPerWeek.ToString();
            }
            btnModify.IsEnabled = true;
            btnModify.Content = "Update Employee";
        }

        //reset employee personal information
        private void ResetEmpInfo()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtSin.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            dtpBirth.SelectedDate = null;
            dtpHire.SelectedDate = null;
        }

        //reset employee pay section
        private void ResetPaySection()
        {
            rdoHourly.IsChecked = false;
            rdoSalary.IsChecked = false;
            rdoManager.IsChecked = false;
            rdoContract.IsChecked = false;
            txtRate.Text = "";
            txtRate.IsEnabled = false;
            txtHour.Text = "";
            txtHour.IsEnabled = false;
            txtSalary.Text = "";
            txtSalary.IsEnabled = false;
            txtContract.Text = "";
            txtContract.IsEnabled = false;
        }

        //move to process payroll page
        private void btnPayRoll_Click(object sender, RoutedEventArgs e)
        {
            checkedPayrollEmployees.Clear();

            // select only CheckBox checked(Active) employees
            foreach (Employee emp in selectedEmployees)
            {
                if (emp.Active == true)
                    checkedPayrollEmployees.Add(emp);
            }
            //sort employees asecding order
            checkedPayrollEmployees = OrderByAsecding(checkedPayrollEmployees);
            this.Frame.Navigate(typeof(ProcessPayinfo), checkedPayrollEmployees);
        }

        //move to hourly employees page
        private void btnHourly_Click(object sender, RoutedEventArgs e)
        {
            hourlyEmployees.Clear();
            foreach (Employee emp in employees)
            {
                if (emp is Hourly)
                {
                    Hourly hourlyEmp = (Hourly)emp;
                    hourlyEmployees.Add(hourlyEmp);
                }
            }
            hourlyEmployees = hourlyEmployees.OrderBy(emp => emp.FirstName).ToList();
            this.Frame.Navigate(typeof(HourlyEmployees), hourlyEmployees);
        }

        //add new employee
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnModify.IsEnabled = true;
            btnModify.Content = "Add New Employee Info";
            btnAdd.IsEnabled = false;
            ResetEmpInfo();
            ResetPaySection();
            rdoHourly.IsChecked = true;
        }

        
        //modify employees list - add employee, update employee
        private void btnModify_Click(object sender, RoutedEventArgs e)
        {

            if ((string)btnModify.Content == "Update Employee")
            {
                // update employee
                Employee newEmp = GetNewEmployee();
                if (newEmp != null)
                {
                    employees.Remove(current);
                    employees.Add(newEmp);
                }
                
                //refresh the listview
                lvwEmployees.ItemsSource = null;
                GetEmpList("All");

                //clear the textbox
                ResetEmpInfo();
                ResetPaySection();
            } 
            else
            {
                //add new employee to employees list
                Employee newEmp = GetNewEmployee();
                if (newEmp != null)
                    employees.Add(newEmp);

                //refresh the listview
                lvwEmployees.ItemsSource = null;
                GetEmpList("All");

                //clear the textbox
                ResetEmpInfo();
                ResetPaySection();
                btnAdd.IsEnabled = true;
            }
        }

        //get employee information and instantiate according employee type
        private Employee GetNewEmployee()
        {
            if (txtFirstName.Text == "" || txtLastName.Text == "" || txtSin.Text == "")
            {
                MessageDialog msg = new MessageDialog("Process failed. Please fill up the form.");
                msg.ShowAsync();
                return null;
            } 
            else
            {
                Employee newEmp;
                //employee type
                if (rdoHourly.IsChecked == true)
                {
                    decimal r = (txtRate.Text != "") ? Convert.ToDecimal(txtRate.Text) : 0M;
                    decimal h = (txtHour.Text != "") ? Convert.ToDecimal(txtHour.Text) : 0M;

                    newEmp = new Hourly(txtSin.Text, txtFirstName.Text, txtLastName.Text, r, h);
                    newEmp = GetEmployeeInfo(newEmp);
                }
                else if (rdoSalary.IsChecked == true)
                {
                    decimal a = (txtSalary.Text != "") ? Convert.ToDecimal(txtSalary.Text) : 0M;
                    newEmp = new Salary(txtSin.Text, txtFirstName.Text, txtLastName.Text, a);
                    newEmp = GetEmployeeInfo(newEmp);
                }
                else if (rdoManager.IsChecked == true)
                {
                    decimal a = (txtSalary.Text != "") ? Convert.ToDecimal(txtSalary.Text) : 0M;
                    newEmp = new Manager(txtSin.Text, txtFirstName.Text, txtLastName.Text, a);
                    newEmp = GetEmployeeInfo(newEmp);
                }
                else
                {
                    decimal a = (txtContract.Text != "") ? Convert.ToDecimal(txtContract.Text) : 0M;
                    newEmp = new Contract(txtSin.Text, txtFirstName.Text, txtLastName.Text, a);
                    newEmp = GetEmployeeInfo(newEmp);
                }
                return newEmp;
            }
        }

        private Employee GetEmployeeInfo(Employee e)
        {
            e.Phone = txtPhone.Text;
            e.Email = txtEmail.Text;
            e.BirthDate = dtpBirth.Date.DateTime;
            e.HireDate = dtpHire.Date.DateTime;
            return e;
        }

        //manage radio button and appropriate text boxs
        private void rdoHourly_Checked(object sender, RoutedEventArgs e)
        {
            txtRate.IsEnabled = true;
            txtHour.IsEnabled = true;
            txtSalary.IsEnabled = false;
            txtContract.IsEnabled = false;
        }

        private void rdoSalary_Checked(object sender, RoutedEventArgs e)
        {
            txtRate.IsEnabled = false;
            txtHour.IsEnabled = false;
            txtSalary.IsEnabled = true;
            txtContract.IsEnabled = false;
        }

        private void rdoContract_Checked(object sender, RoutedEventArgs e)
        {
            txtRate.IsEnabled = false;
            txtHour.IsEnabled = false;
            txtSalary.IsEnabled = false;
            txtContract.IsEnabled = true;
        }

        private void rdoManager_Checked(object sender, RoutedEventArgs e)
        {
            txtRate.IsEnabled = false;
            txtHour.IsEnabled = false;
            txtSalary.IsEnabled = true;
            txtContract.IsEnabled = false;
        }

    }
}
