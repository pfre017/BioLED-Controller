using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Common;
using System.Windows.Media;
using System.Xml.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LED_Controller.Common
{
    public class LED : BindableObject
    {
        #region Constructor

        public LED()
        {
            Fill = new SolidColorBrush(LEDControllerHelper.WavelengthToColor(Wavelength_));
        }

        #endregion

        #region AnalogueChannel Linking
        public bool IsLinkedToAnalogueChannel
        {
            get { return IsLinkedToAnalogueChannel_; }
            set
            {
                IsLinkedToAnalogueChannel_ = value;
                base.RaisePropertyChanged(nameof(IsLinkedToAnalogueChannel));
            }
        }
        private bool IsLinkedToAnalogueChannel_ = false;

        public string? LinkedAnalogueChannelName
        {
            get { return LinkedAnalogueChannelName_; }
            set
            {
                LinkedAnalogueChannelName_ = value;
                base.RaisePropertyChanged(nameof(LinkedAnalogueChannelName));
            }
        }
        private string? LinkedAnalogueChannelName_;

        #endregion

        public double Intensity
        {
            get { return Intensity_; }
            set
            {
                if (value > 100)
                {

                }
                Intensity_ = value;
                base.RaisePropertyChanged(nameof(Intensity));
                OnIntensityChanged(Intensity_);
            }
        }
        private double Intensity_;

        /// <summary>
        /// Only applies to 'Follower' Mode
        /// </summary>
        public double OffIntensity
        {
            get { return OffIntensity_; }
            set
            {
                OffIntensity_ = value;
                base.RaisePropertyChanged(nameof(OffIntensity));
            }
        }
        private double OffIntensity_ = 0;

        public bool IsOn
        {
            get { return IsOn_; }
            set
            {
                IsOn_ = value;
                base.RaisePropertyChanged(nameof(IsOn));
                OnIsOnChanged(IsOn_);
            }
        }
        private bool IsOn_;

        public bool IsFavourite
        {
            get { return IsFavourite_; }
            set { IsFavourite_ = value;
                base.RaisePropertyChanged(nameof(IsFavourite));
            }
        }
        private bool IsFavourite_;

        public LEDModeEnum Mode
        {
            get { return Mode_; }
            set
            {
                //save Intensity for previous Mode
                if (intensities_.ContainsKey(Mode_))
                    intensities_[Mode_] = Intensity;
                else
                    intensities_.Add(Mode_, Intensity);

                Mode_ = value;
                base.RaisePropertyChanged(nameof(Mode));
                OnModeChanged(Mode_);
            }
        }
        private LEDModeEnum Mode_ = LEDModeEnum.Disabled;

        public double MaxCurrent
        {
            get { return MaxCurrent_; }
            set
            {
                MaxCurrent_ = value;
                base.RaisePropertyChanged(nameof(MaxCurrent));
            }
        }
        private double MaxCurrent_ = 1.5;

        public int Wavelength
        {
            get { return Wavelength_; }
            set
            {
                Wavelength_ = value;
                base.RaisePropertyChanged(nameof(Wavelength));
                Fill = new SolidColorBrush(LEDControllerHelper.WavelengthToColor(Wavelength_));
            }
        }
        private int Wavelength_ = 470;

        #region Device

        private int DeviceChannelIndex_;
        public int DeviceChannelIndex
        {
            get { return DeviceChannelIndex_; }
            set
            {
                DeviceChannelIndex_ = value;
                base.RaisePropertyChanged(nameof(DeviceChannelIndex));
            }
        }

        private BioLEDDevice? Device_;
        [XmlIgnore]
        public BioLEDDevice? Device
        {
            get { return Device_; }
            set
            {
                if (Device_ != null)
                    Device_.PropertyChanged -= Device__PropertyChanged;
                Device_ = value;
                Device_?.PropertyChanged += Device__PropertyChanged;
                base.RaisePropertyChanged(nameof(Device));
                base.RaisePropertyChanged(nameof(IsConnected));
            }
        }

        private void Device__PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsConnected")
                base.RaisePropertyChanged(nameof(IsConnected));
        }


        #endregion

        [XmlIgnore]
        public bool IsConnected
        {
            get {
                if (Device_ == null)
                    return false;
                return Device_.IsConnected;
            }
        }

        public string DeviceDetailString
        {
            get
            {
                return $"Device #{Device?.SerialNumber}  {Wavelength}nm  [ChannelIndex #{DeviceChannelIndex}  {(IsConnected ? " CONNECTED" : " DISCONNECTED")}] Mode: {Mode.ToString().ToUpper()} Intensity: {Intensity}%";
            }
        }

        [XmlIgnore]
        public Brush? Fill
        {
            get
            {
                return Fill_;
            }
            private set
            {
                Fill_ = value;
                base.RaisePropertyChanged(nameof(Fill));
            }
        }
        private Brush? Fill_;

        public override string ToString()
        {
            return $"LED {Wavelength}nm  {(IsOn ? "ON" : "OFF")}  [ChannelIndex #{DeviceChannelIndex}  {(IsConnected ? "CONNECTED" : "DISCONNECTED")}]";
        }

        #region Events

        private void OnModeChanged(LEDModeEnum Mode)
        {
            if (intensities_.TryGetValue(Mode, out double value))
                Intensity = value;

            ModeChanged?.Invoke(this, new EventArgs<LEDModeEnum>(Mode));
        }

        private void OnIntensityChanged(double Intensity)
        {
            IntensityChanged?.Invoke(this, new EventArgs<double>(Intensity));

            if (!intensities_.TryAdd(Mode, Intensity))
                intensities_[Mode] = Intensity;
        }

        private void OnIsOnChanged(bool IsOn)
        {
            IsOnChanged?.Invoke(this, new EventArgs<bool>(IsOn));
        }

        public event EventHandler<EventArgs<bool>>? IsOnChanged;
        public event EventHandler<EventArgs<double>>? IntensityChanged;
        public event EventHandler<EventArgs<LEDModeEnum>>? ModeChanged;

        #endregion

        #region Intensity Cache

        private readonly Dictionary<LEDModeEnum, double> intensities_ = [];

        [XmlElement("CachedIntensities"), Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LEDModeIntensity[] Intensities
        {
            get
            {
                return [.. intensities_.Select(a => new LEDModeIntensity() { Mode = a.Key, Intensity = a.Value })];
            }
            set
            {
                if (value == null)
                    return;

                foreach (LEDModeIntensity i in value)
                {
                    if (intensities_.ContainsKey(i.Mode) == false)
                        intensities_.Add(i.Mode, i.Intensity);
                }
                base.RaisePropertyChanged(nameof(Intensities));
            }
        }


        public class LEDModeIntensity
        {
            public LEDModeEnum Mode { get; set; }
            public double Intensity { get; set; }
        }

        #endregion
    }
}
