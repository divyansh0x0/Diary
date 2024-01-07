using LifeInDiary.Libs;
using LifeInDiary.Libs.Animation;
using LifeInDiary.Main.SettingsFile;
using LifeInDiary.Properties;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LifeInDiary.Main.DiaryWindow
{
    public partial class Diary : Window
    {
        private void RedrawSettingsContainer()
        {
            if (this.ActualWidth <= 0) return;
            if (!isSettingsContainerOpen)
            {
                SettingsPopupBorder.Width = Math.Abs(this.ActualWidth);
                SettingsPopupBorder.Height = Math.Abs(this.ActualHeight);
                return;
            }
            try
            {
                Log.Info(this.RenderSize.Width);
                double NewWidth = settingsContainerWidthPercent * this.RenderSize.Width;
                double NewHeight = settingsContainerHeightPercent * this.RenderSize.Height;

                Log.Info("Settings' container width: " + NewWidth + " X " + NewHeight);
                BorderAnimations.WidthAnimation(SettingsPopupBorder, NewWidth);
                BorderAnimations.HeightAnimation(SettingsPopupBorder, NewHeight);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        #region EVENT HANDLERS
        //Setting's button click event
        private void OpenSettingBtnClick(object sender, RoutedEventArgs e)
        {
            openSettingsContainer();
        }

        //Close settigs btn click event
        private void CloseSettingBtnClick(object sender, RoutedEventArgs e)
        {
            closeSettingsContainer();
        }
        #region opening and closing events
        private void openSettingsContainer()
        {
            //Mask the content
            GridAnimations.OpacityAnimation(MaskingGrid, 1, AnimationSpeed);

            BorderAnimations.OpacityAnimation(SettingsPopupBorder, 1, AnimationSpeed);
            isSettingsContainerOpen = true;

            double NewWidth = settingsContainerWidthPercent * this.RenderSize.Width;
            double NewHeight = settingsContainerHeightPercent * this.RenderSize.Height;

            BorderAnimations.WidthAnimation(SettingsPopupBorder, NewWidth, AnimationSpeed);
            BorderAnimations.HeightAnimation(SettingsPopupBorder, NewHeight, AnimationSpeed);

            ContentGrid.IsEnabled = false;
            ContentGrid.Effect = new BlurEffect();
            OpenSettingsBtn.IsEnabled = false;

            CaptionBar.Effect = new BlurEffect();
            CaptionBar.IsEnabled = false;
        }
        private void closeSettingsContainer()
        {
            if (!isSettingsContainerOpen) return;

            ContentGrid.IsEnabled = true;
            ContentGrid.Effect = null;
            OpenSettingsBtn.IsEnabled = true;


            CaptionBar.Effect = null;
            CaptionBar.IsEnabled = true;

            //Remove the mask
            GridAnimations.OpacityAnimation(MaskingGrid, 0, AnimationSpeed);

            BorderAnimations.OpacityAnimation(SettingsPopupBorder, 0, AnimationSpeed);
            isSettingsContainerOpen = false;

            BorderAnimations.WidthAnimation(SettingsPopupBorder, this.ActualWidth, AnimationSpeed);
            BorderAnimations.HeightAnimation(SettingsPopupBorder, this.ActualHeight, AnimationSpeed);

        }
        #endregion
        #endregion
        #region Settings Tab Button Assignment and event handling
        private void AssignSettingsNameBtnEvents()
        {
            TimeSpan TransitionSpeed = TimeSpan.FromMilliseconds(100);
            foreach (Button button in SettingsNameStackPanel.Children)
            {
                button.Click += (s, e) =>
                {
                    Button EventSenderBtn = s as Button;
                    SwitchSettingsTabs(EventSenderBtn, TransitionSpeed);
                };
                button.GotKeyboardFocus += (s, e) =>
                {
                    Button EventSenderBtn = s as Button;
                    SwitchSettingsTabs(EventSenderBtn, TransitionSpeed);
                };
            }
        }
        private void SwitchSettingsTabs(Button EventSenderBtn, TimeSpan transitionSpeed)
        {
            if (EventSenderBtn == StProfileBtn)
            {
                foreach (Grid grid in SettingsOptionGrid.Children)
                {
                    if (grid == _ProfileSettingsGrid)
                    {
                        GridAnimations.OpacityAnimation(grid, 1, transitionSpeed);
                    }
                    else
                    {
                        GridAnimations.OpacityAnimation(grid, 0, transitionSpeed);
                    }
                }
            }
            if (EventSenderBtn == StHomeBtn)
            {
                foreach (Grid grid in SettingsOptionGrid.Children)
                {
                    if (grid == _HomeSettingsGrid)
                    {
                        GridAnimations.OpacityAnimation(grid, 1, transitionSpeed);
                    }
                    else
                    {
                        GridAnimations.OpacityAnimation(grid, 0, transitionSpeed);
                    }
                }
            }

            if (EventSenderBtn == StEditorBtn)
            {
                foreach (Grid grid in SettingsOptionGrid.Children)
                {
                    if (grid == _TextEditorSettingsGrid)
                    {
                        GridAnimations.OpacityAnimation(grid, 1, transitionSpeed);
                    }
                    else
                    {
                        GridAnimations.OpacityAnimation(grid, 0, transitionSpeed);
                    }
                }
            }

            foreach (Button button1 in SettingsNameStackPanel.Children)
            {
                if (button1 != EventSenderBtn)
                {
                    button1.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    button1.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
                }
            }
        }
        #endregion
        #region Methods
        void ChangeUsername(string newName)
        {
            Settings.Default.Username = newName;
            Settings.Default.Save();
            ApplySettings();
        }
        #endregion
        #region Profile Configuration
        private void PS_EditProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            string initialName = ST_ProfileUsernameField.Text;
            Brush DefaultBrush = ST_ProfileUsernameField.Background;

            ST_ProfileUsernameField.Background = new SolidColorBrush(Color.FromArgb(32, 0, 0, 0));
            ST_ProfileUsernameField.AcceptsTab = true;
            isSettingsUnderUse = false;
            ST_ProfileUsernameField.IsReadOnly = false;

            ST_ProfileUsernameField.SelectAll();
            ST_ProfileUsernameField.Focus();

            ST_ProfileUsernameField.LostFocus += (s, e) =>
            {
                ST_ProfileUsernameField.IsReadOnly = true;
                if (ST_ProfileUsernameField.Text.Length == 0)
                    ST_ProfileUsernameField.Text = initialName;
                isSettingsUnderUse = false;
                ST_ProfileUsernameField.AcceptsTab = false;
                ST_ProfileUsernameField.Background = DefaultBrush;
            };
            ST_ProfileUsernameField.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    ST_ProfileUsernameField.IsReadOnly = true;
                    ST_ProfileUsernameField.Text = initialName;
                    Keyboard.ClearFocus();
                }
                else if (e.Key == Key.Enter)
                {
                    if (ST_ProfileUsernameField.Text.Length > 0)
                    {
                        SettingsPopupBorder.IsEnabled = false;
                        ShowNoTitleMsgBox("Are you sure you want to change your username?");
                        NTMB_Cancel.Focus();
                        NTMB_Confirm.Click += (s, e) =>
                        {
                            ST_ProfileUsernameField.IsReadOnly = true;
                            ChangeUsername(ST_ProfileUsernameField.Text.Trim());

                            HideNoTitleMsgBox();
                            isSettingsUnderUse = false;

                            ShowSnackbar("Name changed successfully");
                            SettingsPopupBorder.IsEnabled = true;
                        };
                        NTMB_Cancel.Click += (s, e) =>
                        {
                            HideNoTitleMsgBox();

                            ST_ProfileUsernameField.IsReadOnly = true;
                            ST_ProfileUsernameField.Text = initialName;

                            Keyboard.ClearFocus();
                            isSettingsUnderUse = false;
                            SettingsPopupBorder.IsEnabled = true;
                        };
                    }
                    else
                    {
                        ShowSnackbar("Please enter a name or press ESC to exit");
                    }
                }
            };

        }

        //About box

        private void PS_AboutTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isSettingsUnderUse = true;
            string LastAbout = PS_AboutTextBox.Text;

            PS_AboutTextBox.IsReadOnly = false;

            PS_AboutTextBox.LostFocus += (s, e) =>
            {
                string currentABout = PS_AboutTextBox.Text;
                if (currentABout.Length > 0 && currentABout != LastAbout)
                {
                    ChangeAbout(currentABout);
                }
                isSettingsUnderUse = false;
                PS_AboutTextBox.ScrollToHome();
            };
            PS_AboutTextBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    //PS_ProfilePic.Focus();
                    Keyboard.ClearFocus();
                    //isSettingsUnderUse = true;
                }
                if (e.Key == Key.Enter)
                {
                    string currentABout = PS_AboutTextBox.Text;
                    if (currentABout.Length > 0 && currentABout != LastAbout)
                    {
                        Keyboard.ClearFocus();
                        ChangeAbout(currentABout);
                        LastAbout = currentABout;
                    }
                    else if (currentABout.Length == 0)
                    {
                        ShowSnackbar("Please enter an about or press ESC to cancel");
                    }
                }
            };
        }

        void ChangeAbout(string text)
        {
            Settings.Default.About = text;
            Settings.Default.Save();
            SB_UserAbout.Text = Settings.Default.About;
            SB_UserAbout.ToolTip = Settings.Default.About;
            PS_AboutTextBox.Text = Settings.Default.About;
            PS_AboutTextBox.ToolTip = Settings.Default.About;
            ShowSnackbar("About changed successfully");
        }
        #endregion
        #region Home Config
        //Home screen style
        private void HS_HSS_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((ComboBoxItem)HS_HSS_ComboBox.SelectedItem).Content.ToString() + HS_HSS_ComboBox.SelectedIndex;

            HomeSettings.Default.HomeScreenStyle = selected;
            HomeSettings.Default.Save();

            ApplySettings();
        }

        //Navbar position setting
        private void HS_NavbarPos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((ComboBoxItem)HS_NavbarPos.SelectedItem).Content.ToString();

            HomeSettings.Default.NavbarPosition = selected;
            HomeSettings.Default.Save();

            ApplySettings();
        }
        #endregion
        void ApplySettings()
        {
            try
            {
                ApplyHomeSettings();
                ApplyProfileSettings();

                DirectoryInfo directory = new DirectoryInfo(@"Images");
                directory.Attributes = FileAttributes.Hidden | FileAttributes.ReadOnly;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        #region Saving and applying Home Settings
        private void ApplyHomeSettings()
        {
            string SavedHomeScreenStyle = HomeSettings.Default.HomeScreenStyle;
            string FormatedSavedHomeScreenStyle = SavedHomeScreenStyle.Remove(SavedHomeScreenStyle.Length - 1);
            int SavedHomeScreenIndex = int.Parse(SavedHomeScreenStyle.ToCharArray()[SavedHomeScreenStyle.Length - 1].ToString());

            Log.Info("Index=>" + SavedHomeScreenIndex);
            Log.Info(FormatedSavedHomeScreenStyle);

            int _HSS_Motivation = 0;
            int _HSS_Dark = 1;
            int _HSS_None = 2;


            HS_HSS_ComboBox.SelectedIndex = SavedHomeScreenIndex;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0);
            dispatcherTimer.Tick += null;
            if (SavedHomeScreenIndex != _HSS_None)
            {
                _HomeScreenQuote.Visibility = Visibility.Visible;
                if (SavedHomeScreenIndex == _HSS_Motivation)
                {
                    dispatcherTimer.Tick += (s, e) =>
                    {
                        _HomeScreenQuote.Text = HomeScreenStyle.GetRandomMotivationalQuote();
                        dispatcherTimer.Interval = TimeSpan.FromSeconds(20);
                    };
                    dispatcherTimer.Start();
                }
                if (SavedHomeScreenIndex == _HSS_Dark)
                {
                    dispatcherTimer.Tick += (s, e) =>
                    {
                        _HomeScreenQuote.Text = HomeScreenStyle.GetRandomDarkQuote();
                        dispatcherTimer.Interval = TimeSpan.FromSeconds(20);
                    };
                    dispatcherTimer.Start();
                }
            }
            else
            {
                dispatcherTimer.Stop();
                HS_HSS_ComboBox.Text = "None";
                _HomeScreenQuote.Visibility = Visibility.Collapsed;
            }

            string SavedNavbarPosition = HomeSettings.Default.NavbarPosition.ToLower();
            int NavbarPos_Bottom = 0;
            int NavbarPos_Left = 1;
            if (SavedNavbarPosition == "left")
            {
                NavigationBarBorder.VerticalAlignment = VerticalAlignment.Stretch;
                NavigationBarBorder.Height = double.NaN;
                NavigationBarBorder.Width = 50;
                NavigationBarBorder.Padding = new Thickness(0, 0, 0, 5);
                NB_BtnStackPanel.Orientation = Orientation.Vertical;
                NB_BtnStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                NB_BtnStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
                NB_BtnStackPanel.Margin = new Thickness(0);

                HS_NavbarPos.Text = "Left";

                CaptionBar.Margin = new Thickness(60, 0, 0, 0);

                NavigationBarBorder.HorizontalAlignment = HorizontalAlignment.Left;

                NavBarPosition = NavPos.Left;

                OpenCloseSidebarBtn.VerticalAlignment = VerticalAlignment.Top;
                OpenCloseSidebarBtn.HorizontalAlignment = HorizontalAlignment.Center;

                HomeGrid.Margin = new Thickness(60, 0, 10, 0);
                TextEditor.Margin = new Thickness(60, 25, 10, 10);
            }

            else
            {
                CaptionBar.Margin = new Thickness(0);
                NavigationBarBorder.VerticalAlignment = VerticalAlignment.Bottom;
                NavigationBarBorder.Height = 50;
                NavigationBarBorder.Width = double.NaN;
                NavigationBarBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
                NavigationBarBorder.Padding = new Thickness(5, 0, 5, 0);

                NB_BtnStackPanel.Orientation = Orientation.Horizontal;
                NB_BtnStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                NB_BtnStackPanel.VerticalAlignment = VerticalAlignment.Center;
                NB_BtnStackPanel.Margin = new Thickness(50, 0, 0, 0);

                HS_NavbarPos.Text = "Bottom";

                NavBarPosition = NavPos.Bottom;

                OpenCloseSidebarBtn.VerticalAlignment = VerticalAlignment.Center;
                OpenCloseSidebarBtn.HorizontalAlignment = HorizontalAlignment.Left;

                HomeGrid.Margin = new Thickness(10, 0, 10, 50);
                TextEditor.Margin = new Thickness(10, 25, 10, 50);
            }
            ReAdjustEditor();
        }
        #endregion
        #region Apply Profile Settings
        public void ApplyProfileSettings()
        {
            SB_UsernameLabel.Text = Settings.Default.Username;
            ST_ProfileUsernameField.Text = Settings.Default.Username;

            SB_UserAbout.Text = Settings.Default.About;
            SB_UserAbout.ToolTip = Settings.Default.About;
            PS_AboutTextBox.Text = Settings.Default.About;
            PS_AboutTextBox.ToolTip = Settings.Default.About;

            string ProfilePicImagePath = Settings.Default.ProfilePicLocation;
            if (ProfilePicImagePath.Length > 0)
            {
                MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(ProfilePicImagePath));
                BitmapImage BTM_image = new BitmapImage();
                BTM_image.BeginInit();
                BTM_image.StreamSource = memoryStream;
                BTM_image.CacheOption = BitmapCacheOption.OnLoad;
                BTM_image.EndInit();

                Image profilePicImage = new Image();
                profilePicImage.Source = BTM_image;
                profilePicImage.Stretch = Stretch.UniformToFill;

                VisualBrush visualBrush = new VisualBrush();
                visualBrush.Visual = profilePicImage;
                visualBrush.Stretch = Stretch.UniformToFill;

                PS_ProfilePic.Background = visualBrush;
                SB_ProfilePic.Fill = visualBrush;
            }
        }
        #endregion

    }
}
