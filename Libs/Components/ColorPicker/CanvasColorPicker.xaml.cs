using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LifeInDiary.Libs.Components.ColorPicker
{
    /// <summary>
    /// Interaction logic for CanvasColorPicker.xaml
    /// </summary>
    public partial class CanvasColorPicker : UserControl
    {
        private bool isMouseDown = false;
        private bool isMouseDownOnHue = false;
        Point SaturationRectPoint = new Point(0, 0);
        Point MinSaturationRectPos = new Point(-6, -6);


        public SolidColorBrush PickedColor
        {
            get
            {
                return (SolidColorBrush)SelectedColorRect.Fill;
            }
            //set
            //{
            //    SetPickedColor(value.Color);
            //}
        }


        public CanvasColorPicker()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                bg.Background = this.Background;
                this.Background = null;


                //SetSaturationCanvas();
                HueChooserRect.Width = HueCanvas.ActualWidth;
                HueCanvas.MaxHeight = HueBorder.ActualHeight;
                HueCanvas.Height = HueBorder.ActualHeight;

                SaturationRectCanvas.Height = SaturationRectBorder.ActualHeight;
                SaturationRectCanvas.Width = SaturationRectBorder.ActualWidth;

                SelectedColorRect.Fill = new SolidColorBrush(GetPixelColor(SaturationRectCanvas, new Point(Canvas.GetLeft(SaturationChooserRect), Canvas.GetTop(SaturationChooserRect))));
                GetPointSaturation();
            };

            this.MouseLeftButtonUp += (s, e) =>
            {
                ReleaseMouseCapture();
                isMouseDown = false;
                isMouseDownOnHue = false;
            };
            this.MouseLeave += (s, e) =>
            {
                isMouseDown = false;
                isMouseDownOnHue = false;
                ReleaseMouseCapture();
                ControlGrid.IsHitTestVisible = true;
            };

            AddSaturationEventHandler();
            AddHueEventHandler();
        }
        #region Saturation event handler
        private void AddSaturationEventHandler()
        {
            SaturationRectCanvas.PreviewMouseLeftButtonDown += (s, e) =>
            {
                isMouseDown = true;
                ControlGrid.IsHitTestVisible = false;
                GetSaturationColor();
            };
            SaturationRectCanvas.MouseMove += (s, e) =>
            {
                if (isMouseDown)
                {
                    GetSaturationColor();
                }
            };
            SaturationRectCanvas.PreviewMouseLeftButtonUp += (s, e) =>
            {
                isMouseDown = false;
                ControlGrid.IsHitTestVisible = true;
                ReleaseMouseCapture();
            };
        }
        #endregion
        #region Hue event handler
        private void AddHueEventHandler()
        {
            HueCanvas.PreviewMouseLeftButtonDown += (s, e) =>
            {
                isMouseDownOnHue = true;
                ControlGrid.IsHitTestVisible = false;
                GetHue();
            };
            HueCanvas.MouseMove += (s, e) =>
            {
                if (isMouseDownOnHue)
                {
                    GetHue();
                }
            };
            HueCanvas.PreviewMouseLeftButtonUp += (s, e) =>
            {
                isMouseDownOnHue = false;
                ControlGrid.IsHitTestVisible = true;
                ReleaseMouseCapture();
            };
        }
        #endregion
        private void bg_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                GetSaturationColor();
            }
            if (isMouseDownOnHue)
            {
                GetHue();
            }
        }

        private void SetPickedColor(Color color)
        {
            Point SaturationRectPos = new Point();
            double MaxRectPosX = SaturationRectBorder.ActualWidth - (SaturationChooserRect.Width / 2);
            double MaxRectPosY = SaturationRectBorder.ActualHeight - (SaturationChooserRect.Width / 2);

            if (
                (color.R == 0 && color.B == 255) ||
                (color.R == 0 && color.G == 255) ||
                (color.R == 255 && color.B == 0) ||
                (color.R == 255 && color.G == 0) ||
                (color.B == 0 && color.G == 255) ||
                (color.B == 255 && color.G == 0)
                )
            {
                SaturationRectPos.X = MaxRectPosX;
                SaturationRectPos.Y = MinSaturationRectPos.Y;
            }
            else if (color.R == color.B && color.R == color.G)
            {
                int commonColorValue = color.R;
                SaturationRectPos.X = MinSaturationRectPos.X;
                double avarage = MaxRectPosY / 255;
                SaturationRectPos.Y = MaxRectPosY - (avarage * commonColorValue);

                Log.Info("Saturation color rect position: " + SaturationRectPos.Y);
            }

            if (SaturationRectPos.X < MinSaturationRectPos.X)
                SaturationRectPos.X = MinSaturationRectPos.X;
            if (SaturationRectPos.Y < MinSaturationRectPos.Y)
                SaturationRectPos.Y = MinSaturationRectPos.Y;
            if (SaturationRectPos.X >= MaxRectPosX)
                SaturationRectPos.X = MaxRectPosX;
            if (SaturationRectPos.Y >= MaxRectPosY)
                SaturationRectPos.Y = MaxRectPosY;

            Canvas.SetLeft(SaturationChooserRect, SaturationRectPos.X);
            Canvas.SetTop(SaturationChooserRect, SaturationRectPos.Y);

            Point PixelPos = new Point(Canvas.GetLeft(SaturationChooserRect) + (SaturationChooserRect.ActualWidth / 2), Canvas.GetTop(SaturationChooserRect) + (SaturationChooserRect.ActualHeight / 2));

            SelectedColorRect.Fill = new SolidColorBrush(GetPixelColor(SaturationRectCanvas, PixelPos));
            OnSelectedColorChange(new ColorPickerEventArgs() { SelectedColor = SelectedColorRect.Fill });
        }

        private void GetHue()
        {

            Point RectPos = new Point(0, Mouse.GetPosition(HueCanvas).Y - (HueChooserRect.ActualHeight / 2));
            double MaxRectPosY = HueCanvas.ActualHeight - (HueChooserRect.ActualHeight / 2) - 1;
            if (RectPos.Y < -(HueChooserRect.ActualHeight / 2))
                RectPos.Y = -(HueChooserRect.ActualHeight / 2);
            if (RectPos.Y > MaxRectPosY)
                RectPos.Y = MaxRectPosY;

            Point PixelPos = new Point(0, Canvas.GetTop(HueChooserRect) + (HueChooserRect.ActualHeight / 2));
            Canvas.SetTop(HueChooserRect, RectPos.Y);
            CP_SaturationGradientCurrentColor.Color = GetPixelColor(HueCanvas, PixelPos);

            GetPointSaturation();
            OnSelectedColorChange(new ColorPickerEventArgs() { SelectedColor = SelectedColorRect.Fill });
        }


        private void GetSaturationColor()
        {

            Log.Info($"Saturation canvas => {SaturationRectCanvas.ActualWidth} x {SaturationRectCanvas.ActualHeight}");
            Point MousePos = Mouse.GetPosition(SaturationRectCanvas);
            Point RectPos = new Point(MousePos.X - (SaturationChooserRect.Width / 2), MousePos.Y - (SaturationChooserRect.Height / 2));
            double MaxRectPosX = SaturationRectBorder.ActualWidth - (SaturationChooserRect.Width / 2);
            double MaxRectPosY = SaturationRectBorder.ActualHeight - (SaturationChooserRect.Width / 2);
            if (RectPos.X < MinSaturationRectPos.X)
                RectPos.X = MinSaturationRectPos.X;
            if (RectPos.Y < MinSaturationRectPos.Y)
                RectPos.Y = MinSaturationRectPos.Y;
            if (RectPos.X >= MaxRectPosX)
                RectPos.X = MaxRectPosX;
            if (RectPos.Y >= MaxRectPosY)
                RectPos.Y = MaxRectPosY;

            Canvas.SetLeft(SaturationChooserRect, RectPos.X);
            Canvas.SetTop(SaturationChooserRect, RectPos.Y);

            Point PixelPos = new Point(Canvas.GetLeft(SaturationChooserRect) + (SaturationChooserRect.ActualWidth / 2), Canvas.GetTop(SaturationChooserRect) + (SaturationChooserRect.ActualHeight / 2));

            SelectedColorRect.Fill = new SolidColorBrush(GetPixelColor(SaturationRectCanvas, PixelPos));
            SaturationRectPoint = RectPos;
            OnSelectedColorChange(new ColorPickerEventArgs() { SelectedColor = SelectedColorRect.Fill });
            Log.Info("Mouse Poistion = " + MousePos);
            Log.Info("Rect pos = " + RectPos);

        }

        private void GetPointSaturation()
        {
            Point PixelPos = new Point(Canvas.GetLeft(SaturationChooserRect) + (SaturationChooserRect.Width / 2), Canvas.GetTop(SaturationChooserRect) + (SaturationChooserRect.Height / 2));

            SelectedColorRect.Fill = new SolidColorBrush(GetPixelColor(SaturationRectCanvas, PixelPos));
            OnSelectedColorChange(new ColorPickerEventArgs() { SelectedColor = SelectedColorRect.Fill });
        }

        private void ConfirmBtnClick(object sender, RoutedEventArgs e)
        {
            OnConfirmButtonClick(new ColorPickerEventArgs() { SelectedColor = SelectedColorRect.Fill });
        }
        #region Get Pixel Color
        private Color GetPixelColor(Canvas iCanvas, Point pos)
        {
            var array = new byte[4];
            try
            {
                Canvas canvas = iCanvas;
                Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);

                //Measure and arrange the surface
                canvas.Measure(size);
                canvas.Arrange(new Rect(size));

                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                bitmap.Render(canvas);
                int posX = Convert.ToInt32(pos.X);
                int posY = Convert.ToInt32(pos.Y);
                Log.Info($"Pos x: {posX} and Pos y: {posY}");

                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using (MemoryStream outStream = new MemoryStream())
                {
                    encoder.Save(outStream);

                    System.Drawing.Bitmap bitmap1 = new System.Drawing.Bitmap(outStream);
                    System.Drawing.Color color = bitmap1.GetPixel(posX, posY);
                    array[0] = color.A;
                    array[1] = color.R;
                    array[2] = color.G;
                    array[3] = color.B;
                }
                //using (FileStream fileStream = new FileStream("D:/canvas.bmp", FileMode.OpenOrCreate))
                //{
                //    encoder.Save(fileStream);
                //}
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            //if (array[1] == 0 && array[2] == 0 && array[3] == 0)
            //{
            //    array[1] = 255;
            //    array[2] = 0;
            //    array[3] = 0;
            //}
            array[0] = 255;

            Color pixelColor = Color.FromArgb(array[0], array[1], array[2], array[3]);
            SelectedColorRect.Fill = new SolidColorBrush(pixelColor);
            //Set the color value
            ColorValueTextBox.Text = SelectedColorRect.Fill.ToString();
            return pixelColor;

        }
        #endregion
        public event EventHandler SelectedColorChange;

        protected virtual void OnSelectedColorChange(ColorPickerEventArgs e)
        {
            EventHandler handler = SelectedColorChange;
            handler?.Invoke(this, e);
        }

        public event EventHandler ConfirmButtonClick;

        protected virtual void OnConfirmButtonClick(ColorPickerEventArgs e)
        {
            EventHandler handler = ConfirmButtonClick;
            handler?.Invoke(this, e);
        }
        private delegate void ThresholdReachedEventHandler(object sender, EventArgs e);

    }


    public class ColorPickerEventArgs : EventArgs
    {
        public Brush SelectedColor { get; set; }
    }
}
