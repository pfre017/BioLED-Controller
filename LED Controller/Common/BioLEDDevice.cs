using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Common;
using System.Xml.Serialization;

namespace LED_Controller.Common
{
    public class BioLEDDevice : BindableObject
    {
        #region Constructor

        public BioLEDDevice(int DeviceHandle)
        {
            this.DeviceHandle = DeviceHandle;
            LEDs.CollectionChanged += LEDs_CollectionChanged;
        }

        public BioLEDDevice()
        {
            LEDs.CollectionChanged += LEDs_CollectionChanged;
        }

        #endregion

        #region Properties

        private ObservableCollection<LED> LEDs_ = [];
        public ObservableCollection<LED> LEDs
        {
            get
            {
                return LEDs_;
            }
            private set
            {
                LEDs_ = value;
                base.RaisePropertyChanged(nameof(LEDs));
                base.RaisePropertyChanged(nameof(HasLEDs));
            }
        }

        private void LEDs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.RaisePropertyChanged(nameof(HasLEDs));
        }

        public bool HasLEDs
        {
            get
            {
                return LEDs_ != null && LEDs_.Count > 0;
            }
        }

        private int ChannelCount_;
        public int ChannelCount
        {
            get { return ChannelCount_; }
            set
            {
                ChannelCount_ = value;
                base.RaisePropertyChanged(nameof(ChannelCount));
            }
        }

        private string? SerialNumber_;
        public string? SerialNumber
        {
            get { return SerialNumber_; }
            set
            {
                SerialNumber_ = value;
                base.RaisePropertyChanged(nameof(SerialNumber));
            }
        }

        private int DeviceHandle_;
        [XmlIgnore]
        public int DeviceHandle
        {
            get { return DeviceHandle_; }
            private set
            {
                DeviceHandle_ = value;
                base.RaisePropertyChanged(nameof(DeviceHandle));
            }
        }

        private bool IsConneccted_;
        [XmlIgnore]
        public bool IsConnected
        {
            get { return IsConneccted_; }
            set
            {
                IsConneccted_ = value;
                base.RaisePropertyChanged(nameof(IsConnected));
            }
        }



        #endregion

        public override string ToString()
        {
            return $"Device ({SerialNumber}), Handle = {DeviceHandle}, ChannelCount = {ChannelCount}";
        }

    }
}
