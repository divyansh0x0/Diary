using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LifeInDiary.Libs.Components
{
    /// <summary>
    /// Interaction logic for DiaryPageVisualizer.xaml
    /// </summary>
    public partial class DiaryPageVisualizer : UserControl
    {
        public DiaryPageVisualizer()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
             {
                 DV_Border.Background = this.Background;
                 this.Background = null;
             };

            DV_CloseBtn.Click += (s, e) =>
            {
                CloseButtonClick(EventArgs.Empty);
            };
        }
        //public Brush Background
        //{
        //    get
        //    {
        //        return DV_Border.Background;
        //    }
        //    set
        //    {
        //        DV_Border.Background = value;
        //        this.Background = null;
        //    }
        //}
        public Stream LoadContentStream
        {
            set
            {
                TextRange textRange = new TextRange(DV_RichTextBox.Document.ContentStart, DV_RichTextBox.Document.ContentEnd);
                textRange.Load(value, DataFormats.Rtf);
                try { value.Flush(); } catch (Exception ex) { Log.Error(ex.Message); };
            }
        }
        public string PageDateTime
        {
            set
            {
                DV_DateTimeTextBox.Text = value;
            }
        }

        public string PageHeading
        {
            set
            {
                DV_DiaryPageName.Text = value;
            }
        }


        public event EventHandler CloseButtonClicked;

        protected virtual void CloseButtonClick(EventArgs e)
        {
            EventHandler handler =  CloseButtonClicked;
            handler?.Invoke(this, e);
        }
        public delegate void ThresholdReachedEventHandler(object sender, EventArgs e);
    }
}
