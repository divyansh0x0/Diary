using LifeInDiary.Libs.Animation;
using LifeInDiary.Libs.UI;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace LifeInDiary.Libs.Components
{
    /// <summary>
    /// Interaction logic for FlatMessageBox.xaml
    /// </summary>
    public partial class FlatMessageBox : Window
    {
        public FlatMessageBox()
        {
            InitializeComponent();
            Common.AddChrome(this, TMB_TitleBlock.Height + 5);
        }
        private static FlatMessageBox _messageBox;
        private static Grid Mask;
        private static MessageBoxResult _result = MessageBoxResult.No;


        public static MessageBoxResult Show(string caption, string msg, Grid mask)
        {
            Mask = mask;
            GridAnimations.OpacityAnimation(mask, 1);
            return Show(caption, msg);
        }

        public static MessageBoxResult Show (string caption, string msg)
        {
            _messageBox = new FlatMessageBox();
            _messageBox.TMB_TitleBlock.Text = caption;
            _messageBox.TMB_Message.Text = msg;
            _messageBox.ShowDialog();
            return _result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == TMB_Confirm)
                _result = MessageBoxResult.Yes;
            else if(sender == TMB_Cancel)
                _result = MessageBoxResult.No;
            GridAnimations.OpacityAnimation(Mask, 0);
            _messageBox.Close();
            _messageBox = null;
        }
    }
}
