using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using System.Windows.Data;

namespace LED_Controller.Common
{
    public class LEDControllerCommands : UIElement
    {
        static LEDControllerCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(LEDControllerCommands), new CommandBinding(AddPresetLEDIntensityCommand, OnAddPresetLEDIntensity, OnQueryAddPresetLEDIntensity));
            CommandManager.RegisterClassCommandBinding(typeof(LEDControllerCommands), new CommandBinding(RemovePresetLEDIntensityCommand, OnRemovePresetLEDIntensity, OnQueryRemovePresetLEDIntensity));
        }

        public static RoutedCommand AddPresetLEDIntensityCommand { get; } = new RoutedCommand("AddPresetLEDIntensityCommand", typeof(LEDControllerCommands));
        public static RoutedCommand RemovePresetLEDIntensityCommand { get; } = new RoutedCommand("RemovePresetLEDIntensityCommand", typeof(LEDControllerCommands));

        private static void OnAddPresetLEDIntensity(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                PresetLEDIntensitiesViewModel vm = (PresetLEDIntensitiesViewModel)e.Parameter;

                if (vm.PresetLEDIntensities.Contains(vm.NewPresetLEDIntensityValue) == false)
                    vm.PresetLEDIntensities.Add(vm.NewPresetLEDIntensityValue);
                else
                    Debug.Print("PresetLEDIntensity {0} already present in list (not added)", vm.NewPresetLEDIntensityValue);

            }
            catch (Exception ex)
            {
                Debug.Print("Error ADDING PresetLEDIntensity\n\t{0}", ex.Message);
            }
        }

        private static void OnQueryAddPresetLEDIntensity(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void OnRemovePresetLEDIntensity(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                PresetLEDIntensitiesViewModel vm = (PresetLEDIntensitiesViewModel)e.Parameter;

                if (vm.PresetLEDIntensities.Contains(vm.SelectedLEDItensity))
                {
                    vm.PresetLEDIntensities.Remove(vm.SelectedLEDItensity);
                }
                vm.SelectedLEDItensity = -1;
            }
            catch (Exception ex)
            {
                Debug.Print("Error REMOVING PresetLEDIntensity\n\t{0}", ex.Message);
            }
        }

        private static void OnQueryRemovePresetLEDIntensity(object sender, CanExecuteRoutedEventArgs e)
        {
            PresetLEDIntensitiesViewModel vm = (PresetLEDIntensitiesViewModel)e.Parameter;
            if (vm == null)
            {
                e.CanExecute = false;
                return;
            }
            if (vm.SelectedLEDItensity != -1)
                e.CanExecute = true;        //only if there is a PresetIntensity selected
            else
                e.CanExecute = false;
        }
    }
}
