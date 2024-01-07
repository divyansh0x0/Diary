using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LifeInDiary.Libs.Components
{
    /// <summary>
    /// Interaction logic for SavedDiaryTiles.xaml
    /// </summary>
    public partial class DiaryTile : UserControl
    {

        public string DiaryID { get; set; }
        public string PageName
        {
            get
            {
                return DiaryHeading.Text.Trim();
            }
            set
            {
                DiaryHeading.Text = value.Trim();
            }
        }
        public string PageDate
        {
            get
            {
                return DiaryDateBlock.Text.Trim();
            }
            set
            {
                DiaryDateBlock.Text = value.Trim();
            }
        }
        public RichTextBox TileRichTextBox
        {
            get
            {
                return DiaryContentBlock;
            }
        }
        public Brush Background
        {
            get
            {
                return bg.Background;
            }
            set
            {
                bg.Background = value;
            }
        }
        public DiaryTile()
        {
            InitializeComponent();
        }

        public string PageFileUrl { get; set; }

        public event EventHandler OpenButtonClicked;

        protected virtual void OpenButtonClick(EventArgs e)
        {
            EventHandler handler = OpenButtonClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler EditButtonClicked;

        protected virtual void EditButtonClick(EventArgs e)
        {
            EventHandler handler = EditButtonClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler DeleteButtonClicked;

        protected virtual void DeleteButtonClick(EventArgs e)
        {
            EventHandler handler = DeleteButtonClicked;
            handler?.Invoke(this, e);
        }
        public delegate void ThresholdReachedEventHandler(object sender, TileClickEventArgs e);

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            TileClickEventArgs tileClickEventArgs = new TileClickEventArgs();
            tileClickEventArgs.DiaryID = DiaryID;
            tileClickEventArgs.PagePath = PageFileUrl;
            OpenButtonClick(tileClickEventArgs);
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            TileClickEventArgs tileClickEventArgs = new TileClickEventArgs();
            tileClickEventArgs.DiaryID = DiaryID;
            tileClickEventArgs.PagePath = PageFileUrl;
            EditButtonClick(tileClickEventArgs);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            TileClickEventArgs tileClickEventArgs = new TileClickEventArgs();
            tileClickEventArgs.DiaryID = DiaryID;
            tileClickEventArgs.PagePath = PageFileUrl;
            DeleteButtonClick(tileClickEventArgs);
        }
    }
    public class TileClickEventArgs : EventArgs
    {
        public string DiaryID { get; set; }
        public string PagePath { get; set; }
    }
}
