using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;
namespace WpfApp29
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void VideoClipPlayHandler(object sender, RoutedEventArgs e)
        {
            VideoClip.Play();
            if (timer != null)
                timer.Start();
        }
        private void VideoClipPauseHandler(object sender, RoutedEventArgs e)
        {
            VideoClip.Pause();
            if (timer != null)
                timer.Stop();
        }
        private void VideoClipStopHandler(object sender, RoutedEventArgs e)
        {
            VideoClip.Stop();
            if (timer != null)
                timer.Stop();
        }
        private void VideoClip_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(e.ErrorException.Message);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VideoClip.ScrubbingEnabled = true;
            VideoClip.Stop();
        }

        private void VideoClip_MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                TimerSlider.Maximum = VideoClip.NaturalDuration.TimeSpan.TotalSeconds;

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;

                totalTimeofVideo.Content = VideoClip.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");

                VideoClip.Play();
                VideoClip.Pause();
            }
            catch (Exception exception)
            {
                VideoClip.Stop();
                MessageBox.Show("Invalid extension");
            }

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerSlider.Value = VideoClip.Position.TotalSeconds;
        }
        private void TimerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoClip.Position = TimeSpan.FromSeconds(TimerSlider.Value);
        }
        private void TimerSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            VideoClip.Pause();
            if (timer != null)
                timer.Stop();
        }
        private void TimerSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            VideoClip.Play();
            timer.Start();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                pathUrl.Text = System.IO.Path.GetFullPath(openFileDialog.FileName);
        }
    }
}
