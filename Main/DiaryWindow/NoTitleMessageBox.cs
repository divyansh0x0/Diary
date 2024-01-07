using LifeInDiary.Libs.Animation;
using System.Windows;

namespace LifeInDiary.Main.DiaryWindow
{
    public partial class Diary : Window
    {
        void ShowNoTitleMsgBox(string Message)
        {
            BorderAnimations.OpacityAnimation(NoTitleMsgBox, 1, AnimationSpeed);
            GridAnimations.OpacityAnimation(MB_MaskingGrid, 1, AnimationSpeed);

            IsNoTitleMessageBoxOpen = true;
        }
        void HideNoTitleMsgBox()
        {
            BorderAnimations.OpacityAnimation(NoTitleMsgBox, 0, AnimationSpeed);
            GridAnimations.OpacityAnimation(MB_MaskingGrid, 0, AnimationSpeed);

            IsNoTitleMessageBoxOpen = false;
        }
    }
}
