using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace WaterAllocationConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WAViewModel viewModel;
        private bool handleFields = true;
        private bool handleCanal = true;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new WAViewModel();
            this.DataContext = viewModel;
            WAViewModel.HeaderClick += WAViewModel_HeaderClick;
            LoadComboBox();
        }

        private void WAViewModel_HeaderClick(WAViewModel.ButtonClickType e)
        {
            switch (e.ToString())
            {
                case "Close":
                    Environment.Exit(0);
                    break;
                case "Minimize":
                    this.Dispatcher.Invoke(new MethodInvoker(() =>
                    {
                        this.WindowState = WindowState.Minimized;
                    }));
                    break;
            }
        }

        private void LoadComboBox()
        {
            //Load Canal Dropdown List
            cmbCanal.Items.Add("Canal1");
            cmbCanal.Items.Add("Canal2");
        }

        private void cmbFields_DropDownClosed(object sender, EventArgs e)
        {
            if (handleFields) 
                HandleFieldSelection();
            handleFields = true;
        }

        private void cmbFields_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                System.Windows.Controls.ComboBox cmb = sender as System.Windows.Controls.ComboBox;
                handleFields = !cmb.IsDropDownOpen;
                HandleFieldSelection();
            }
        }

        private void cmbCanal_DropDownClosed(object sender, EventArgs e)
        {
            if (handleCanal) 
                HandleCanalSelection();
            handleCanal = true;
        }

        private void cmbCanal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cmb = sender as System.Windows.Controls.ComboBox;
            handleCanal = !cmb.IsDropDownOpen;
            HandleCanalSelection();
        }

        private void HandleCanalSelection()
        {
            cmbFields.Items.Clear();
            SPDetails.Visibility = Visibility.Hidden;
            SPSoil.Visibility = Visibility.Hidden;
            SPWeather.Visibility = Visibility.Hidden;
            SPStatus.Visibility = Visibility.Hidden;
            switch (cmbCanal.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "Canal1":
                    cmbFields.Items.Add("IKFZ03");
                    break;
                case "Canal2":
                    cmbFields.Items.Add("IKFB01");
                    cmbFields.Items.Add("IKFM02");
                    break;
            }
        }

        private void HandleFieldSelection()
        {
            SPDetails.Visibility = Visibility.Visible;
            SPSoil.Visibility = Visibility.Visible;
            SPWeather.Visibility = Visibility.Visible;
            SPStatus.Visibility = Visibility.Visible;
            switch (cmbFields.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "IKFB01":
                    txtDetails.Text = "10000 sq ft farm - planted urad dal as plantation for the durationn of 2021-2022";
                    
                    //Map data from the sensor from the location
                    lblSoil.Content = "300";
                    //Map data from the weather report-percipitation
                    lblWeather.Content = "20%";

                    break;
                case "IKFM02":
                    txtDetails.Text = "7000 sq ft farm - planted rice dal as plantation for the durationn of 2021-2022";
                    //Map data from the sensor from the location
                    lblSoil.Content = "500";
                    //Map data from the weather report-percipitation
                    lblWeather.Content = "88%";
                    break;
                case "IKFZ03":
                    txtDetails.Text = "25000 sq ft farm - planted wheat dal as plantation for the durationn of 2021-2022";
                    //Map data from the sensor from the location
                    lblSoil.Content = "1000";
                    //Map data from the weather report-percipitation
                    lblWeather.Content = "12%";
                    break;
            }
            var result = CalculateWaterTapStatus();
            lblTapStatus.Content = result == true ? "ON" : "OFF";
        }

        private bool CalculateWaterTapStatus()
        {
            int soilDampness = Convert.ToInt32(lblSoil.Content);
            int weatherPercipitation = Convert.ToInt32(lblWeather.Content.ToString().Trim('%'));
            if (soilDampness < 500)
                return false;
            else if (weatherPercipitation > 80)
                return false;
            return true;
        }
    }
}
