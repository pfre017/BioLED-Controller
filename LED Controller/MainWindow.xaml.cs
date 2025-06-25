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
            DataContext = this;
        }

        static MainWindow()
        {
            InitCommands();
        }

        #endregion

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

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
            if (((bool)e.NewValue) == true)
                ThemeAssist.SetTheme(((MainWindow)o), BaseTheme.Dark);
            else
                ThemeAssist.SetTheme(((MainWindow)o), BaseTheme.Light);
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
            DependencyProperty.Register("Devices", typeof(ObservableCollection<BioLEDDevice>), typeof(MainWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

        public bool HasDevices
        {
            get { return (bool)GetValue(HasDevicesProperty); }
            set { SetValue(HasDevicesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasDevices.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasDevicesProperty =
            DependencyProperty.Register("HasDevices", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));



        public BioLEDDevice? SelectedDevice
        {
            get { return (BioLEDDevice?)GetValue(SelectedDeviceProperty); }
            set { SetValue(SelectedDeviceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDevice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDeviceProperty =
            DependencyProperty.Register("SelectedDevice", typeof(BioLEDDevice), typeof(MainWindow), new PropertyMetadata(null));

        public LED? SelectedLED
        {
            get { return (LED?)GetValue(SelectedLEDProperty); }
            set { SetValue(SelectedLEDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedLED.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLEDProperty =
            DependencyProperty.Register("SelectedLED", typeof(LED), typeof(MainWindow), new PropertyMetadata(null));



        #endregion

        #region Commands

        private static RoutedCommand? AllLEDsOffCommand_;
        public static RoutedCommand? AllLEDsOffCommand
        {
            get { return AllLEDsOffCommand_; }
        }

        private static RoutedCommand? AddLEDCommand_;
        public static RoutedCommand? AddLEDCommand
        {
            get
            {
                return AddLEDCommand_;
            }
        }

        private static RoutedCommand? RemoveLEDCommand_;
        public static RoutedCommand? RemoveLEDCommand
        {
            get
            {
                return RemoveLEDCommand_;
            }
        }

        private static RoutedCommand? EditLEDCommand_;
        public static RoutedCommand? EditLEDCommand
        {
            get
            {
                return EditLEDCommand_;
            }
        }

        private static RoutedCommand? ConnectArduinoCommand_;
        public static RoutedCommand? ConnectArduinoCommand
        {
            get
            {
                return ConnectArduinoCommand_;
            }
        }

        private static RoutedCommand? ScanDevicesCommand_;
        public static RoutedCommand? ScanDevicesCommand
        {
            get
            {
                return ScanDevicesCommand_;
            }
        }

        private static RoutedCommand? RemoveDeviceCommand_;
        public static RoutedCommand? RemoveDeviceCommand
        {
            get
            {
                return RemoveDeviceCommand_;
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
            if (mw.SelectedDevice.LEDs.Count < mw.SelectedDevice.ChannelCount)
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
                mw.EditLED(led);
            }
        }
        private static void OnQueryEditLED(object o, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is LED)
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
            throw new NotImplementedException("ConnectArduino command is not implemented yet.");
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
            MainWindow? mw = o as MainWindow;
            mw?.Connect_Click(mw, null);
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
            if (e.Parameter is BioLEDDevice)
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
                snackbar.MessageQueue?.Enqueue(string.Format("LED ({0} nm) EDITED", LED.Wavelength));
            });
        }

        public void AllLEDsOff()
        {
            if (SelectedDevice != null)
            {
                foreach (LED led in SelectedDevice.LEDs.Where(a => a.IsConnected))
                {
                    led.Mode = LEDModeEnum.Disabled;
                }
            }
        }

        public async void AddLED()
        {
            if (SelectedDevice == null)
            {
                snackbar.MessageQueue?.Enqueue("Unable to add LED. No Device Selected");
                return;
            }

            if (SelectedDevice.LEDs.Count >= SelectedDevice.ChannelCount)
            {
                snackbar.MessageQueue?.Enqueue("Unable to add LED. All Device Channels used");
            }

            object o = App.Current.Resources["DIALOG_EditLED"];

            LED led = new()
            {
                Wavelength = 470,
                Device = SelectedDevice,
                DeviceChannelIndex = SelectedDevice.LEDs.Count + 1
            };

            ((FrameworkElement)o).DataContext = led;

            var result = await DialogHost.Show(o);

            if (result == null)
                return;

            if ((bool)result)
            {
                led.IntensityChanged += LED_IntensityChanged;
                led.IsOnChanged += LED_IsOnChanged;
                led.ModeChanged += LED_ModeChanged;

                SelectedDevice.LEDs.Add(led);
                snackbar.MessageQueue?.Enqueue($"LED ({led.Wavelength} nm) ADDED");
                SaveSettings();
            }
        }

        public void RemoveLED(LED LED)
        {
            if (SelectedDevice == null)
            {
                snackbar.MessageQueue?.Enqueue("Unable to remove LED. No Device Selected");
                return;
            }

            LED.IntensityChanged -= LED_IntensityChanged;
            LED.IsOnChanged -= LED_IsOnChanged;
            LED.ModeChanged -= LED_ModeChanged;

            _ = SelectedDevice.LEDs.Remove(LED);
            snackbar.MessageQueue?.Enqueue($"LED ({LED.Wavelength}) REMOVED");

            SaveSettings();
        }

        public void RemoveDevice(BioLEDDevice Device)
        {
            SelectedLED = null;
            SelectedDevice = null;

            _ = Devices.Remove(Device);
        }

        #endregion

        #region LED Callbacks

        void LED_ModeChanged(object? sender, Helper.Common.EventArgs<LEDModeEnum> e)
        {
            if (sender == null || e == null)
                return;

            LED led = (LED)sender;
            Log($"LED_ModeChanged called for (new value = {e.Value}) for {led.DeviceDetailString}");

            if (led.Device == null)
            {
                Log("LED_ModeChanged called, but Device is null, returning");
                return;
            }

            if (e.Value == LEDModeEnum.Pulse)
            {
                _ = MessageBox.Show("This Mode is not currently supported");
                led.Mode = LEDModeEnum.Disabled;
            }
            _ = BioLEDInterface.MTUSB_BLSDriverSetMode(led.Device.DeviceHandle, led.DeviceChannelIndex, e.Value.LEDModeToInt());
        }

        void LED_IsOnChanged(object? sender, Helper.Common.EventArgs<bool> e)
        {
            if (sender == null || e == null)
                return;
            LED led = (LED)sender;
            Send_LEDIntensity(led, led.Intensity);      //Send_LEDIntensity will check the ON/OFF status, and send the appropriate Intensity
            Log("LED_IsOnChanged called for {0}", led.DeviceDetailString);
        }

        void LED_IntensityChanged(object? sender, Helper.Common.EventArgs<double> e)
        {
            if (sender == null || e == null)
                return;
            LED led = (LED)sender;

            Send_LEDIntensity(led, e.Value);
        }

        #endregion

        #region Settings

        private void SaveSettings()
        {
            XmlWriter writer = XmlWriter.Create(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + "LED Controller.settings");
            XmlSerializer serializer = new(typeof(Settings));

            Settings s = new()
            {
                Devices = [.. Devices],

                BaudRate = string.IsNullOrWhiteSpace(SelectedBaudRate) ? Settings.BaudRate : SelectedBaudRate,
                COMPort = string.IsNullOrWhiteSpace(SelectedCOMPort) ? Settings.COMPort : SelectedCOMPort,
                IsCompactMode = IsCompactMode,
                IsDarkUIMode = IsDarkUIMode,
                PresetIntensities = [.. PresetIntensities]
            };

            serializer.Serialize(writer, s);

            writer.Close();

            snackbar.MessageQueue?.Enqueue("SAVED");
        }

        private void LoadSettings()
        {
            Log($"LoadSettings() called Filename: {settingsFilename}");

            XmlReader? reader = null;
            try
            {
                reader = XmlReader.Create(settingsFilename);
                XmlSerializer serializer = new(typeof(Settings));

                if (serializer.Deserialize(reader) is Settings settings)
                    Settings = settings;
                else
                {
                    Log($"LoadSettings() FAILED to Deserialize to settings");
                    Settings = new Settings(); //if deserialization fails, create a new Settings object with default values
                }
            }
            catch (Exception e)
            {
                Settings = new Settings();
                Log($"LoadSettings() failed ({e.Message})");
            }
            finally
            {
                reader?.Close();
            }

            snackbar.MessageQueue?.Enqueue(string.Format("OPENED\n\n{0}", settingsFilename));

            COMPorts.Clear();

            BaudRates.Clear();

            BaudRates.Add("2400");
            BaudRates.Add("4800");
            BaudRates.Add("9600");
            BaudRates.Add("14400");
            BaudRates.Add("19200");
            BaudRates.Add("28800");
            BaudRates.Add("38400");
            BaudRates.Add("57600");
            BaudRates.Add("115200");

            if (COMPorts.Contains(Settings.COMPort))
                SelectedCOMPort = Settings.COMPort;

            SelectedBaudRate = Settings.BaudRate;

            Devices.Clear();
            Settings.Devices.ForEach(a => Devices.Add(a));
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
                    _ = BioLEDInterface.MTUSB_BLSDriverSetMode(device.DeviceHandle, led.DeviceChannelIndex, led.Mode.LEDModeToInt());
                }
            }

            if (Settings.PresetIntensities == null)
                Settings.PresetIntensities = [.. Settings.DefaultPresetIntensities];       //default Preset Intensities
            else if (Settings.PresetIntensities.Count == 0)
                Settings.PresetIntensities = [.. Settings.DefaultPresetIntensities];       //default Preset Intensities

            SelectedDevice = Devices.FirstOrNullObject(null);
            IsDarkUIMode = Settings.IsDarkUIMode ?? true;
            IsCompactMode = Settings.IsCompactMode ?? true;
            PresetIntensities = new ObservableCollection<double>(Settings.PresetIntensities);
        }

        private readonly string settingsFilename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + "LED Controller.settings";

        #endregion

        #region Window Open/Close

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();

            base.OnClosing(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }

        #endregion

        #region Private Functions

        private void Connect_Click(object sender, RoutedEventArgs? e)
        {
            //Remove event handlers from OLD devices before calling GetDevices (which clears all devices)
            foreach (BioLEDDevice device in Devices)
            {
                Log($"Removing event handlers from BioLEDDevice #{device.SerialNumber}");

                device.LEDs.AsParallel().ForAll(a => a.IntensityChanged -= LED_IntensityChanged);
                device.LEDs.AsParallel().ForAll(a => a.IsOnChanged -= LED_IsOnChanged);
                device.LEDs.AsParallel().ForAll(a => a.ModeChanged -= LED_ModeChanged);
            }

            GetDevices();

            foreach (BioLEDDevice device in Devices)
            {
                Log($"Adding event handlers for BioLEDDevice #{device.SerialNumber}");

                device.LEDs.AsParallel().ForAll(a => a.Device = device);
                device.LEDs.AsParallel().ForAll(a => a.IntensityChanged += LED_IntensityChanged);
                device.LEDs.AsParallel().ForAll(a => a.IsOnChanged += LED_IsOnChanged);
                device.LEDs.AsParallel().ForAll(a => a.ModeChanged += LED_ModeChanged);

                foreach (LED led in device.LEDs)
                {
                    _ = BioLEDInterface.MTUSB_BLSDriverSetMode(device.DeviceHandle, led.DeviceChannelIndex, led.Mode.LEDModeToInt());
                }
            }
        }

        private async void GetDevices()
        {
            Log("GetDevices() called");

            int devicecount = BioLEDInterface.MTUSB_BLSDriverInitDevices();

            Log($"{devicecount} BioLED devices could be found (BioLEDInterface.MTUSB_BLSDriverInitDevices())");

            if (devicecount == 0)
            {
                var result = await DialogHost.Show(App.Current.Resources["Dialog_NoBioLEDDevices"]);
                HasDevices = false;
                Devices.Clear();
                return;
            }

            for (int i = 0; i < devicecount; i++)
            {
                int result = BioLEDInterface.MTUSB_BLSDriverOpenDevice(i);

                Log($"Device # {i} OPEN called, return = {result}");

                if (result > -1)
                {
                    BioLEDDevice device = new(result);

                    StringBuilder sb = new(256);

                    int ok = BioLEDInterface.MTUSB_BLSDriverGetSerialNo(device.DeviceHandle, sb, 256);

                    device.SerialNumber = sb.ToString();

                    int channelcount = BioLEDInterface.MTUSB_BLSDriverGetChannels(device.DeviceHandle);

                    Log($"MTUSB_BLSDriverGetChannels returned {channelcount}");

                    device.ChannelCount = channelcount;
                    device.IsConnected = true;

                    if (Devices.Any(a => a.SerialNumber == device.SerialNumber))
                    {
                        //Device already present (ie. was loaded from the Settings File), therefore simply update its status to IsConnected
                        Devices.First(a => a.SerialNumber == device.SerialNumber).IsConnected = true;
                        Log($"Device {device.SerialNumber} found");
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

        private readonly Dictionary<string, int> previousvalue = [];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LED"></param>
        /// <param name="Value">As percentage (e.g. 0.5 = 50%)</param>
        private void Send_LEDIntensity(LED LED, double Value)
        {
            if (LED == null)
            {
                Log("Send_LEDIntensity called, but LED is null, returning");
                return;
            }
            if (LED.Device == null)
            {
                Log("Send_LEDIntensity called, but LED.Device is null, returning");
                return;
            }

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
                    _ = DialogHost.Show(Resources["DIALOG_NotImplemented"]);

                if (!sentValues.TryAdd(LED, Value))
                    sentValues[LED] = Value;
            }
        }

        private readonly Dictionary<LED, double> sentValues = [];

        #endregion

        #region Ardunio Connection

        //private async void ConnectMonitor_Click(object sender, RoutedEventArgs e)
        //{

        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(SelectedBaudRate))
        //        {
        //            var result = await DialogHost.Show(App.Current.Resources["Dialog_BaudRateNotSet"]);
        //            return;
        //        }
        //        if (string.IsNullOrWhiteSpace(SelectedCOMPort))
        //        {
        //            var result = await DialogHost.Show(App.Current.Resources["Dialog_COMPortNotSet"]);
        //            return;
        //        }

        //        previousvalue.Clear();

        //        if (port != null && port.IsOpen)
        //        {
        //            port.Close();
        //            port.Dispose();
        //        }

        //        port = new SerialPort(SelectedCOMPort);
        //        port.BaudRate = int.Parse(SelectedBaudRate);
        //        port.DtrEnable = true;
        //        port.ReadTimeout = 5000;
        //        port.WriteTimeout = 500;
        //        port.DataReceived += port_DataReceived;

        //        port.Open();

        //        //send test string
        //        controllerteststring = string.Format("TEST {0}", new Random(2).NextDouble());

        //        try
        //        {
        //            port.WriteLine(controllerteststring);
        //            //controllerpassedtest = true;
        //        }
        //        catch (System.TimeoutException)
        //        {
        //            var result = await DialogHost.Show(App.Current.Resources["Dialog_COMPorttTimeout"]);

        //            port.Close();
        //            port.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var result = await DialogHost.Show(string.Format("Connect Arduino failed\n\n{0} {1}\n\n{2}", ex.HResult, ex.Message, ex.ToString()));
        //    }
        //}

        //private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    //Debug.Print("port_DataReceived {0}", e.EventType);

        //    if (port.IsOpen)
        //    {
        //        string s = port.ReadLine();
        //        if (string.IsNullOrWhiteSpace(s))
        //            return;

        //        //if (s == controllerteststring)
        //        //    controllerpassedtest = true;


        //        string ID = s.Substring(0, 2);

        //        string valuestring = s.Substring(2).Trim();

        //        if (valuestring.StartsWith("B"))
        //        {
        //            Debug.Print("ID: {0}, button state: {1}", ID, valuestring);
        //            LED led = Dispatcher.Invoke(new Func<LED>(delegate
        //            {
        //                LED result = SelectedDevice.LEDs.FirstOrNullObject(a => a.IsLinkedToAnalogueChannel && a.LinkedAnalogueChannelName == ID, null);
        //                return result;
        //            }), System.Windows.Threading.DispatcherPriority.Normal);
        //            if (led == null)
        //                return;
        //            led.IsOn = !led.IsOn;
        //            return;
        //        }

        //        int rawvalue = int.Parse(valuestring);

        //        Debug.Print("ID: {0}, rawvalue: {1}", ID, rawvalue);

        //        if (previousvalue.ContainsKey(ID) == false)
        //        {
        //            //Debug.Print("creating previousvalue entry");
        //            previousvalue.Add(ID, -1);
        //        }

        //        if (IsCloseTo(previousvalue[ID], rawvalue, 5) == false)
        //        {
        //            //Debug.Print("finding LED now");
        //            LED led = Dispatcher.Invoke(new Func<LED>(delegate
        //            {
        //                LED result = SelectedDevice.LEDs.FirstOrNullObject(a => a.IsLinkedToAnalogueChannel && a.LinkedAnalogueChannelName == ID, null);
        //                return result;
        //            }), System.Windows.Threading.DispatcherPriority.Normal);


        //            if (led != null)
        //            {
        //                //Debug.Print("LED found {0}", led.ToString());
        //                double value = MapAnalogueToPercentage(ID, rawvalue);

        //                Debug.Print("valuestring = {0}, rawvalue {1} mapped to {2}", valuestring, rawvalue, value.ToString("0.000"));
        //                previousvalue[ID] = rawvalue;


        //                led.Intensity = value;
        //                //Send_LEDIntensity(led, value);
        //            }

        //        }
        //    }
        //}

        #endregion

        #region Menu items

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ArduinoDialog_Click(object sender, RoutedEventArgs e)
        {
            object o = App.Current.Resources["DIALOG_ArduinoDevice"];
            _ = await DialogHost.Show(o);
        }

        private void CreateDEMODevice_Click(object sender, RoutedEventArgs e)
        {
            Devices.Clear();
            Devices.Add(new BioLEDDevice(100) { ChannelCount = 4, SerialNumber = "DEMO DEVICE", IsConnected = true });

            Devices.Last().LEDs.Add(new LED() { Wavelength = 470, DeviceChannelIndex = 0, Device = Devices.Last() });
            Devices.Last().LEDs.Add(new LED() { Wavelength = 590, DeviceChannelIndex = 1, Device = Devices.Last() });
            SelectedDevice = Devices.Last();
            HasDevices = Devices.Count > 0;
            return;
        }

        private async void About_Click(object sender, RoutedEventArgs e)
        {
            object o = App.Current.Resources["DIALOG_About"];

            AboutViewModel vm = new()
            {
                Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version ??= new Version(0, 0, 0, 1),
                SettingsFilename = settingsFilename
            };
            ((FrameworkElement)o).DataContext = vm;
            _ = await DialogHost.Show(o);
        }

        private async void PresetIntensities_Click(object sender, RoutedEventArgs e)
        {
            object o = App.Current.Resources["DIALOG_PresetIntensities"];
            PresetLEDIntensitiesViewModel vm = new() { PresetLEDIntensities = new ObservableCollection<double>([.. PresetIntensities]) };
            ((FrameworkElement)o).DataContext = vm;

            _ = await DialogHost.Show(o);

            PresetIntensities.Clear();
            PresetIntensities.AddRangeUnique(vm.PresetLEDIntensities);
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            //Debug.Print("CollectionViewSource_Filter");
            if (e.Item is not LED led)
                return;
            e.Accepted = !IsCompactMode || led.IsFavourite;
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
            Logs.Add(string.Format("{0}:\t{1}", Logs.Count, Message));
            Debug.Print("Log: {0}", Message);
        }

        private void Log(string Format, params object[] p)
        {
            Log(string.Format(Format, p));
        }

        #endregion

    }
}
