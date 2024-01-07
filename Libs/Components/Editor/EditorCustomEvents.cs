using System;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LifeInDiary.Libs.Components
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public event EventHandler SaveFile;

        protected virtual void OnSave(EventArgs e)
        {
            EventHandler handler = SaveFile;
            handler?.Invoke(this, e);
        }
        private delegate void ThresholdReachedEventHandler(object sender, EditorEventArgs e);
    }


    class EditorEventArgs : EventArgs
    {
        public FlowDocument EditorDocument { get; set; }
    }
}
