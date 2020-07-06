using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED_Controller.Common
{
    public class PresetLEDIntensitiesViewModel
    {
        public double NewPresetLEDIntensityValue { get; set; } = 0;
        public ObservableCollection<double> PresetLEDIntensities { get; set; }
        public double SelectedLEDItensity { get; set; } = -1;
    }
}
