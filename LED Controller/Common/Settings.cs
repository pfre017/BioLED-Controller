using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LED_Controller.Common;

namespace LED_Controller.Common
{
    public class Settings
    {
        public Settings()
        {
            Devices = [];
            PresetIntensities = [];
        }

        public List<BioLEDDevice> Devices { get; set; }
        public string BaudRate { get; set; } = "115200";
        public string COMPort { get; set; } = "COM3";
        public bool? IsCompactMode { get; set; }
        public bool? IsDarkUIMode { get; set; }

        public List<double> PresetIntensities { get; set; }

        public IEnumerable<double> DefaultPresetIntensities
        {
            get
            {
                return DefaultPresetIntensities_;
            }
        }
        private readonly List<double> DefaultPresetIntensities_ = [1, 2, 5, 10, 20, 50, 100];
    }
}
