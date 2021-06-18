using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Net;
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

        //arduino controls
        SerialPort port;
        bool isConnected = false;
        string connectAdruinoCommand = "#STAR\n";
        string soilMoistureAdruinoCommand = "#SOIL\n";
        string StopAdruinoCommand = "#STOP\n";

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new WAViewModel();
            this.DataContext = viewModel;
            WAViewModel.HeaderClick += WAViewModel_HeaderClick;
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

        private void HandleFieldSelection()
        {
            SPDetails.Visibility = Visibility.Visible;
            SPStatus.Visibility = Visibility.Visible;
            SPData.Visibility = Visibility.Visible;
            var list = new ObservableCollection<DataObject>();
            list.Add(new DataObject() { Dampness = "Dampness %", Rain = "Rain Forecast %", TapStatus = "Irrigation Status" });
            string canalName = string.Empty;
            string selectedFieldName = cmbFields.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();

            switch (selectedFieldName)
            {
                case "IKFB01":
                    lblDetails.Content = "10000 sq ft farm - planted urad dal as plantation for the duration of 2021-2022 - Mapped to canal IKCB001";
                    canalName = "IKCB001";
                    break;
                case "IKFM02":
                    lblDetails.Content = "7000 sq ft farm - planted rice dal as plantation for the duration of 2021-2022 - Mapped to canal IKCM121";
                    canalName = "IKCM121";
                    break;
                case "IKFZ03":
                    lblDetails.Content = "25000 sq ft farm - planted wheat dal as plantation for the duration of 2021-2022 - Mapped to canal IKCZ010";
                    canalName = "IKCZ010";
                    break;
            }
            string soilDampness = GetSoilMoisture(selectedFieldName);
            string rainForecast = GetPrecipitationData(selectedFieldName);
            var result = CalculateWaterTapStatus(soilDampness, rainForecast);
            list.Add(new DataObject() { Dampness = soilDampness, Rain = rainForecast ,TapStatus = result == true ? "ON" : "OFF" });
            this.dataGrid1.ItemsSource = list;
            txtTapStatus.Text = string.Format("The Auto irrigation for the selected field {0} with respect to the soil dampness and rain forecast is to be turned {1} and the same staus is been sent to the canal {2} admin.",
                selectedFieldName, result == true ? "ON" : "OFF", canalName);
        }
        private string GetPrecipitationData(string fieldName)
        {
            string data = string.Empty;
            try
            {
                string json = new WebClient().DownloadString("https://weatherapidata.eu-gb.mybluemix.net/api/values");
                WeatherData items = JsonConvert.DeserializeObject<WeatherData>(json);
                data = items.preceipechance;
            }
            catch (Exception)
            {
                Random random = new Random();
                data = random.Next(0, 100).ToString();
            }
            return data;
        }

        private string GetSoilMoisture(string fieldName)
        {
            string data = string.Empty;
            try
            {
                port.Write(soilMoistureAdruinoCommand);
                data = port.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get data from arduino: {ex.Message}");
                Random random = new Random();
                data = random.Next(30, 90).ToString();
            }
            return data;
        }


        private bool CalculateWaterTapStatus(string soilDampness, string rainForecast)
        {
            int soilDampnes = Convert.ToInt32(soilDampness);
            int weatherPercipitation = Convert.ToInt32(rainForecast);
            if (soilDampnes > 80 || weatherPercipitation > 80)
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

    public class DataObject
    {
        public string Dampness { get; set; }
        public string Rain { get; set; }
        public string TapStatus { get; set; }
    }

}