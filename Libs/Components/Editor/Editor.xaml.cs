using LifeInDiary.Libs.UI;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace LifeInDiary.Libs.Components
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        bool isDiaryTextEditorOpen = false;
        bool isDistractionFreeModeOn = false;
        private SolidColorBrush DefaultFontColorBrush;
        //private ColorPickerControl colorPickerControl = new ColorPickerControl();

        TextEditing _TextEditing;
        bool _IsTextBold = false;
        bool _IsTextItalic = false;
        bool _IsTextUnderlined = false;

        public Editor()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                DropShadowEffect dropShadowEffect = new DropShadowEffect();
                dropShadowEffect.ShadowDepth = 0;
                dropShadowEffect.BlurRadius = 10;

                TextEditorBorder.Background = this.Background;
                this.Background = null;
                ReAdjustEditor();
                DefaultFontColorBrush = (SolidColorBrush)TextEditorBox.Foreground;
            };
            this.SizeChanged += (s, e) =>
            {
                ReAdjustEditor();
            };

            TE_FontSizeComboBox.Loaded += (sender, e) =>
            {
                var obj = (ComboBox)sender;
                if (obj != null)
                {
                    var myTextBox = (TextBox)obj.Template.FindName("PART_EditableTextBox", obj);
                    if (myTextBox != null)
                    {
                        myTextBox.MaxLength = 2;
                        myTextBox.TextChanged += (s, e) =>
                        {
                            try
                            {
                                myTextBox.Text = Math.Round(Convert.ToDecimal(myTextBox.Text)).ToString();
                            }
                            catch (Exception ex) { Log.Error(ex); }
                        };
                    }
                };
            };
            TE_DiaryPageName.PreviewMouseLeftButtonDown += (s, e) =>
            {
                if (!TE_DiaryPageName.IsKeyboardFocusWithin)
                {
                    TE_DiaryPageName.Focus();
                    e.Handled = true;
                }
            };
            TE_DiaryPageName.GotKeyboardFocus += (s, e) =>
            {
                TE_DiaryPageName.SelectAll();
                e.Handled = true;
            };
            TE_DiaryPageName.MouseDoubleClick += (s, e) =>
            {
                TE_DiaryPageName.SelectAll();
                e.Handled = true;
            };
            _TextEditing = new TextEditing(TextEditorBox);
            _TextEditing.RemoveAllKeyBindings();
            Common.AddFontsToCB(TE_FontSelector, TextEditorBox.FontFamily.Source);
            Common.AddFontSizeToCB(TE_FontSizeComboBox);
        }
        /// <summary>
        /// Returns a text range containing editor content
        /// </summary>
        public TextRange GetEditorText
        {
            get
            {
                return new TextRange(TextEditorBox.Document.ContentStart, TextEditorBox.Document.ContentEnd);
            }
        }

        /// <summary>
        /// Loads a stream of text file into the text editor
        /// </summary>
        public Stream LoadTextStream
        {
            set
            {
                TextRange textRange = new TextRange(TextEditorBox.Document.ContentStart, TextEditorBox.Document.ContentEnd);
                textRange.Load(value, DataFormats.Rtf);
                value.Flush();
            }
        }

        /// <summary>
        /// Returns the date time return in the header
        /// </summary>
        public string EditorDateTime
        {
            get
            {
                return DiaryPageHeaderTextBox.Text;
            }
            set
            {
                DiaryPageHeaderTextBox.Text = value.Trim();
            }
        }
        public string PageName
        {
            get
            {
                return TE_DiaryPageName.Text;
            }
            set
            {
                TE_DiaryPageName.Text = value.Trim();
            }
        }
        public DiaryDateFormat HeaderDateFormat { get; set; }
        public DiaryTimeFormat HeaderTimeFormat { get; set; }
        /// <summary>
        /// Updates the header date time
        /// </summary>
        public void UpdateHeaderDateTime()
        {
            DiaryPageHeaderTextBox.Text = $"{DiaryComponents.GetDay()}, {DiaryComponents.GetDate(HeaderDateFormat)}, {DiaryComponents.GetTime(HeaderTimeFormat)}";
        }
        #region Open/Collapse editor methods
        public void CollapseEditor()
        {

        }
        public void ShowEditor()
        {

        }
        public void OpenNewEditor()
        {
            isDiaryTextEditorOpen = true;
            DiaryDateFormat diaryDateFormat = DiaryDateFormat.US;
            DiaryPageHeaderTextBox.Text = $"{DiaryComponents.GetDay()}, {DiaryComponents.GetDate(diaryDateFormat)}, { DiaryComponents.GetTime(DiaryTimeFormat._12)}";

            TE_FontSelector.SelectedItem = TextEditorBox.FontFamily.FamilyNames;


        }
        #endregion
        private void ReAdjustEditor()
        {

            TextEditorBox.Height = TextEditorScrollViewer.ActualHeight;
        }
        private string GetTextFromRichTextBox(RichTextBox textBox)
        {
            TextRange textRange = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
            return textRange.Text;
        }
        #region Event handlers
        #region Editor Selection changed
        private void TextEditorBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp;
            //Changing font weight
            temp = TextEditorBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            TE_BoldTextBtn.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            //Changing font style
            temp = TextEditorBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            TE_ToggleItalicBtn.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            //Changing font decoration
            temp = TextEditorBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            TE_ToggleUnderlineBtn.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            //Changing font size
            temp = TextEditorBox.Selection.GetPropertyValue(Inline.FontSizeProperty);
            TE_FontSizeComboBox.Text = temp.ToString();
        }
        #endregion
        private void TextEditorBorder_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ReAdjustEditor();
        }
        #region Text editor key combinations
        //Text Editor key functions
        private void TextEditorBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                #region Select all
                if (e.Key == Key.A)
                {
                    try
                    {
                        TextPointer startSelection = TextEditorBox.Document.ContentStart;
                        TextPointer endSelection = startSelection.GetPositionAtOffset(GetTextFromRichTextBox(TextEditorBox).Length);
                        TextEditorBox.Selection.Select(startSelection, endSelection);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
                #endregion
                #region Bold Text
                if (_IsTextBold && e.Key == Key.B)
                {
                    _TextEditing.toggleBold();
                    TE_BoldTextBtn.IsChecked = false;
                    _IsTextBold = false;
                }
                else if (!_IsTextBold && e.Key == Key.B)
                {
                    TE_BoldTextBtn.IsChecked = true;
                    _TextEditing.toggleBold();
                    _IsTextBold = true;
                }
                #endregion
                #region Toggle Italic
                if (_IsTextItalic && e.Key == Key.I)
                {
                    TE_ToggleItalicBtn.IsChecked = false;
                    _TextEditing.toggleItalic();
                    _IsTextItalic = false;
                }
                else if (!_IsTextBold && e.Key == Key.I)
                {
                    TE_ToggleItalicBtn.IsChecked = true;
                    _TextEditing.toggleItalic();
                    _IsTextItalic = true;
                }
                #endregion
                #region Underline Text
                if (_IsTextUnderlined && e.Key == Key.U)
                {
                    TE_ToggleUnderlineBtn.IsChecked = false;
                    _TextEditing.toggleUnderline();
                    _IsTextUnderlined = false;
                }
                else if (!_IsTextUnderlined && e.Key == Key.U)
                {
                    TE_ToggleUnderlineBtn.IsChecked = true;
                    _TextEditing.toggleUnderline();
                    _IsTextUnderlined = true;
                }
                #endregion

                #region raise save file event
                if (e.Key == Key.S)
                {
                    RaiseSaveFileEvent();
                }
                #endregion
            }
            #region Tab and Enter
            if (e.Key == Key.Enter)
            {
                EditingCommands.EnterLineBreak.Execute(null, TextEditorBox);
                EditingCommands.MoveToLineStart.Execute(null, TextEditorBox);
            }
            #endregion
        }
        #endregion
        #region Mouse wheel
        //Increase decrease font size
        private double transformSize = 1;

        private void TextEditorBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Delta > 0)
                {
                    if (transformSize <= 5)
                    {
                        transformSize += 0.2;
                    }
                }
                else if(e.Delta < 0)
                {
                    if (transformSize >= 1)
                    {
                        transformSize -= 0.2;
                    }
                }
                EditorBoxScaleTransform.ScaleY = transformSize;
                EditorBoxScaleTransform.ScaleX = transformSize;
            }
            
        }
        #endregion

        private void FocusTextEditorButton_Click(object sender, RoutedEventArgs e)
        {
            //EnterDistractionFreeMode();
        }
        #endregion

        #region Distraction Free Mode
        private void SlideConfigurationBorderRect_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void ConfigurationBorder_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void ConfigurationBorder_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        #endregion

        #region Toolbar event handler
        #region Bold text
        private void TE_BoldTextBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (TE_BoldTextBtn.IsChecked == true)
            {
                _IsTextBold = true;
            }
            else
            {
                _IsTextBold = false;
            }
        }
        private void TE_BoldTextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TE_BoldTextBtn.IsChecked == true)
            {
                if (TextEditorBox.IsSelectionActive)
                {
                    TextEditorBox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
                }
                _IsTextBold = true;
            }
            else
            {
                if (TextEditorBox.IsSelectionActive)
                {
                    TextEditorBox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
                }
                _IsTextBold = false;
            }
            _TextEditing.toggleBold();
            TextEditorBox.Focus();
        }
        #endregion
        #region Toggle Italic
        private void TE_ToggleItalicBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (TE_ToggleItalicBtn.IsChecked == true)
            {
                _IsTextItalic = true;
            }
            else
            {
                _IsTextItalic = false;
            }
        }
        private void ToggleItalicBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TE_ToggleItalicBtn.IsChecked == true)
            {
                if (TextEditorBox.IsSelectionActive)
                {
                    TextEditorBox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
                }
                _IsTextItalic = true;
            }
            else
            {
                if (TextEditorBox.IsSelectionActive)
                {
                    TextEditorBox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
                }
                _IsTextItalic = false;
            }
            _TextEditing.toggleItalic();
            TextEditorBox.Focus();
        }
        #endregion
        #region Toggle underline
        private void TE_ToggleUnderlineBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (TE_ToggleUnderlineBtn.IsChecked == true)
            {
                _IsTextUnderlined = true;
            }
            else
            {
                _IsTextUnderlined = false;
            }
        }
        private void TE_ToggleUnderlineBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TE_ToggleUnderlineBtn.IsChecked == true)
            {
                if (TextEditorBox.IsSelectionActive)
                {
                    TextEditorBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                }
                _IsTextUnderlined = true;
            }
            else
            {
                if (TextEditorBox.IsSelectionActive)
                {
                    TextEditorBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                }
                _IsTextUnderlined = false;
            }
            _TextEditing.toggleUnderline();
            TextEditorBox.Focus();
        }
        #endregion
        #region Font family selector
        private void TE_FontSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (TE_FontSelector.SelectedItem == null) return;
                string SelectedFont = ((ComboBoxItem)TE_FontSelector.SelectedItem).Content.ToString();
                if (!TextEditorBox.Selection.IsEmpty)
                {
                    TextEditorBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, SelectedFont);
                }
                else
                {
                    TextEditorBox.FontFamily = new FontFamily(SelectedFont);
                    TE_FontSelector.ToolTip = SelectedFont;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        #endregion
        #region Font size selector
        private void TE_FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //int SelectedSize = Convert.ToInt32(((ComboBoxItem)TE_FontSizeComboBox.SelectedItem).Content.ToString());
                if (TE_FontSizeComboBox.SelectedItem == null) return;
                if (!TextEditorBox.Selection.IsEmpty)
                {
                    TextEditorBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, TE_FontSizeComboBox.Text);
                }
                else if (TextEditorBox.CaretPosition == TextEditorBox.Document.ContentStart)
                {
                    TextEditorBox.FontSize = int.Parse(TE_FontSizeComboBox.Text);
                    TE_FontSizeComboBox.Text = TextEditorBox.FontSize.ToString();
                }
                Log.Info(TE_FontSizeComboBox.Text);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            TextEditorBox.Focus();
        }

        private void TE_FontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TE_FontSizeComboBox.Text.Length > 0)
                    TextEditorBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, TE_FontSizeComboBox.Text);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void TE_FontSizeComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
        #endregion



        private void Image_Initialized(object sender, EventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(File.ReadAllBytes(@"Images/save-files.png"));
            bitmap.EndInit();
            Image image = sender as Image;
            image.Source = bitmap;
        }

        private void TE_SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            RaiseSaveFileEvent();
        }
        private void RaiseSaveFileEvent()
        {
            EditorEventArgs editorEventArgs = new EditorEventArgs();
            editorEventArgs.EditorDocument = TextEditorBox.Document;
            OnSave(editorEventArgs);
        }
    }

}