using System;
using System.Windows;

namespace ScreenToGif.Windows
{
    /// <summary>
    /// Interaction logic for MiniPath.xaml
    /// </summary>
    public partial class MiniPath : Window
    {
        public MiniPath()
        {
            InitializeComponent();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ExamplePath.Data = Geometry.Parse(InputTextBox.Text);
            }
            catch (Exception ex)
            {
                //LogWriter.Log(ex, "Geometry Parse error", InputTextBox.Text);
            }
        }
    }
}
