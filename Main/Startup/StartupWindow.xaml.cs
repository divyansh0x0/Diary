using LifeInDiary.Libs;
using LifeInDiary.Libs.UI;
using LifeInDiary.Main.Configuration;
using LifeInDiary.Main.DiaryWindow;
using LifeInDiary.Properties;
using System;
using System.Windows;
using System.Windows.Threading;

namespace LifeInDiary.Main.Startup
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
            Common.AddChrome(this, this.Height, new Thickness(0));
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            StartValidation();
        }
        public void StartValidation()
        {
            int i = 0;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            dispatcherTimer.Tick += (s, e) =>
            {
                i++;
                if (i > 50)
                {
                    if (isUserConfigurationComplete())
                    {
                        new Diary().Show();
                        this.Close();
                    }
                    else
                    {
                        new ConfigurationWindow().Show();
                        this.Close();
                    }
                    dispatcherTimer.Stop();
                }
            };
            dispatcherTimer.Start();

        }

        private bool isUserConfigurationComplete()
        {
            Log.Info("IS APPLICATION CONFIGURED : " + Settings.Default.IsConfigured);
            Log.Info("USERNAME : " + Settings.Default.Username);

            if (Settings.Default.IsConfigured)
            {
                return true;
            }
            return false;
        }
    }
}
