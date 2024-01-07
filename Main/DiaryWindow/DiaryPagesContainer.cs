using LifeInDiary.Libs;
using LifeInDiary.Libs.Animation;
using LifeInDiary.Libs.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Xml;

namespace LifeInDiary.Main.DiaryWindow
{
    public partial class Diary : Window
    {
        bool AreFilesLoaded = false;
        bool LoadMoreFiles = false;
        bool IsReloadPage = false;
        int LoadedFileIndex = 0;
        List<FileInfo> PageFileInfos = new List<FileInfo>();
        List<string> LoadedInfoList = new List<string>();
        #region Border
        private void NB_CloseDiaryPagesBtn_Click(object sender, RoutedEventArgs e)
        {
            CloseDiaryPagesContainer();
        }

        private void CloseDiaryPagesContainer()
        {
            BorderAnimations.OpacityAnimation(DPC_Border, 0, AnimationSpeed);
            BorderAnimations.WidthAnimation(DPC_Border, this.ActualWidth, AnimationSpeed);
            BorderAnimations.HeightAnimation(DPC_Border, this.ActualHeight, AnimationSpeed);

            GridAnimations.OpacityAnimation(DPC_MaskingGrid, 0, AnimationSpeed);
            ContentGrid.IsEnabled = true;
        }

        private void NB_OpenDiaryPagesBtn_Click(object sender, RoutedEventArgs e)
        {
            double NewWidth = 0.95 * this.RenderSize.Width;
            double NewHeight = 0.90 * this.RenderSize.Height;

            BorderAnimations.OpacityAnimation(DPC_Border, 1, AnimationSpeed);
            BorderAnimations.WidthAnimation(DPC_Border, NewWidth, AnimationSpeed);
            BorderAnimations.HeightAnimation(DPC_Border, NewHeight, AnimationSpeed);

            GridAnimations.OpacityAnimation(DPC_MaskingGrid, 1, AnimationSpeed);
            ContentGrid.IsEnabled = false;

            ValidateAndLoadFiles();
        }

        private void ReadjustDPC_Border()
        {
            double NewWidth = 0.95 * this.RenderSize.Width;
            double NewHeight = 0.90 * this.RenderSize.Height;

            BorderAnimations.WidthAnimation(DPC_Border, NewWidth, AnimationSpeed);
            BorderAnimations.HeightAnimation(DPC_Border, NewHeight, AnimationSpeed);

        }
        #endregion


        private void DPC_ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;

            if (AreFilesLoaded == true && LoadMoreFiles == false && scrollViewer.ContentVerticalOffset == scrollViewer.ScrollableHeight)
            {
                if (scrollViewer.ScrollableHeight > 0)
                {
                    LoadMoreFiles = true;
                    ValidateAndLoadFiles();
                }
            }
        }

