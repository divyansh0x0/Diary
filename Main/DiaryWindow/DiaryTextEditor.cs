using LifeInDiary.Libs;
using LifeInDiary.Libs.Animation;
using LifeInDiary.Libs.Components;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Xml;

namespace LifeInDiary.Main.DiaryWindow
{
    public partial class Diary : Window
    {
        string OutputFileName = "";
        private void CreateNewPage()
        {
            TE_IsNewPage = true;
            TE_IsExistingPage = false;
            TextEditor.Visibility = Visibility.Visible;
            TextEditor.Opacity = 0;
            TextEditor.HeaderTimeFormat = DiaryTimeFormat._12;
            TextEditor.HeaderDateFormat = DiaryDateFormat.US;
            TextEditor.UpdateHeaderDateTime();
            byte[] emptyByte = new byte[1]; 
            TextEditor.LoadTextStream = new MemoryStream(emptyByte);
            TextEditor.PageName = "New Page [Click to Edit]";

            TextEditor.SaveFile += (s, e) =>
            {
                SaveDiary(TextEditor.PageName);
            };

            GridAnimations.OpacityAnimation(HomeGrid, 0, AnimationSpeed);
            DoubleAnimation doubleAnimation = new DoubleAnimation(1, AnimationSpeed);

            TextEditor.BeginAnimation(Editor.OpacityProperty, doubleAnimation);
            //TE_IsNewPage = false;
        }
        private void ReAdjustEditor()
        {

        }
        private void SaveDiary(string OutputFileName)
        {
            if (TE_IsNewPage && File.Exists($"{DiaryPages.OutputDirectory}\\{TextEditor.PageName}.{DiaryPages.Extension}"))
            {
                var messageBoxResult = FlatMessageBox.Show("Replace Page", "A page with same name already exist. Are you sure you want to replace it?", WindowMask);
                if (messageBoxResult != MessageBoxResult.Yes) return;
                TE_IsNewPage = false;
            }
            bool Replace = !TE_IsNewPage ? true : false;
            try
            {

                new Task(() =>
                {
                    bool ReplaceExistingFile = Replace;
                    string OutputFile = OutputFileName.Clone().ToString();
                    OutputFile = OutputFile.Replace(":", "-");
                    string outputDirectory = DiaryPages.OutputDirectory;
                    if (!Directory.Exists(outputDirectory))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(outputDirectory);
                        directoryInfo.Attributes = FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.System;
                        directoryInfo.Create();
                    }

                    if (!ReplaceExistingFile)
                    {
                        string tempFileName = OutputFile;
                        int i = 0;
                        while (File.Exists($"{outputDirectory}\\{tempFileName}.{DiaryPages.Extension}"))
                        {
                            i++;
                            tempFileName = $"{OutputFile}_{i}";
                        }
                        OutputFileName = tempFileName;
                        OutputFile = OutputFileName;
                        Log.Info($"output directory {outputDirectory}\\{OutputFile}");
                    }
                    outputDirectory = $"{outputDirectory}\\{OutputFile}.{DiaryPages.Extension}";
                    GenerateXml(outputDirectory);
                }).Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSnackbar("An error occurred while saving the page");
            }
        }

        private async Task GenerateXml(string OutputFile)
        {
            string EncryptedTextRange = "";
            string EncryptedDateTime = "";
            string EncryptedHeading = "";
            await this.Dispatcher.InvokeAsync((Action)delegate
            {
                TextRange textRange = TextEditor.GetEditorText;
                EncryptedTextRange = Cryptography.EncryptTextRange(textRange);
                EncryptedDateTime = Cryptography.EncryptString(TextEditor.EditorDateTime);
                EncryptedHeading = Cryptography.EncryptString(TextEditor.PageName);
            });

            using (var xmlWriter = XmlWriter.Create(new FileStream(OutputFile, FileMode.OpenOrCreate)))
            {
                xmlWriter.WriteStartElement("Page");
                xmlWriter.WriteElementString("Heading", EncryptedHeading);
                xmlWriter.WriteElementString("DateTime", EncryptedDateTime);
                xmlWriter.WriteElementString("Body", EncryptedTextRange);
                xmlWriter.WriteEndElement();
                xmlWriter.Close();
            }
            await this.Dispatcher.InvokeAsync((Action)delegate
            {
                ShowSnackbar("Page saved successfully");
                ReloadPages();
            });
        }

        private void EditFile(string Heading, string DateTime, string Content, string FileName)
        {
            TextEditor.EditorDateTime = DateTime;
            TextEditor.PageName = Heading;
            TextEditor.LoadTextStream = new MemoryStream(Encoding.UTF8.GetBytes(Content));

            TE_IsEditingPage = true;
            TextEditor.Visibility = Visibility.Visible;
            TextEditor.Opacity = 0;
            TextEditor.HeaderTimeFormat = DiaryTimeFormat._12;
            TextEditor.HeaderDateFormat = DiaryDateFormat.US;
            //TextEditor.UpdateHeaderDateTime();
            TextEditor.SaveFile += (s, e) =>
            {
                try
                {

                    new Task(() =>
                    {
                        string outputDirectory = FileName;
                        GenerateXml(outputDirectory);
                    }).Start();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    ShowSnackbar("An error occurred while saving the page");
                }
            };

            GridAnimations.OpacityAnimation(HomeGrid, 0, AnimationSpeed);
            DoubleAnimation doubleAnimation = new DoubleAnimation(1, AnimationSpeed);

            TextEditor.BeginAnimation(Editor.OpacityProperty, doubleAnimation);
        }
    }

}

