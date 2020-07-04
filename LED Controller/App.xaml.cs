using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LED_Controller.Common;


namespace LED_Controller
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            base.OnStartup(e);


        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception.Source == "MaterialDesignThemes.WPF")
                return;

            Helper.Controls.Dialogs.ExceptionViewer ev = new Helper.Controls.Dialogs.ExceptionViewer("An unexpected error occured in the application", e.Exception, this.MainWindow);
            ev.ShowDialog();
            e.Handled = true;
        }

        private void PresetIntensity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                LED led = (LED)Helper.Utilities.FindParent<ListBoxItem>(b).Content;
                led.Intensity = double.Parse((string)b.Tag);
            }
            catch (Exception ex)
            {
                Helper.Controls.Dialogs.ExceptionViewer ev = new Helper.Controls.Dialogs.ExceptionViewer("An unexpected error occured in PresetIntensity_Click", ex, this.MainWindow);
            }
        }




    }


    


}