        private void ValidateAndLoadFiles()
        {
            if (!Directory.Exists($"{DiaryPages.OutputDirectory}")) return;

            if (IsReloadPage)
            {
                DPC_WrapPanel.Children.Clear();
                AreFilesLoaded = false;
                
                LoadedFileIndex = 0;
                PageFileInfos.Clear();
                LoadedInfoList.Clear();
                
                DirectoryInfo directoryInfo = new DirectoryInfo(DiaryPages.OutputDirectory);
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                int maximum = 10;
                for (int i = 0; i < maximum; i++)
                {
                    if (LoadedFileIndex >= fileInfos.Length)
                    {
                        Log.Info("All initial files loaded");
                        break;
                    }
                    FileInfo currentFile = fileInfos[LoadedFileIndex];
                    if (!PageFileInfos.Contains(currentFile))
                        PageFileInfos.Add(currentFile);

                    LoadedFileIndex++;
                }
                IsReloadPage = false;
                AreFilesLoaded = true;
            }

            if (!AreFilesLoaded)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(DiaryPages.OutputDirectory);
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                int maximum = 10;
                for (int i = 0; i < maximum; i++)
                {
                    if (LoadedFileIndex >= fileInfos.Length)
                    {
                        Log.Info("All initial files loaded");
                        break;
                    }
                    FileInfo currentFile = fileInfos[LoadedFileIndex];
                    if (!PageFileInfos.Contains(currentFile))
                        PageFileInfos.Add(currentFile);

                    LoadedFileIndex++;
                }
                AreFilesLoaded = true;
            }
            if (LoadMoreFiles)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(DiaryPages.OutputDirectory);
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                int maximum = 10;
                for (int i = 0; i < maximum; i++)
                {
                    if (LoadedFileIndex >= fileInfos.Length)
                    {
                        break;
                    }

                    FileInfo currentFile = fileInfos[LoadedFileIndex];
                    if (!PageFileInfos.Contains(currentFile))
                        PageFileInfos.Add(currentFile);

                    LoadedFileIndex++;
                }
                LoadMoreFiles = false;
            }
            if (DPC_WrapPanel.Children.Count < 10)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(DiaryPages.OutputDirectory);
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                int maximum = 10;
                for (int i = 0; i < maximum; i++)
                {
                    if (LoadedFileIndex >= fileInfos.Length)
                    {
                        break;
                    }

                    FileInfo currentFile = fileInfos[LoadedFileIndex];
                    if (!PageFileInfos.Contains(currentFile))
                        PageFileInfos.Add(currentFile);

                    LoadedFileIndex++;
                }
            }
            LoadFiles(PageFileInfos);
        }

        private void ValidateAndLoadTile(int number)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(DiaryPages.OutputDirectory);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            int maximum = number;
            for (int i = 0; i < maximum; i++)
            {
                if (LoadedFileIndex >= fileInfos.Length)
                {
                    break;
                }

                FileInfo currentFile = fileInfos[LoadedFileIndex];
                if (!PageFileInfos.Contains(currentFile))
                    PageFileInfos.Add(currentFile);

                LoadedFileIndex++;
            }
            LoadFiles(PageFileInfos);
        }
        #region creating tiles
        private void LoadFiles(List<FileInfo> FileInfos)
        {
            new Task(async () =>
            {
                try
                {
                    foreach (FileInfo fileInfo in FileInfos)
                    {
                        if (LoadedInfoList.Contains(fileInfo.FullName)) continue;
                        string Heading = "";
                        string DateTime = "";
                        string Body = "";
                        try
                        {
                            DPC_Log("Files: " + fileInfo.FullName);
                            string file = fileInfo.FullName;
                            using (XmlReader xmlReader = XmlReader.Create(file))
                            {
                                xmlReader.MoveToContent();
                                while (xmlReader.Read())
                                {
                                    if (xmlReader.NodeType == XmlNodeType.Element)
                                    {
                                        if (xmlReader.Name == "Heading")
                                        {
                                            Heading = Cryptography.DecryptString(xmlReader.ReadElementContentAsString());
                                            //Log.Info($"Heading = {Heading}");
                                        }
                                        if (xmlReader.Name == "DateTime")
                                        {
                                            DateTime = Cryptography.DecryptString(xmlReader.ReadElementContentAsString());
                                            //Log.Info($"Date and time = {DateTime}");
                                        }
                                        if (xmlReader.Name == "Body")
                                        {
                                            Body = Cryptography.DecryptString(xmlReader.ReadElementContentAsString());
                                            //Log.Info($"Body = {Body}");
                                        }
                                    }

                                }
                            }
                            LoadedInfoList.Add(fileInfo.FullName);
                            await this.Dispatcher.BeginInvoke((ThreadStart)delegate { CreateTile(Heading, DateTime, Body, fileInfo.FullName); });

                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            LoadedInfoList.Add(fileInfo.FullName);
                            await this.Dispatcher.BeginInvoke((ThreadStart)delegate { CreateTile(Heading, DateTime, Body, fileInfo.FullName); });
                        }
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }).Start();
        }
        private void CreateTile(string Heading, string DateTime, string content, string fileUrl)
        {
            if (Heading == "" || DateTime == "" || content == "") return;
            DropShadowEffect dropShadow = new DropShadowEffect();
            dropShadow.ShadowDepth = 0;
            dropShadow.BlurRadius = 4;

            DiaryTile savedDiaryTiles = new DiaryTile();
            savedDiaryTiles.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
            savedDiaryTiles.PageFileUrl = fileUrl;

            savedDiaryTiles.MouseEnter += (s, e) => { savedDiaryTiles.Background = new SolidColorBrush(Color.FromArgb(30, 255, 255, 255)); };
            savedDiaryTiles.MouseLeave += (s, e) => { savedDiaryTiles.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255)); };
            savedDiaryTiles.RequestBringIntoView += (s, e) => { e.Handled = true; };
            savedDiaryTiles.OpenButtonClicked += (s, e) =>
            {
                ShowVisualizer();
                ReadPage(Heading, DateTime, content);
            };
            savedDiaryTiles.EditButtonClicked += (s, e) =>
            {
                var clickedTile = s as DiaryTile;
                EditFile(Heading, DateTime, content, fileUrl);
                CloseDiaryPagesContainer();
            };
            savedDiaryTiles.DeleteButtonClicked += (s, e) =>
            {
                var messageBoxResult = FlatMessageBox.Show("Delete Page", "Are you sure you want to permanently delete this page?", WindowMask);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var clickedTile = s as DiaryTile;
                    DPC_WrapPanel.Children.Remove(clickedTile);
                    DeletePage(clickedTile.PageFileUrl);
                    ValidateAndLoadTile(1);
                }
            };


            savedDiaryTiles.PageName = Heading;
            savedDiaryTiles.PageDate = DateTime;
            TextRange richTextRange = new TextRange(savedDiaryTiles.TileRichTextBox.Document.ContentStart, savedDiaryTiles.TileRichTextBox.Document.ContentEnd);
            richTextRange.Load(new MemoryStream(Encoding.UTF8.GetBytes(content)), DataFormats.Rtf);


            DPC_WrapPanel.Children.Add(savedDiaryTiles);
        }
        #endregion

        private void DPC_Log(object data)
        {
            DPC_DebugTextBox.Dispatcher.BeginInvoke((ThreadStart)delegate
            {
                DPC_DebugTextBox.Text += data + "\n";
            });
        }

        #region opening page

        #endregion
        #region Deleting page
        private void DeletePage(string path)
        {
            LoadedFileIndex--;
            new Task(() =>
            {
                FileInfo fileInfo = new FileInfo(path);
                fileInfo.Delete();
                DPC_Log("[Deleted] : " + fileInfo);
            }).Start();
        }
        #endregion

        private void ReloadPages()
        {
            IsReloadPage = true;
        }

    }
}
