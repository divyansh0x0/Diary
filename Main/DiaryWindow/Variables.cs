using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace LifeInDiary.Main.DiaryWindow
{
    public partial class Diary : Window
    {
        #region Animation Variables
        TimeSpan AnimationSpeed = TimeSpan.FromMilliseconds(200);
        #endregion
        #region WINDOW's VARIABLES
        private int CAPTION_HEIGHT = 20;

        private bool IsNoTitleMessageBoxOpen = false;
        #endregion
        #region Side Bar
        bool isSideBarOpen = false;
        #endregion
        #region Setting's Container
        bool isSettingsContainerOpen = false;
        bool isSettingsUnderUse = false;

        double settingsContainerWidthPercent = 0.95;
        const double settingsContainerHeightPercent = 0.90;
        #endregion
        #region Diary Text Editor
        bool isDiaryTextEditorOpen = false;
        bool TE_IsExistingPage = false;
        bool TE_IsEditingPage = false;
        bool TE_IsNewPage = false;
        SolidColorBrush DefaultConfigurationBarBg;
        SolidColorBrush DefaultTextEditorbackgroundBrush;

        //TextEditing _TextEditing;
        bool _IsTextBold = false;
        #endregion
        #region Navigation Bar
        NavPos NavBarPosition;
        #endregion
    }

    public enum NavPos
    {
        Bottom = 0,
        Left = 1,
    }

    public static class DiaryPages
    {
        public static string Extension = "diarypage";
        public static string OutputDirectory = $@"{Directory.GetCurrentDirectory()}\DiaryFiles";
    }
}
