using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LifeInDiary.Libs.Animation
{
    class GridAnimations
    {
        #region Opacity Animation
        /// <summary>
        /// Applies an opacity animation with a duration of 100ms
        /// </summary>
        /// <param name="grid">Grid to which the animation will be applied</param>
        /// <param name="toValue">To value of the opacity between 0 and 1</param>
        public static void OpacityAnimation(Grid grid, double toValue)
        {
            if (toValue > 1) { toValue = 0; }
            grid.Visibility = Visibility.Visible;
            DoubleAnimation doubleAnimation = new DoubleAnimation(toValue, TimeSpan.FromMilliseconds(100));
            doubleAnimation.Completed += (s, e) =>
            {
                if (toValue == 0)
                {
                    grid.Visibility = Visibility.Collapsed;
                }
            };
            grid.BeginAnimation(Grid.OpacityProperty, doubleAnimation);
        }
        /// <summary>
        /// Applies an opacity animation with a duration of 100ms
        /// </summary>
        /// <param name="grid">Grid to which the animation will be applied</param>
        /// <param name="toValue">To value of the opacity between 0 and 1</param>
        /// <param name="Duration">Duration of transition</param>
        public static void OpacityAnimation(Grid grid, double toValue, TimeSpan Duration)
        {
            if (toValue > 1) { toValue = 0; }
            grid.Visibility = Visibility.Visible;
            DoubleAnimation doubleAnimation = new DoubleAnimation(toValue, Duration);
            doubleAnimation.Completed += (s, e) =>
            {
                if (toValue == 0)
                {
                    grid.Visibility = Visibility.Collapsed;
                }
            };
            grid.BeginAnimation(Grid.OpacityProperty, doubleAnimation);
        }
        #endregion
        #region Width animations
        /// <summary>
        /// Smooth Height Transition For Grid
        /// </summary>
        /// <param name="grid">Grid to which the transition should be applied</param>
        /// <param name="toWidth">The final height</param>
        public static void WidthAnimation(Grid grid, double toWidth)
        {
            grid.BeginAnimation(Grid.HeightProperty, new DoubleAnimation(toWidth, TimeSpan.FromMilliseconds(100)));
        }
        /// <summary>
        /// Smooth Height Transition For Grid
        /// </summary>
        /// <param name="grid">Grid to which the transition should be applied</param>
        /// <param name="toWidth">The final height</param>
        /// <param name="duration">The duration of the transition</param>
        public static void WidthAnimation(Grid grid, double toWidth, TimeSpan duration)
        {
            grid.BeginAnimation(Grid.HeightProperty, new DoubleAnimation(toWidth, duration));
        }
        #endregion
        #region Height animations
        /// <summary>
        /// Smooth Height Transition For Grid
        /// </summary>
        /// <param name="grid">Grid to which the transition should be applied</param>
        /// <param name="toHeight">The final height</param>
        public static void HeightAnimation(Grid grid, double toHeight)
        {
            grid.BeginAnimation(Grid.HeightProperty, new DoubleAnimation(toHeight, TimeSpan.FromMilliseconds(100)));
        }
        /// <summary>
        /// Smooth Height Transition For Grid
        /// </summary>
        /// <param name="grid">Grid to which the transition should be applied</param>
        /// <param name="toHeight">The final height</param>
        /// <param name="duration">The duration of the transition</param>
        public static void HeightAnimation(Grid grid, double toHeight, TimeSpan duration)
        {
            grid.BeginAnimation(Grid.HeightProperty, new DoubleAnimation(toHeight, duration));
        }
        #endregion
        #region Margin Animation
        /// <summary>
        /// Smooth margin transition to a given thickness
        /// </summary>
        /// <param name="grid">The Grid to which the animation should be applied</param>
        /// <param name="ToMargin">The final margin</param>
        public static void MarginAnimation(Grid grid, Thickness ToMargin)
        {
            grid.BeginAnimation(Grid.MarginProperty, new ThicknessAnimation(ToMargin, TimeSpan.FromMilliseconds(100)));
        }
        /// <summary>
        /// Smooth margin transition to a given thickness
        /// </summary>
        /// <param name="grid">The Grid to which the animation should be applied</param>
        /// <param name="ToMargin">The final margin</param>
        /// <param name="Duration">The total duration of the transition</param>
        public static void MarginAnimation(Grid grid, Thickness ToMargin, TimeSpan Duration)
        {
            grid.BeginAnimation(Grid.MarginProperty, new ThicknessAnimation(ToMargin, Duration));
        }
        #endregion
    }
}
