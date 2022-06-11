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

namespace UnoAppBackgroundBugWPF.WPF.Host
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Background = new SolidColorBrush(Colors.Red);

            // no effect but leaving it here anyways
            root.Background = new SolidColorBrush(Colors.Green);

            root.Content = new global::Uno.UI.Skia.Platform.WpfHost(Dispatcher, () => new UnoAppBackgroundBugWPF.App());

            // uncomment this to see WpfHost control's background render over shared apps content
            //(root.Content as Uno.UI.Skia.Platform.WpfHost).Background = new SolidColorBrush(Colors.HotPink);

            // This is required because child elements are not available right away
            Forget(DelayFindChildsAsync());
        }

        private async Task DelayFindChildsAsync()
        {
            await Task.Delay(100);

            var border = VisualTreeHelper.GetChild(root.Content as Uno.UI.Skia.Platform.WpfHost, 0) as Border;
            var canvas = VisualTreeHelper.GetChild(border, 0) as Canvas;

            // uncomment this to see border control's background render over shared apps content
            //border.Background = new SolidColorBrush(Colors.Magenta);

            // uncomment this to see border control's background render over shared apps content
            //canvas.Background = new SolidColorBrush(Colors.Cyan);
        }

        private async void Forget(Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
