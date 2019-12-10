using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Pranas;

namespace SpyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowState prevState;
        private DispatcherTimer timer = new DispatcherTimer();
        private Random random = new Random();
        private int period;
        public MainWindow()
        {
            timer.Tick += ScreenShot;
            InitializeComponent();
            
            
        }
        private void TaskbarIconTrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = prevState;
            taskBar.Visibility = Visibility.Hidden;
        }
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (WindowState == WindowState.Normal)
            {
                Hide();
                taskBar.Visibility = Visibility.Visible;
            }
            else
                prevState = WindowState;
        }
        private void SaveScreen(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(periodTB.Text, out period))
            {
                MessageBox.Show("Некорректные данные!");
                return;
            }
            timer.Start();
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();
            else
                prevState = WindowState;
        }
       

       
        private void ScreenShot(object sender, object e)
        {
            var randomTime = random.Next(period * 60) + 1;
            timer.Interval = new TimeSpan(0, 0, 0, randomTime, 0);
            System.Drawing.Image screen = Pranas.ScreenshotCapture.TakeScreenshot(true);
            var path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Pictures\ScreenApp";
            Directory.CreateDirectory(path);
            path += @$"\{DateTime.Now.ToString("ddMMyyyy - hhmmss")}.png";
            screen.Save(path);
        }

    }

}

