using LifeInDiary.Libs;
using LifeInDiary.Libs.Animation;
using LifeInDiary.Libs.Components;
using LifeInDiary.Libs.UI;
using LifeInDiary.Properties;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace LifeInDiary.Main.DiaryWindow
{
    public partial class Diary : Window
    {
        private void ShowVisualizer()
        {
            DPC_Border.IsEnabled = true;

            GridAnimations.OpacityAnimation(PV_MaskingGrid, 1, AnimationSpeed);
            PageVisualizer.Visibility = Visibility.Visible;
            PageVisualizer.BeginAnimation(DiaryPageVisualizer.OpacityProperty, new DoubleAnimation(1, AnimationSpeed));

            PageVisualizer.CloseButtonClicked += (s, e) =>
            {
                HideVisualizer();
            };
        }

        private void ReadPage(string Heading, string DateTime, string content)
        {
            PageVisualizer.PageHeading = Heading;
            PageVisualizer.PageDateTime = DateTime;
            PageVisualizer.LoadContentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        private void HideVisualizer()
        {
            DPC_Border.IsEnabled = true;

            DoubleAnimation doubleAnimation = new DoubleAnimation(0, AnimationSpeed);
            doubleAnimation.Completed += (s, e) => { PageVisualizer.Visibility = Visibility.Collapsed; };

            GridAnimations.OpacityAnimation(PV_MaskingGrid, 0, AnimationSpeed);
            PageVisualizer.BeginAnimation(DiaryPageVisualizer.OpacityProperty, doubleAnimation);
        }
    }
}
