using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using LED_Controller.Common;
using LED_Controller.Interface;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using Helper.Extensions;
using Helper.Controls.Dialogs;
using System.ComponentModel;
using System.IO.Ports;
using System.Collections.Concurrent;
using MaterialDesignThemes.Wpf;
using System.Threading;

namespace LED_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            SetValue(DevicesProperty, new ObservableCollection<BioLEDDevice>());
            SetValue(IsShutterOpenProperty, false);
            SetValue(COMPortsProperty, new ObservableCollection<string>());
            SetValue(BaudRatesProperty, new ObservableCollection<string>());
            this.DataContext = this;
        }

        static MainWindow()
        {
            InitCommands();
        }

        #endregion

        #region Dependency Properties



        public ObservableCollection<double> PresetIntensities
        {
            get { return (ObservableCollection<double>)GetValue(PresetIntensitiesProperty); }
            set { SetValue(PresetIntensitiesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PresetIntensities.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PresetIntensitiesProperty =
            DependencyProperty.Register("PresetIntensities", typeof(ObservableCollection<double>), typeof(MainWindow), new PropertyMetadata(null));


        public bool IsDarkUIMode
        {
            get { return (bool)GetValue(IsDarkUIModeProperty); }
            set { SetValue(IsDarkUIModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDarkUIMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDarkUIModeProperty =
            DependencyProperty.Register("IsDarkUIMode", typeof(bool), typeof(MainWindow), new FrameworkPropertyMetadata(false, OnDarkUIModeChanged));

        private static void OnDarkUIModeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PaletteHelper palette = new PaletteHelper();
            ITheme theme = palette.GetTheme();
            IBaseTheme basetheme = (bool)e.NewValue ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(basetheme);
            palette.SetTheme(theme);
        }

        public bool IsCompactMode
        {
            get { return (bool)GetValue(IsCompactModeProperty); }
            set { SetValue(IsCompactModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCompactMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCompactModeProperty =
            DependencyProperty.Register("IsCompactMode", typeof(bool), typeof(MainWindow), new PropertyMetadata(false, OnIsCompactModeChanged));

        private static void OnIsCompactModeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

            CollectionViewSource cvs = (CollectionViewSource)App.Current.MainWindow.Resources["FavouriteLEDs"];
            cvs.View.Refresh();
        }

        public ObservableCollection<string> COMPorts
        {
            get { return (ObservableCollection<string>)GetValue(COMPortsProperty); }
            set { SetValue(COMPortsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for COMPorts.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty COMPortsProperty =
            DependencyProperty.Register("COMPorts", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata(null));

        public ObservableCollection<string> BaudRates
        {
            get { return (ObservableCollection<string>)GetValue(BaudRatesProperty); }
            set { SetValue(BaudRatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BaudRates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaudRatesProperty =
            DependencyProperty.Register("BaudRates", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata(null));

        public string SelectedCOMPort
        {
            get { return (string)GetValue(SelectedCOMPortProperty); }
            set { SetValue(SelectedCOMPortProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedCOMPort.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCOMPortProperty =
            DependencyProperty.Register("SelectedCOMPort", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public string SelectedBaudRate
        {
            get { return (string)GetValue(SelectedBaudRateProperty); }
            set { SetValue(SelectedBaudRateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBaudRate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBaudRateProperty =
            DependencyProperty.Register("SelectedBaudRate", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        //public bool ShowFavouriteLEDsOnly
        //{
        //    get { return (bool)GetValue(ShowFavouriteLEDsOnlyProperty); }
        //    set { SetValue(ShowFavouriteLEDsOnlyProperty, value); }
        //}

        public bool IsSafeModeChangeEndabled
        {
            get { return (bool)GetValue(IsSafeModeChangeEndabledProperty); }
            set { SetValue(IsSafeModeChangeEndabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSafeModeChangeEndabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSafeModeChangeEndabledProperty =
            DependencyProperty.Register("IsSafeModeChangeEndabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        public bool IsShutterOpen
        {
            get { return (bool)GetValue(IsShutterOpenProperty); }
            set { SetValue(IsShutterOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShutterOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShutterOpenProperty =
            DependencyProperty.Register("IsShutterOpen", typeof(bool), typeof(MainWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, OnIsShutterOpenChanged));

        private static void OnIsShutterOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mw = (MainWindow)o;
            mw.ShutterButtonText = ((bool)e.NewValue) ? "Close Shutter" : "Open Shutter";
        }

        public string ShutterButtonText
        {
            get { return (string)GetValue(ShutterButtonTextProperty); }
            set { SetValue(ShutterButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShutterButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShutterButtonTextProperty =
            DependencyProperty.Register("ShutterButtonText", typeof(string), typeof(MainWindow), new PropertyMetadata("Open Shutter"));

        public Settings Settings
        {
            get { return (Settings)GetValue(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Settings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingsProperty =
            DependencyProperty.Register("Settings", typeof(Settings), typeof(MainWindow), new PropertyMetadata(null));

        public ObservableCollection<BioLEDDevice> Devices
        {
            get { return (ObservableCollection<BioLEDDevice>)GetValue(DevicesProperty); }
            set { SetValue(DevicesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Devices.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevicesProperty =
            DependencyProperty.Register("Devices", typeof(ObservableCollection<BioLEDDevice>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnDevicesChanged));

        private static void OnDevicesChanged(object o, DependencyPropertyChangedEventArgs e)
        {

        }


        public bool HasDevices
        {
            get { return (bool)GetValue(HasDevicesProperty); }
            set { SetValue(HasDevicesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasDevices.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasDevicesProperty =
            DependencyProperty.Register("HasDevices", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));



        public BioLEDDevice SelectedDevice
        {
            get { return (BioLEDDevice)GetValue(SelectedDeviceProperty); }
            set { SetValue(SelectedDeviceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDevice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDeviceProperty =
            DependencyProperty.Register("SelectedDevice", typeof(BioLEDDevice), typeof(MainWindow), new PropertyMetadata(null));

        public LED SelectedLED
        {
            get { return (LED)GetValue(SelectedLEDProperty); }
            set { SetValue(SelectedLEDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedLED.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLEDProperty =
            DependencyProperty.Register("SelectedLED", typeof(LED), typeof(MainWindow), new PropertyMetadata(null));



        #endregion

        #region Commands

        private static RoutedCommand AllLEDsOffCommand_;
        public static RoutedCommand AllLEDsOffCommand
        {
            get { return AllLEDsOffCommand_; }
        }

        private static RoutedCommand AddLEDCommand_;
        public static RoutedCommand AddLEDCommand
        {
            get
            {
                return AddLEDCommand_;
            }
        }

        private static RoutedCommand RemoveLEDCommand_;
        public static RoutedCommand RemoveLEDCommand
        {
            get
            {
                return RemoveLEDCommand_;
            }
        }

        private static RoutedCommand EditLEDCommand_;
        public static RoutedCommand EditLEDCommand
        {
            get
            {
                return EditLEDCommand_;
            }
        }

        private static RoutedCommand ConnectArduinoCommand_;
        public static RoutedCommand ConnectArduinoCommand
        {
            get
            {
                return ConnectArduinoCommand_;
            }
        }

        private static RoutedCommand ScanDevicesCommand_;
        public static RoutedCommand ScanDevicesCommand
        {
            get
            {
                return ScanDevicesCommand_;
            }
        }

        private static RoutedCommand RemoveDeviceCommand_;
        public static RoutedCommand RemoveDeviceCommand
        {
            get
            {
                return ScanDevicesCommand_;
            }
        }

        static void InitCommands()
        {
            AddLEDCommand_ = new RoutedCommand("AddLEDCommand", typeof(MainWindow));
            RemoveLEDCommand_ = new RoutedCommand("RemoveLEDCommand", typeof(MainWindow));
            EditLEDCommand_ = new RoutedCommand("EditLEDCommand", typeof(MainWindow));

            AllLEDsOffCommand_ = new RoutedCommand("AllLEDsOffCommand", typeof(MainWindow));

            ConnectArduinoCommand_ = new RoutedCommand("ConnectArduinoCommand", typeof(MainWindow));
            ScanDevicesCommand_ = new RoutedCommand("ScanDevicesCommand", typeof(MainWindow));
            RemoveDeviceCommand_ = new RoutedCommand("RemoveDeviceCommand", typeof(MainWindow));


            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(AddLEDCommand_, OnAddLED, OnQueryAddLED));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(RemoveLEDCommand_, OnRemoveLED, OnQueryRemoveLED));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(EditLEDCommand_, OnEditLED, OnQueryEditLED));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(AllLEDsOffCommand_, OnAllLEDsOff, OnQueryAllLEDsOff));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(ConnectArduinoCommand_, OnConnectArduino, OnQueryConnectArduino));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(ScanDevicesCommand_, OnScanDevices, OnQueryScanDevices));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(RemoveDeviceCommand_, OnRemoveDevice, OnQueryRemoveDevice));

        }


        private static void OnAddLED(object o, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)o;
            mw.AddLED();
        }
        private static void OnQueryAddLED(object o, CanExecuteRoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)o;
            if (mw.SelectedDevice == null)
            {
                e.CanExecute = false;
                return;
            }
            if (mw.SelectedDevice.LEDs.Count() < mw.SelectedDevice.ChannelCount)
            {
                e.CanExecute = true;
            }
            else e.CanExecute = false;
        }

        private static void OnRemoveLED(object o, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)o;
            mw.RemoveLED((LED)e.Parameter);
        }
        private static void OnQueryRemoveLED(object o, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter as LED != null;
        }

        private static void OnEditLED(object o, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is LED led)
            {
                MainWindow mw = (MainWindow)o;
                mw.EditLED((LED)e.Parameter);
            }
        }
        private static void OnQueryEditLED(object o, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is LED led)
            {
                e.CanExecute = true;
                return;
            }

            e.CanExecute = false;
        }

        private static void OnAllLEDsOff(object o, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)o;
            mw.AllLEDsOff();
        }
        private static void OnQueryAllLEDsOff(object o, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void OnConnectArduino(object o, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)o;
            mw.ConnectMonitor_Click(null, null);
        }

        private static void OnQueryConnectArduino(object o, CanExecuteRoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)o;
            if (string.IsNullOrWhiteSpace(mw.SelectedBaudRate))
            {
                e.CanExecute = false;
                return;
            }
            if (string.IsNullOrWhiteSpace(mw.SelectedCOMPort))
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = true;
        }

        private static void OnScanDevices(object o, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = o as MainWindow;
            mw.Connect_Click(null, null);
        }

        private static void OnQueryScanDevices(object o, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void OnRemoveDevice(object o, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)o;
            mw.RemoveDevice((BioLEDDevice)e.Parameter);
        }
        private static void OnQueryRemoveDevice(object o, CanExecuteRoutedEventArgs e)
        {
            var device = e.Parameter as BioLEDDevice;
            if (device != null)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        #endregion

        #region Public Methods

        public void EditLED(LED LED)
        {
            object o = App.Current.Resources["DIALOG_EditLED"];
            ((FrameworkElement)o).DataContext = LED;

            var result = DialogHost.Show(o, delegate (object oo, DialogOpenedEventArgs args)
            {
                SaveSettings();
                snackbar.MessageQueue.Enqueue(string.Format("LED ({0} nm) EDITED", LED.Wavelength));
            });
        }

        public void AllLEDsOff()
        {
            if (SelectedDevice != null)
            {
                foreach (LED led in this.SelectedDevice.LEDs.Where(a => a.IsConnected))
                {
                    led.Mode = LEDModeEnum.Disabled;
                }
            }
        }

        public async void AddLED()
        {
            if (this.SelectedDevice.LEDs.Count() >= this.SelectedDevice.ChannelCount)
            {
                snackbar.MessageQueue.Enqueue("Unable to add LED. All Device Channels used");
            }

            object o = App.Current.Resources["DIALOG_EditLED"];

            LED led = new LED() { Wavelength = 470, Device = this.SelectedDevice };

            led.DeviceChannelIndex = this.SelectedDevice.LEDs.Count() + 1;

            ((FrameworkElement)o).DataContext = led;

            var result = await DialogHost.Show(o);

            if ((bool)result)
            {
                led.IntensityChanged += LED_IntensityChanged;
                led.IsOnChanged += LED_IsOnChanged;
                led.ModeChanged += LED_ModeChanged;

                this.SelectedDevice.LEDs.Add(led);
                snackbar.MessageQueue.Enqueue(string.Format("LED ({0} nm) ADDED", led.Wavelength));
                SaveSettings();
            }
        }

        public void RemoveLED(LED LED)
        {
            LED.IntensityChanged -= LED_IntensityChanged;
            LED.IsOnChanged -= LED_IsOnChanged;
            LED.ModeChanged -= LED_ModeChanged;

            this.SelectedDevice.LEDs.Remove(LED);
            snackbar.MessageQueue.Enqueue(string.Format("LED REMOVED", LED.Wavelength));

            SaveSettings();
        }

        public void RemoveDevice(BioLEDDevice Device)
        {
            this.SelectedLED = null;
            this.SelectedDevice = null;

            //Device.PropertyChanged -= propertych

            this.Devices.Remove(Device);
        }

        #endregion

        #region LED Callbacks

        void LED_ModeChanged(object sender, Helper.Common.EventArgs<LEDModeEnum> e)
        {
            LED led = (LED)sender;
            Log("LED_ModeChanged called for (new value = {0}) for {1}", e.Value.ToString(), led.DeviceDetailString);

            if (e.Value == LEDModeEnum.Pulse)
            {
                MessageBox.Show("This Mode is not currently supported");
                led.Mode = LEDModeEnum.Disabled;
            }

            BioLEDInterface.MTUSB_BLSDriverSetMode(led.Device.DeviceHandle, led.DeviceChannelIndex, e.Value.LEDModeToInt());
        }

        void LED_IsOnChanged(object sender, Helper.Common.EventArgs<bool> e)
        {
            LED led = (LED)sender;
            Send_LEDIntensity(led, led.Intensity);      //Send_LEDIntensity will check the ON/OFF status, and send the appropriate Intensity
            Log("LED_IsOnChanged called for {0}", led.DeviceDetailString);
            //try
            //{
            //    LED led = (LED)sender;

            //    BioLEDInterface.MTUSB_BLSDriverSetNormalCurrent(led.Device.DeviceHandle, led.DeviceChannelIndex + 1, e.Value ? (int)Math.Floor(led.Intensity * 1000) : 0);

            //    if (led.IsLinkedToAnalogueChannel)
            //    {
            //        if (port != null && port.IsOpen)
            //        {
            //            port.WriteLine(string.Format("{0} S{1}", led.LinkedAnalogueChannelName, e.Value ? "1" : "0"));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(string.Format("Send_LEDIntensity failed\n\n{0} {1}\n\n{2}", ex.HResult, ex.Message, ex.ToString()), "Error");
            //}
        }

        void LED_IntensityChanged(object sender, Helper.Common.EventArgs<double> e)
        {
            LED led = (LED)sender;
            //Debug.Print("LED_IntensityChanged to {0} for {1}", e.Value.ToString(), led.ToString());

            Send_LEDIntensity(led, e.Value);
        }

        #endregion

        #region Settings

        private void SaveSettings()
        {
            XmlWriter writer = XmlWriter.Create(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + "LED Controller.settings");
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            Settings s = new Settings();
            s.Devices = this.Devices.ToList();

            s.BaudRate = string.IsNullOrWhiteSpace(this.SelectedBaudRate) ? this.Settings.BaudRate : this.SelectedBaudRate;
            s.COMPort = string.IsNullOrWhiteSpace(this.SelectedCOMPort) ? this.Settings.COMPort : this.SelectedCOMPort;
            s.IsCompactMode = this.IsCompactMode;
            s.IsDarkUIMode = this.IsDarkUIMode;
            s.PresetIntensities = this.PresetIntensities?.ToList();

            serializer.Serialize(writer, s);

            writer.Close();

            snackbar.MessageQueue.Enqueue("SAVED");
        }

        private void LoadSettings()
        {
            Log("LoadSettings() called Filename: {0}", settingsfilename);

            XmlReader reader = null;
            try
            {
                reader = XmlReader.Create(settingsfilename);
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                this.Settings = (Settings)serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                this.Settings = new Settings();
                Log("LoadSettings() failed ({0})", e.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            snackbar.MessageQueue.Enqueue(string.Format("OPENED\n\n{0}", settingsfilename));

            this.COMPorts.Clear();
            foreach (string port in SerialPort.GetPortNames())
            {
                this.COMPorts.Add(port);
            }

            this.BaudRates.Clear();

            this.BaudRates.Add("2400");
            this.BaudRates.Add("4800");
            this.BaudRates.Add("9600");
            this.BaudRates.Add("14400");
            this.BaudRates.Add("19200");
            this.BaudRates.Add("28800");
            this.BaudRates.Add("38400");
            this.BaudRates.Add("57600");
            this.BaudRates.Add("115200");

            if (this.COMPorts.Contains(this.Settings.COMPort))
                this.SelectedCOMPort = this.Settings.COMPort;

            this.SelectedBaudRate = this.Settings.BaudRate;

            this.Devices.Clear();
            Settings.Devices.ForEach(a => this.Devices.Add(a));
            HasDevices = Devices.Count > 0;

            foreach (BioLEDDevice device in Devices)
            {
                device.LEDs.AsParallel().ForAll(a => a.Device = device);
                device.LEDs.AsParallel().ForAll(a => a.IsOn = false);

                device.LEDs.AsParallel().ForAll(a => a.IntensityChanged += LED_IntensityChanged);
                device.LEDs.AsParallel().ForAll(a => a.IsOnChanged += LED_IsOnChanged);
                device.LEDs.AsParallel().ForAll(a => a.ModeChanged += LED_ModeChanged);

                foreach (LED led in device.LEDs)
                {
                    BioLEDInterface.MTUSB_BLSDriverSetMode(led.Device.DeviceHandle, led.DeviceChannelIndex, led.Mode.LEDModeToInt());
                }
            }

            if (this.Settings.PresetIntensities == null)
                this.Settings.PresetIntensities = Settings.DefaultPresetIntensities.ToList();       //default Preset Intensities
            else if (this.Settings.PresetIntensities.Count == 0)
                this.Settings.PresetIntensities = Settings.DefaultPresetIntensities.ToList();       //default Preset Intensities

            this.SelectedDevice = Devices.FirstOrNullObject(null);
            this.IsDarkUIMode = Settings.IsDarkUIMode;
            this.IsCompactMode = Settings.IsCompactMode;
            this.PresetIntensities = new ObservableCollection<double>(Settings.PresetIntensities);
        }

        private string settingsfilename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + "LED Controller.settings";

        #endregion

        #region Window Open/Close

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();

            if (port != null)
            {
                port.Close();
            }
            base.OnClosing(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }

        #endregion

        #region Private Functions

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            //Remove event handlers from OLD devices before calling GetDevices (which clears all devices)
            foreach (BioLEDDevice device in Devices)
            {
                Log(string.Format("Removing event handlers from BioLEDDevice #{0}", device.SerialNumber));

                device.LEDs.AsParallel().ForAll(a => a.IntensityChanged -= LED_IntensityChanged);
                device.LEDs.AsParallel().ForAll(a => a.IsOnChanged -= LED_IsOnChanged);
                device.LEDs.AsParallel().ForAll(a => a.ModeChanged -= LED_ModeChanged);
            }

            GetDevices();

            foreach (BioLEDDevice device in Devices)
            {
                Log(string.Format("Adding event handlers for BioLEDDevice #{0}", device.SerialNumber));

                device.LEDs.AsParallel().ForAll(a => a.Device = device);
                device.LEDs.AsParallel().ForAll(a => a.IntensityChanged += LED_IntensityChanged);
                device.LEDs.AsParallel().ForAll(a => a.IsOnChanged += LED_IsOnChanged);
                device.LEDs.AsParallel().ForAll(a => a.ModeChanged += LED_ModeChanged);

                foreach (LED led in device.LEDs)
                {
                    BioLEDInterface.MTUSB_BLSDriverSetMode(led.Device.DeviceHandle, led.DeviceChannelIndex, led.Mode.LEDModeToInt());
                }
            }

        }

        private async void GetDevices()
        {
            //Devices.Clear();
            Log("GetDevices() called");

            int devicecount = BioLEDInterface.MTUSB_BLSDriverInitDevices();

            //snackbar.MessageQueue.Enqueue("Get Devices");

            Log(string.Format("{0} BioLED devices could be found (BioLEDInterface.MTUSB_BLSDriverInitDevices())", devicecount));

            if (devicecount == 0)
            {
                var result = await DialogHost.Show(App.Current.Resources["Dialog_NoBioLEDDevices"]);
                return;
            }

            for (int i = 0; i < devicecount; i++)
            {
                int result = BioLEDInterface.MTUSB_BLSDriverOpenDevice(i);

                Log(string.Format("Device # {0} OPEN called, return = {1}", i, result));
                //MessageBox.Show(string.Format("Device # {0} OPEN called, return = {1}", i, result));

                if (result > -1)
                {
                    BioLEDDevice device = new BioLEDDevice(result);

                    StringBuilder sb = new StringBuilder(256);

                    int ok = BioLEDInterface.MTUSB_BLSDriverGetSerialNo(device.DeviceHandle, sb, 256);

                    //Debug.Print("MTUSB_BLSDriverGetSerialNo returned {0}", ok);
                    device.SerialNumber = sb.ToString();

                    int channelcount = BioLEDInterface.MTUSB_BLSDriverGetChannels(device.DeviceHandle);

                    Log(string.Format("MTUSB_BLSDriverGetChannels returned {0}", channelcount));
                    //MessageBox.Show(string.Format("MTUSB_BLSDriverGetChannels returned {0}", channelcount));

                    device.ChannelCount = channelcount;
                    device.IsConnected = true;

                    //MessageBox.Show(device.ToString());
                    if (this.Devices.Any(a => a.SerialNumber == device.SerialNumber))
                    {
                        //Device already present (ie. was loaded from the Settings File), therefore simply update its status to IsConnected
                        this.Devices.First(a => a.SerialNumber == device.SerialNumber).IsConnected = true;
                        Log(string.Format("Device {0} found", device.SerialNumber));
                    }
                    else
                    {
                        Devices.Add(device);
                        Log("Device (new) added");
                    }
                    HasDevices = Devices.Count > 0;
                }
            }
        }

        //private bool controllerpassedtest = false;
        private string controllerteststring;

        public static SerialPort port;
        private Dictionary<string, int> previousvalue = new Dictionary<string, int>();

        private static double MapAnalogueToPercentage(string AnalogueChannelID, int Value)
        {
            //find calibration information for the given AnalogueChannelID
            double factor = (1d / 1024d);
            double offset = 0;


            //return (double)Value * factor;

            return Math.Round(((Value - offset) * factor), 3);
        }

        private bool IsCloseTo(int Value1, int Value2, int Jitter)
        {
            if (Value1 == Value2)
                return true;
            if (Value1 == 0)
                return false;
            if (Value1 >= 255)
                return false;
            if ((int)Math.Abs(Value1 - Value2) <= Jitter)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LED"></param>
        /// <param name="Value">As percentage (e.g. 0.5 = 50%)</param>
        private void Send_LEDIntensity(LED LED, double Value)
        {

            if (LED.Mode == LEDModeEnum.Disabled)
            {
                _ = BioLEDInterface.MTUSB_BLSDriverSetNormalCurrent(LED.Device.DeviceHandle, LED.DeviceChannelIndex, 0);
                _ = BioLEDInterface.MTUSB_BLSDriverSetFollowModeDetail(LED.Device.DeviceHandle, LED.DeviceChannelIndex, 0, 0);
            }
            else
            {
                if (LED.Mode == LEDModeEnum.Constant)
                {
                    if (LED.IsOn == false)
                    {
                        //LED is OFF, send 0 Intensity
                        _ = BioLEDInterface.MTUSB_BLSDriverSetNormalCurrent(LED.Device.DeviceHandle, LED.DeviceChannelIndex, 0);
                        Log("Send_LEDIntensity:   {0} LED is OFF (sending value:0)", LED.DeviceDetailString);
                        return;
                    }
                    else
                    {
                        _ = BioLEDInterface.MTUSB_BLSDriverSetNormalCurrent(LED.Device.DeviceHandle, LED.DeviceChannelIndex, (int)Math.Floor(LED.Intensity * 10));
                        Log(string.Format("Send_LEDIntensity:    {0} (sending value:{1})", LED.DeviceDetailString, (int)Math.Floor(LED.Intensity * 10)));
                    }
                }
                if (LED.Mode == LEDModeEnum.Follower)
                    //What to do if LED is OFF, for FOLLOWER mode?
                    if (LED.IsOn == false)
                    {
                        //LED is OFF, send 0 Intensity
                        _ = BioLEDInterface.MTUSB_BLSDriverSetFollowModeDetail(LED.Device.DeviceHandle, LED.DeviceChannelIndex, 0, 0);
                        Log("Send_LEDIntensity:    {0} LED is OFF (sending value:0)", LED.DeviceDetailString);
                        return;
                    }
                    else
                    {
                        _ = BioLEDInterface.MTUSB_BLSDriverSetFollowModeDetail(LED.Device.DeviceHandle, LED.DeviceChannelIndex, (int)Math.Floor(LED.Intensity * 10), (int)Math.Floor(LED.OffIntensity * 10));
                        Log(string.Format("Send_LEDIntensity:    {0} (sending value:{1})", LED.DeviceDetailString, (int)Math.Floor(LED.Intensity * 10)));

                    }
                if (LED.Mode == LEDModeEnum.Pulse)
                    DialogHost.Show(this.Resources["DIALOG_NotImplemented"]);

                if (sentvalues.ContainsKey(LED))
                    sentvalues[LED] = Value;
                else
                    sentvalues.Add(LED, Value);
            }
        }

        private Dictionary<LED, double> sentvalues = new Dictionary<LED, double>();

        #endregion

        #region Ardunio Connection

        private async void ConnectMonitor_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(SelectedBaudRate))
                {
                    var result = await DialogHost.Show(App.Current.Resources["Dialog_BaudRateNotSet"]);
                    return;
                }
                if (string.IsNullOrWhiteSpace(SelectedCOMPort))
                {
                    var result = await DialogHost.Show(App.Current.Resources["Dialog_COMPortNotSet"]);
                    return;
                }

                previousvalue.Clear();

                if (port != null && port.IsOpen)
                {
                    port.Close();
                    port.Dispose();
                }

                port = new SerialPort(SelectedCOMPort);
                port.BaudRate = int.Parse(SelectedBaudRate);
                port.DtrEnable = true;
                port.ReadTimeout = 5000;
                port.WriteTimeout = 500;
                port.DataReceived += port_DataReceived;

                port.Open();

                //send test string
                controllerteststring = string.Format("TEST {0}", new Random(2).NextDouble());

                try
                {
                    port.WriteLine(controllerteststring);
                    //controllerpassedtest = true;
                }
                catch (System.TimeoutException)
                {
                    var result = await DialogHost.Show(App.Current.Resources["Dialog_COMPorttTimeout"]);

                    port.Close();
                    port.Dispose();
                }
            }
            catch (Exception ex)
            {
                var result = await DialogHost.Show(string.Format("Connect Arduino failed\n\n{0} {1}\n\n{2}", ex.HResult, ex.Message, ex.ToString()));
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Debug.Print("port_DataReceived {0}", e.EventType);

            if (port.IsOpen)
            {
                string s = port.ReadLine();
                if (string.IsNullOrWhiteSpace(s))
                    return;

                //if (s == controllerteststring)
                //    controllerpassedtest = true;


                string ID = s.Substring(0, 2);

                string valuestring = s.Substring(2).Trim();

                if (valuestring.StartsWith("B"))
                {
                    Debug.Print("ID: {0}, button state: {1}", ID, valuestring);
                    LED led = Dispatcher.Invoke(new Func<LED>(delegate
                    {
                        LED result = SelectedDevice.LEDs.FirstOrNullObject(a => a.IsLinkedToAnalogueChannel && a.LinkedAnalogueChannelName == ID, null);
                        return result;
                    }), System.Windows.Threading.DispatcherPriority.Normal);
                    if (led == null)
                        return;
                    led.IsOn = !led.IsOn;
                    return;
                }

                int rawvalue = int.Parse(valuestring);

                Debug.Print("ID: {0}, rawvalue: {1}", ID, rawvalue);

                if (previousvalue.ContainsKey(ID) == false)
                {
                    //Debug.Print("creating previousvalue entry");
                    previousvalue.Add(ID, -1);
                }

                if (IsCloseTo(previousvalue[ID], rawvalue, 5) == false)
                {
                    //Debug.Print("finding LED now");
                    LED led = Dispatcher.Invoke(new Func<LED>(delegate
                    {
                        LED result = SelectedDevice.LEDs.FirstOrNullObject(a => a.IsLinkedToAnalogueChannel && a.LinkedAnalogueChannelName == ID, null);
                        return result;
                    }), System.Windows.Threading.DispatcherPriority.Normal);


                    if (led != null)
                    {
                        //Debug.Print("LED found {0}", led.ToString());
                        double value = MapAnalogueToPercentage(ID, rawvalue);

                        Debug.Print("valuestring = {0}, rawvalue {1} mapped to {2}", valuestring, rawvalue, value.ToString("0.000"));
                        previousvalue[ID] = rawvalue;


                        led.Intensity = value;
                        //Send_LEDIntensity(led, value);
                    }

                }
            }
        }

        #endregion

        #region Menu items

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void ArduinoDialog_Click(object sender, RoutedEventArgs e)
        {
            object o = App.Current.Resources["DIALOG_ArduinoDevice"];
            var result = await DialogHost.Show(o);
        }

        private void CreateDEMODevice_Click(object sender, RoutedEventArgs e)
        {
            Devices.Clear();
            Devices.Add(new BioLEDDevice(100) { ChannelCount = 4, SerialNumber = "DEMO DEVICE", IsConnected = true });
            HasDevices = Devices.Count > 0;
            return;
        }

        private async void About_Click(object sender, RoutedEventArgs e)
        {
            object o = App.Current.Resources["DIALOG_About"];

            AboutViewModel vm = new AboutViewModel();
            vm.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            vm.SettingsFolder = System.IO.Path.GetDirectoryName(settingsfilename);
            ((FrameworkElement)o).DataContext = vm;
            var result = await DialogHost.Show(o);
        }

        private async void PresetIntensities_Click(object sender, RoutedEventArgs e)
        {
            object o = App.Current.Resources["DIALOG_PresetIntensities"];
            PresetLEDIntensitiesViewModel vm = new PresetLEDIntensitiesViewModel() { PresetLEDIntensities = new ObservableCollection<double>(this.PresetIntensities.ToList()) };
            ((FrameworkElement)o).DataContext = vm;

            var result = await DialogHost.Show(o);

            this.PresetIntensities.Clear();
            this.PresetIntensities.AddRangeUnique(vm.PresetLEDIntensities);
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            //Debug.Print("CollectionViewSource_Filter");
            var led = e.Item as LED;
            e.Accepted = IsCompactMode ? led.IsFavourite : true;
        }

        #endregion

        #region Logging

        public ObservableCollection<string> Logs
        {
            get { return (ObservableCollection<string>)GetValue(LogsProperty); }
            set { SetValue(LogsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Logs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogsProperty =
            DependencyProperty.Register("Logs", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<string>()));


        private void Log(string Message)
        {
            this.Logs.Add(string.Format("{0}:\t{1}", Logs.Count, Message));
            Debug.Print("Log: {0}", Message);
        }

        private void Log(string Format, params object[] p)
        {
            Log(string.Format(Format, p));
        }

        #endregion

    }
}
