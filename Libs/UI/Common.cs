using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shell;
namespace LifeInDiary.Libs.UI
{
    static class Common
    {
        #region ADD CHROME
        public static void AddChrome(Window window, double CaptionHeight)
        {
            WindowChrome windowChrome = new WindowChrome();
            windowChrome.CaptionHeight = CaptionHeight;
            windowChrome.GlassFrameThickness = new Thickness(1);
            windowChrome.ResizeBorderThickness = new Thickness(5);
            windowChrome.UseAeroCaptionButtons = false;

            WindowChrome.SetWindowChrome(window, windowChrome);
        }
        public static void AddChrome(Window window, double CaptionHeight, Thickness ResizeBorderThickness)
        {
            WindowChrome windowChrome = new WindowChrome();
            windowChrome.CaptionHeight = CaptionHeight - ResizeBorderThickness.Top;
            windowChrome.GlassFrameThickness = new Thickness(0, 0, 0, 0.4);
            windowChrome.ResizeBorderThickness = ResizeBorderThickness;
            windowChrome.UseAeroCaptionButtons = false;

            WindowChrome.SetWindowChrome(window, windowChrome);
        }
        #endregion
        #region Progress Bar Animation
        /// <summary>
        /// Sets the progress bar value with ANIMATION
        /// </summary>
        /// <param name="progressBar">Progress bar to which the animation will be applied</param>
        /// <param name="value">The set value</param>
        public static void SetProgressBarValue(ProgressBar progressBar, double value)
        {
            if (value > 100) { value = 0; }

            DoubleAnimation doubleAnimation = new DoubleAnimation(value, TimeSpan.FromMilliseconds(100));
            progressBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);
        }
        /// <summary>
        /// Sets the progress bar value with ANIMATION
        /// </summary>
        /// <param name="progressBar">Progress bar to which the animation will be applied</param>
        /// <param name="value">The set value</param>
        /// <param name="Duration">Duration of animation</param>
        public static void SetProgressBarValue(ProgressBar progressBar, double value, TimeSpan Duration)
        {
            if (value > 100) { value = 0; }

            DoubleAnimation doubleAnimation = new DoubleAnimation(value, Duration);
            progressBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);
        }
        #endregion
        #region Add fonts to combo box
        public static void AddFontsToCB(ComboBox comboBox, string DefaultFont)
        {
            new Task(() =>
            {
                foreach (FontFamily font in Fonts.SystemFontFamilies)
                {
                    comboBox.Dispatcher.BeginInvoke((ThreadStart)delegate
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        comboBoxItem.Content = font.Source;
                        comboBoxItem.Padding = new Thickness(5);
                        comboBoxItem.ToolTip = font.Source;
                        comboBoxItem.FontFamily = font;

                        comboBox.Items.Add(comboBoxItem);
                        if (font.Source == DefaultFont)
                        {
                            comboBox.SelectedItem = comboBoxItem;
                            comboBox.ToolTip = comboBoxItem.Content.ToString();
                        }
                        comboBox.RequestBringIntoView += (s, e) =>
                        {
                            e.Handled = true;
                        };
                    });
                    Thread.Sleep(50);
                }
            }).Start();
        }
        #endregion
        #region Add font size to combo box
        public static void AddFontSizeToCB(ComboBox comboBox)
        {
            new Task(() =>
            {
                int[] fontSizeArr = { 6, 7, 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 48, 72 };
                foreach (int fontSize in fontSizeArr)
                {
                    comboBox.Dispatcher.BeginInvoke((ThreadStart)delegate
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        comboBoxItem.Content = fontSize;
                        comboBoxItem.Padding = new Thickness(5);
                        comboBox.Items.Add(comboBoxItem);
                        if (fontSize == 14)
                        {
                            comboBox.SelectedItem = comboBoxItem;
                        }
                        comboBox.RequestBringIntoView += (s, e) =>
                        {
                            e.Handled = true;
                        };
                    });
                    Thread.Sleep(10);
                }

            }).Start();
        }
        #endregion
    }

}
