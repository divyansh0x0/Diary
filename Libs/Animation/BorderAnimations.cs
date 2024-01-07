using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace LifeInDiary.Libs.Animation
{
    public static class BorderAnimations
    {
        #region Opacity Animation
        /// <summary>
        /// Applies an opacity animation with a duration of 100ms
        /// </summary>
        /// <param name="border">Border to which the animation will be applied</param>
        /// <param name="toValue">To value of the opacity</param>
        public static void OpacityAnimation(Border border, double toValue)
        {
            if (toValue > 1) { toValue = 1; }
            border.Visibility = Visibility.Visible;
            DoubleAnimation doubleAnimation = new DoubleAnimation(toValue, TimeSpan.FromMilliseconds(100));
            doubleAnimation.Completed += (s, e) =>
            {
                if (toValue == 0)
                {
                    border.Visibility = Visibility.Collapsed;
                }
            };
            border.BeginAnimation(Border.OpacityProperty, doubleAnimation);
        }
        /// <summary>
        /// Applies an opacity animation with a duration of 100ms
        /// </summary>
        /// <param name="border">Border to which the animation will be applied</param>
        /// <param name="toValue">To value of the opacity</param>
        /// <param name="Duration">Duration of transition</param>
        public static void OpacityAnimation(Border border, double toValue, TimeSpan Duration)
        {
            if (toValue > 1) { toValue = 0; }
            border.Visibility = Visibility.Visible;
            DoubleAnimation doubleAnimation = new DoubleAnimation(toValue, Duration);
            doubleAnimation.Completed += (s, e) =>
            {
                if (toValue == 0)
                {
                    border.Visibility = Visibility.Collapsed;
                }
            };
            border.BeginAnimation(Border.OpacityProperty, doubleAnimation);
        }

        public static void TempVisibilityAnimation(Border border, TimeSpan Duration, TimeSpan ReverseDelay)
        {
            border.Visibility = Visibility.Visible;
            border.Opacity = 0;
            DoubleAnimation doubleAnimation = new DoubleAnimation(1, Duration);
            doubleAnimation.Completed += (s, e) =>
            {
                int i = 0;
                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = ReverseDelay;
                dispatcherTimer.Tick += (s0, e0) =>
                {

                    DoubleAnimation ReverseAnimation = new DoubleAnimation(0, Duration);
                    ReverseAnimation.Completed += (s1, e1) =>
                    {
                        border.Visibility = Visibility.Collapsed;
                    };
                    border.BeginAnimation(Border.OpacityProperty, ReverseAnimation);
                    dispatcherTimer.Stop();

                };
                dispatcherTimer.Start();
            };
            border.BeginAnimation(Border.OpacityProperty, doubleAnimation);
        }
        #endregion
        #region Width Animation
        /// <summary>
        /// Smooth width tranisition for border element
        /// </summary>
        /// <param name="border">The border to which this animation will be applied</param>
        /// <param name="toWidth">The final Width</param>
        public static void WidthAnimation(Border border, double toWidth)
        {
            border.BeginAnimation(Border.WidthProperty, new DoubleAnimation(toWidth, TimeSpan.FromMilliseconds(100)));
        }
        /// <summary>
        /// Smooth width tranisition for border element
        /// </summary>
        /// <param name="border">The border to which this animation will be applied</param>
        /// <param name="toWidth">The final Width</param>
        /// <param name="Duration">Duration of the animation</param>
        public static void WidthAnimation(Border border, double toWidth, TimeSpan Duration)
        {
            border.BeginAnimation(Border.WidthProperty, new DoubleAnimation(toWidth, Duration));
        }
        #endregion
        #region Height animations
        /// <summary>
        /// Smooth Height Transition For Border
        /// </summary>
        /// <param name="border">Border to which the transition should be applied</param>
        /// <param name="toHeight">The final height</param>
        public static void HeightAnimation(Border border, double toHeight)
        {
            border.BeginAnimation(Border.HeightProperty, new DoubleAnimation(toHeight, TimeSpan.FromMilliseconds(100)));
        }
        /// <summary>
        /// Smooth Height Transition For Border
        /// </summary>
        /// <param name="border">Border to which the transition should be applied</param>
        /// <param name="toHeight">The final height</param>
        /// <param name="duration">The duration of the transition</param>
        public static void HeightAnimation(Border border, double toHeight, TimeSpan duration)
        {
            border.BeginAnimation(Border.HeightProperty, new DoubleAnimation(toHeight, duration));
        }
        #endregion
        #region Margin Animations
        /// <summary>
        /// Smooth margin transition for border element
        /// </summary>
        /// <param name="border">Border to which this transition will be applied</param>
        /// <param name="ToMargin">Final margin</param>
        public static void MarginAnimation(Border border, Thickness ToMargin)
        {
            border.BeginAnimation(Border.MarginProperty, new ThicknessAnimation(ToMargin, TimeSpan.FromMilliseconds(100)));
        }
        /// <summary>
        /// Smooth margin transition for border element
        /// </summary>
        /// <param name="border">Border to which this transition will be applied</param>
        /// <param name="ToMargin">Final margin</param>
        /// <param name="Duration">Duration of transition</param>
        public static void MarginAnimation(Border border, Thickness ToMargin, TimeSpan Duration)
        {
            border.BeginAnimation(Border.MarginProperty, new ThicknessAnimation(ToMargin, Duration));
        }
        #endregion
    }
}
