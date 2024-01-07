using LifeInDiary.Libs;
using LifeInDiary.Libs.Animation;
using System;
using System.Windows;
using System.Windows.Threading;

namespace LifeInDiary.Main.DiaryWindow
{
    public partial class Diary : Window
    {
        bool _isSnackBarOpen = false;
        private void ShowSnackbar(string Text)
        {
            if (_isSnackBarOpen) { return; }
            try
            {
                _SLS_Text.Text = Text;
                BorderAnimations.OpacityAnimation(SingleLine_Snackbar, 1);
                //BorderAnimations.MarginAnimation(SingleLine_Snackbar, new Thickness(0,0,0,20));
                _isSnackBarOpen = true;
                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromSeconds(3);
                dispatcherTimer.Tick += (s, e) =>
                {
                    BorderAnimations.OpacityAnimation(SingleLine_Snackbar, 0);
                    //BorderAnimations.MarginAnimation(SingleLine_Snackbar, new Thickness(0, 0, 0, -1 * this.ActualHeight), animationSpeed);

                    _isSnackBarOpen = false;
                    dispatcherTimer.Stop();
                };
                dispatcherTimer.Start();
            }
            catch (Exception e) { Log.Error(e); }
        }
        private void HideSnackbar()
        {
            try
            {
                SingleLine_Snackbar.Opacity = 0;
                SingleLine_Snackbar.Visibility = Visibility.Collapsed;
            }
            catch (Exception e) { Log.Error(e); };
        }
        private void ReadjustSnackbar()
        {
            if (!_isSnackBarOpen)
            {
                //BorderAnimations.MarginAnimation(SingleLine_Snackbar, new Thickness(0, 0, 0, -1 * this.ActualHeight));
            }
        }
    }
}
