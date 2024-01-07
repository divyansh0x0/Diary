using LifeInDiary.Libs;
using LifeInDiary.Libs.Animation;
using LifeInDiary.Libs.UI;
using LifeInDiary.Properties;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LifeInDiary.Main.DiaryWindow
{
    /// <summary>
    /// Interaction logic for Diary.xaml
    /// </summary>
    public partial class Diary : Window
    {
        public Diary()
        {
            InitializeComponent();
            Common.AddChrome(this, CAPTION_HEIGHT, new Thickness(5, 3, 5, 5));

            //Set a new diary background
            CreateDiaryBackground();

            //Removes some default keybindings from TextEditor
            RemoveKeyBindings();

            CaptionBarTitle.Text = "";

            ApplySettings();

            SettingsPopupBorder.Width = this.ActualWidth + 300;
            SettingsPopupBorder.Height = this.ActualHeight + 600;
            SettingsPopupBorder.Visibility = Visibility.Collapsed;
            SettingsPopupBorder.Opacity = 0;
            SettingsPopupBorder.Margin = new Thickness(0);

            AssignSettingsNameBtnEvents();
            HideNoTitleMsgBox();
            HideSnackbar();

            SidebarBorder.IsEnabled = true;
            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new FileStream(@"Images/icon.png", FileMode.Open);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                CSThumbnailImage.Source = image;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            TextEditor.Visibility = Visibility.Collapsed;
            TextEditor.Opacity = 0;

            HomeGrid.Visibility = Visibility.Visible;
            HomeGrid.Opacity = 1;

            DPC_Border.Visibility = Visibility.Hidden;
            DPC_Border.Opacity = 0;

            var parent = DPC_DebugTextBox.Parent as ScrollViewer;
            parent.Visibility = Visibility.Collapsed;

            HideVisualizer();
        }

        private void CreateDiaryBackground()
        {

        }

        private void RemoveKeyBindings()
        {
            KeyBinding removeSelectAllKeyBinding = new KeyBinding(ApplicationCommands.NotACommand, Key.A, ModifierKeys.Control);
            KeyBinding IngoreDefaultTabKeyBinding = new KeyBinding(ApplicationCommands.NotACommand, Key.Tab, ModifierKeys.None);
            KeyBinding OpenSettingsKeyBinding = new KeyBinding(ApplicationCommands.NotACommand, Key.S, ModifierKeys.Alt);

            //this.InputBindings.Add(OpenSettingsKeyBinding);
            //TextEditorBox.InputBindings.Add(IngoreDefaultTabKeyBinding);
        }
        #region Windows's event handlers
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Reheight the Diary Container
            ReAdjustSideBar();

            //Redraw the settings popup
            RedrawSettingsContainer();

            //If the window is maximized change the caption buttons and add margin to the root grid
            if (WindowState == WindowState.Maximized)
            {
                MaximizeBtn.Visibility = Visibility.Collapsed;
                RestoreBtn.Visibility = Visibility.Visible;
                RootGrid.Margin = new Thickness(4);
            }
            else if (WindowState == WindowState.Normal)
            {
                MaximizeBtn.Visibility = Visibility.Visible;
                RestoreBtn.Visibility = Visibility.Collapsed;
                RootGrid.Margin = new Thickness(0);
            }

            //Re position the editor
            ReAdjustEditor();

            //Re position the pages conatiner
            ReadjustDPC_Border();

            ReadjustSnackbar();
        }
        #endregion
        #region Caption button's event
        private void MinimizeBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void RestoreBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void CloseBtnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Key Events
        private void WindowsKeyEvents(object sender, KeyEventArgs e)
        {
            if (!IsNoTitleMessageBoxOpen)
            {
                //Configuration [Settings] shortcut keys
                //if (!isSettingsUnderUse && isSettingsContainerOpen && e.Key == Key.Escape)
                //{
                //    closeSettingsContainer();
                //    return;
                //}
                if (!isSettingsContainerOpen && e.Key == Key.S && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)))
                {
                    openSettingsContainer();
                    return;
                }

                //Side bar shortcut keys
                if (isSideBarOpen && e.Key == Key.Escape)
                {
                    closeSidebar();
                    return;
                }
                else if (isSideBarOpen && e.Key == Key.M && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)))
                {
                    closeSidebar();
                    return;
                }
                if (!isSideBarOpen && e.Key == Key.M && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)))
                {
                    openSidebar();
                    return;
                }
            }
        }
        #endregion
        #region Mouse events
        private void CreateNewDiaryBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateNewPage();
        }

        private void MaskingGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isSideBarOpen) { closeSidebar(); }
        }
        private void ContentGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isSideBarOpen)
            {
                //HomeGrid.Effect = null;
                HomeGrid.IsEnabled = true;

                //Remove the mask
                GridAnimations.OpacityAnimation(MaskingGrid, 0);

                BorderAnimations.WidthAnimation(SidebarBorder, 0, TimeSpan.FromMilliseconds(300));
                GridAnimations.MarginAnimation(HomeGrid, new Thickness(0, HomeGrid.Margin.Top, HomeGrid.Margin.Right, HomeGrid.Margin.Bottom), TimeSpan.FromMilliseconds(300));
                isSideBarOpen = false;
            }
        }
        #endregion

        #region Misc Methods
        private void ShowTip(string TipsText)
        {
            TipsBorder.Visibility = Visibility.Collapsed;
            TipsBorder.Opacity = 0;

            TipsTextBlock.Text = TipsText;
            BorderAnimations.TempVisibilityAnimation(TipsBorder, TimeSpan.FromMilliseconds(200), TimeSpan.FromSeconds(3));
        }
        #endregion

        private void OpenProfilePicDialogBoxBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".jpeg";
            fileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.PNG";

            Nullable<bool> result = fileDialog.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string srcFilename = fileDialog.FileName;
                string DestinationFile = @"Images/ProfilePic.png";

                Log.Info(srcFilename);
                File.Copy(srcFilename, DestinationFile, true);

                Settings.Default.ProfilePicLocation = DestinationFile;
                Settings.Default.Save();
                Log.Info("saved destination " + DestinationFile);
                ApplyProfileSettings();
            }
        }
    }
}
//_||||_________-------------
