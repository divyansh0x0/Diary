using LifeInDiary.Libs.Animation;
using System.Windows;
using System.Windows.Media.Effects;

namespace LifeInDiary.Main.DiaryWindow
{
    public partial class Diary : Window
    {
        private const int MinSideGridWidth = 200;
        private const int MaxSideGridWidth = 300;

        #region  MOUSE EVENT HANDLING
        private void SidebarOpenCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isSideBarOpen) { closeSidebar(); }
            else { openSidebar(); }
        }
        void openSidebar()
        {
            if (!isSideBarOpen)
            {
                SidebarBorder.IsEnabled = true;
                //HomeGrid.Effect = new BlurEffect();
                HomeGrid.IsEnabled = false;

                //Mask the content
                GridAnimations.OpacityAnimation(MaskingGrid, 1);

                //BorderAnimations.WidthAnimation(SidebarBorder, MaxSideGridWidth, animationSpeed);
                BorderAnimations.MarginAnimation(SidebarBorder, new Thickness(0, 0, 0, 0), AnimationSpeed);
                DropShadowEffect dropShadowEffect = new DropShadowEffect();
                dropShadowEffect.BlurRadius = 20;
                SidebarBorder.Effect = dropShadowEffect;
                isSideBarOpen = true;

                ReAdjustSideBar();

                OpenCloseSidebarBtn.ToolTip = "Close side menu [Esc OR Ctrl + Alt + M]";
                ContentGrid.IsHitTestVisible = false;
            }
        }
        void closeSidebar()
        {
            if (isSideBarOpen)
            {
                SidebarBorder.IsEnabled = false;
                //HomeGrid.Effect = null;
                HomeGrid.IsEnabled = true;

                //Remove the mask
                GridAnimations.OpacityAnimation(MaskingGrid, 0);

                //BorderAnimations.WidthAnimation(SidebarBorder, 0, animationSpeed);
                BorderAnimations.MarginAnimation(SidebarBorder, new Thickness(-1 * this.ActualWidth, 0, 0, 0), AnimationSpeed);
                SidebarBorder.Effect = null;

                isSideBarOpen = false;

                ReAdjustSideBar();

                OpenCloseSidebarBtn.ToolTip = "Open side menu [Ctrl + Alt + M]";
                ContentGrid.IsHitTestVisible = true;
            }
        }
        #endregion
        #region Responsive Functions
        private void ReAdjustSideBar()
        {
            double NewScrollViewerBorderHeight = SidebarBorder.ActualHeight - (ProfileBorder.Margin.Top + ProfileBorder.ActualHeight + DiaryPagesScrollViewerBorder.Margin.Bottom + DiaryPagesScrollViewerBorder.Margin.Top + 20); //60 is the height of heading 
            BorderAnimations.HeightAnimation(DiaryPagesScrollViewerBorder, NewScrollViewerBorderHeight);
            if (!isSideBarOpen)
                SidebarBorder.Margin = new Thickness(0, 0, this.ActualWidth + 50, 0);
        }
        #endregion
    }
}
