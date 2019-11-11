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
using System.Windows.Shapes;
using LED_Controller.Common;

namespace LED_Controller
{
    /// <summary>
    /// Interaction logic for LEDDialog.xaml
    /// </summary>
    public partial class LEDDialog : Window
    {
        public LEDDialog()
        {
            InitializeComponent();
        }


        public bool ShowDialog(LED LED)
        {
            this.DataContext = LED;

            return base.ShowDialog().GetValueOrDefault(false);

        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.DialogResult = false;
            this.Close();
        }


        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            base.DialogResult = true;
            this.Close();

        }
    
    }




    
}
