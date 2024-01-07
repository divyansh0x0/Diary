using LifeInDiary.Libs.Animation;
using LifeInDiary.Libs.UI;
using LifeInDiary.Main.DiaryWindow;
using LifeInDiary.Properties;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LifeInDiary.Main.Configuration
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        private int CAPTION_HEIGHT = 20;
        public ConfigurationWindow()
        {
            InitializeComponent();
            Common.AddChrome(this, CAPTION_HEIGHT, new Thickness(5, 2, 5, 5));
            ValidateCaptionButtons();

            ConfirmationPopupContainer.Visibility = Visibility.Hidden;
            ConfirmationPopupContainer.Opacity = 0;

            ConfiguringAppPopupContainer.Visibility = Visibility.Collapsed;
            ConfiguringAppPopupContainer.Opacity = 0;


            ErrorBox.Visibility = Visibility.Collapsed;
            ErrorBox.Opacity = 0;

        }

        public void ValidateCaptionButtons()
        {
            if (WindowState == WindowState.Maximized)
            {
                CaptionBar.Margin = new Thickness(4);
            }
            else if (WindowState == WindowState.Normal)
            {
                CaptionBar.Margin = new Thickness(0);
            }
        }
        #region Windows's event handlers
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ValidateCaptionButtons();
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper((Window)sender).Handle;
            var value = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (int)(value & ~WS_MAXIMIZEBOX));
        }
        #endregion
        #region Caption button's event
        private void MinimizeBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseBtnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
        #region USER NAME FIELD
        private void UsernameField_GotFocus(object sender, RoutedEventArgs e)
        {
            AnimatedHelpText.BeginAnimation(TextBox.MarginProperty, new ThicknessAnimation(new Thickness(0, 0, 0, 32d), TimeSpan.FromMilliseconds(100)));
            AnimatedHelpTextBackground.BeginAnimation(TextBox.MarginProperty, new ThicknessAnimation(new Thickness(0, 0, 0, 32), TimeSpan.FromMilliseconds(100)));
            AnimatedHelpText.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));

            borderBtm.BeginAnimation(Rectangle.WidthProperty, new DoubleAnimation(400, TimeSpan.FromMilliseconds(100)));
        }

        private void UsernameField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameField.Text.Trim().Length == 0)
            {
                AnimatedHelpText.BeginAnimation(TextBox.MarginProperty, new ThicknessAnimation(new Thickness(0, 0, 0, 10), TimeSpan.FromMilliseconds(100)));
                AnimatedHelpTextBackground.BeginAnimation(TextBox.MarginProperty, new ThicknessAnimation(new Thickness(0, 0, 0, 10), TimeSpan.FromMilliseconds(100)));
                AnimatedHelpText.Foreground = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            }
            borderBtm.BeginAnimation(Rectangle.WidthProperty, new DoubleAnimation(0, TimeSpan.FromMilliseconds(100)));
        }
        #endregion
        #region Form Button events        
        private void SubmitConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 15;
            ConfigurationContainer.Effect = blurEffect;

            //If the field is empty show error 
            if (UsernameField.Text.Trim().Length == 0)
            {
                BorderAnimations.OpacityAnimation(ErrorBox, 1);
                return;
            }

            ConfirmationNameLabel.Text = $"Mr. {UsernameField.Text.Trim()}";
            BorderAnimations.OpacityAnimation(ConfirmationPopupContainer, 1);
        }
        private void ConfirmCompletionBtn_Click(object sender, RoutedEventArgs e)
        {
            BorderAnimations.OpacityAnimation(ConfirmationPopupContainer, 0);
            //Save user name and configure the app
            ConfigureAndStartApplication(UsernameField.Text.Trim());
            //Show configuration running container

            BorderAnimations.OpacityAnimation(ConfiguringAppPopupContainer, 1);
        }
        private void Refill_InformationBtn_Click(object sender, RoutedEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.ShadowDepth = 0;
            ConfigurationContainer.Effect = dropShadowEffect;

            BorderAnimations.OpacityAnimation(ConfirmationPopupContainer, 0);
        }

        /// <summary>
        /// Close error box 
        /// </summary>
        private void CloseErrorBoxButton_Click(object sender, RoutedEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.ShadowDepth = 0;
            ConfigurationContainer.Effect = dropShadowEffect;

            BorderAnimations.OpacityAnimation(ErrorBox, 0);
        }
        #endregion
        private void ConfigureAndStartApplication(string UserName)
        {
            Settings.Default.IsConfigured = true;
            Settings.Default.Username = UserName;
            Settings.Default.Save();

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            int i = 0;
            dispatcherTimer.Tick += (s, e) =>
            {
                i++;
                if (i > 10)
                {
                    new Diary().Show();
                    this.Close();
                    dispatcherTimer.Stop();
                }
            };
            dispatcherTimer.Start();

        }
    }
}
