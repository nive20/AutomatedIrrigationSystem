using System;
using System.IO.Ports;
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

        //arduino controls
        SerialPort port;
        bool isConnected=false;
        string connectAdruinoCommand = "#STAR\n";
        string soilMoistureAdruinoCommand = "#SOIL\n";
        string StopAdruinoCommand = "#STOP\n";

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new WAViewModel();
            this.DataContext = viewModel;
            WAViewModel.HeaderClick += WAViewModel_HeaderClick;
            LoadComboBox();
            connectToArduino();
        }
        private void connectToArduino()
        {
            try
            {
                isConnected = true;
                var serialports = SerialPort.GetPortNames();
                port = new SerialPort(serialports.Last(), 9600, Parity.None, 8, StopBits.One);
                port.Open();
                port.Write(connectAdruinoCommand);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to arduino {ex.Message}");
            }
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
                    lblSoil.Content = GetSoilMoisture();
                    //Map data from the weather report-percipitation
                    lblWeather.Content = "20%";

                    break;
                case "IKFM02":
                    txtDetails.Text = "7000 sq ft farm - planted rice dal as plantation for the durationn of 2021-2022";
                    //Map data from the sensor from the location
                    lblSoil.Content = GetSoilMoisture();
                    //Map data from the weather report-percipitation
                    lblWeather.Content = "88%";
                    break;
                case "IKFZ03":
                    txtDetails.Text = "25000 sq ft farm - planted wheat dal as plantation for the durationn of 2021-2022";
                    //Map data from the sensor from the location
                    lblSoil.Content = GetSoilMoisture();
                    //Map data from the weather report-percipitation
                    lblWeather.Content = "12%";
                    break;
            }
            var result = CalculateWaterTapStatus();
            lblTapStatus.Content = result == true ? "ON" : "OFF";
        }

        private string GetSoilMoisture()
        {
            try
            {
                string data = string.Empty;
                port.Write(soilMoistureAdruinoCommand);
                while (true)
                {
                    data = port.ReadLine();
                    if (!string.IsNullOrEmpty(data))
                    {
                        data = data.Split(':')[1];
                        break;
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get data from arduino: {ex.Message}");
            }
            return "500";
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

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                isConnected = false;
                port.Write(StopAdruinoCommand);
                port.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Arduino not available.");
            }
        }
    }
}
