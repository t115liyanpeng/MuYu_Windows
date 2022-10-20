using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MuYu.Conf;
using NAudio.CoreAudioApi;
using NAudio.FileFormats.Mp3;
using NAudio.Wave;

namespace MuYu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DoubleAnimation movey = new DoubleAnimation(10, -100, TimeSpan.FromSeconds(0.5));
        private DoubleAnimation movex = new DoubleAnimation(0, -10, TimeSpan.FromSeconds(0.5));
        private DoubleAnimation fdx = new DoubleAnimation(1, 2, TimeSpan.FromSeconds(0.5));
        private DoubleAnimation fdy = new DoubleAnimation(1, 2, TimeSpan.FromSeconds(0.5));
        private DoubleAnimation hid = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));
        private string display = "功德+1";
        private SolidColorBrush brush = new SolidColorBrush(Colors.Black);
        private Stream? voicesStream=null;
        public MainWindow()
        {
            InitializeComponent();
            Inited();
        }

        private void Inited()
        {
            Uri mp3 = new Uri("pack://application:,,,/MuYu;component/Resources/my.mp3", UriKind.Absolute);
            voicesStream = Application.GetResourceStream(mp3)!.Stream;

        }


        #region Event
        private void Muyu_OnClick(object sender, RoutedEventArgs e)
        {
            TransformGroup g = new TransformGroup();
            g.Children.Add(new ScaleTransform());
            g.Children.Add(new TranslateTransform());

            TextBlock rect = new TextBlock()
            {
                Text = display,
                RenderTransform = g,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                //Margin = new Thickness(0,-200,0,0)
                Foreground = brush
            };
            disgrid.Children.Add(rect);
           
            Storyboard.SetTargetProperty(movey, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)"));
            Storyboard.SetTarget(movey, rect);
            
            Storyboard.SetTargetProperty(movex, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)"));
            Storyboard.SetTarget(movex, rect);
            
            Storyboard.SetTargetProperty(fdx, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
            Storyboard.SetTarget(fdx, rect);
            
            Storyboard.SetTargetProperty(fdy, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
            Storyboard.SetTarget(fdy, rect);
           
            hid.BeginTime = TimeSpan.FromSeconds(0.3);
            Storyboard.SetTargetProperty(hid, new PropertyPath("Opacity"));
            Storyboard.SetTarget(hid, rect);
            Storyboard sb = new Storyboard()
            {
                FillBehavior = FillBehavior.Stop,
                AutoReverse = false
            };
            sb.Children.Add(movey);
            sb.Children.Add(movex);
            sb.Children.Add(fdx);
            sb.Children.Add(fdy);
            sb.Children.Add(hid);
            sb.Completed += ((o, args) =>
            {
                disgrid.Children.Remove(rect);
            });

            //var reader = new MediaFoundationReader("Resources/my.mp3");
            
            var reader = new StreamMediaFoundationReader(voicesStream);
            var wasapiOut = new WasapiOut();
            wasapiOut.Init(reader);
            wasapiOut.PlaybackStopped += (o, args) =>
            {
                wasapiOut.Dispose();
                reader.Dispose();
            };
            sb.Begin();
            try
            {
                wasapiOut.Play();
            }
            catch
            {

            }

        }
        
        private void Disgrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Max_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            
                this.WindowState = WindowState.Minimized;
            
        }
        private void Set_Click(object sender, RoutedEventArgs e)
        {

           SettingWin sw=new SettingWin(){Owner = this};
           sw.Top=this.Top+100;
           if (sw.ShowDialog() == true)
           {
               display = sw.DisPlay;
               brush = new SolidColorBrush(sw.DisColor);
           }

        }

        #endregion
    }
}
