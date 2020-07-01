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
            Devices = new List<BioLEDDevice>();
        }

        public List<BioLEDDevice> Devices { get; set; }
        public string BaudRate { get; set; }
        public string COMPort { get; set; }
        public bool IsCompactMode { get; set; }
        public bool IsDarkUIMode { get; set; }

        public List<double> PresetIntensities { get; set; }
    }
}
