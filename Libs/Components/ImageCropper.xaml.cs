using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LifeInDiary.Libs.Components
{
    /// <summary>
    /// Interaction logic for ImageCropper.xaml
    /// </summary>
    public partial class ImageCropper : UserControl
    {
        private BitmapImage BTM_image = new BitmapImage();
        public string OutputFileLocation { get; set; }
        public Image CroppedImage { get; private set; }
        private float ImageScale = 1f;
        private double ImageScaleX = 1f;
        private double ImageScaleY = 1f;
        private bool isMouseDown = false;
        Point ToCropImageOffset = new Point();

        public string BitmapInputImagePath
        {
            set
            {
                try
                {
                    MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(value));
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();
                    BTM_image = bitmapImage;
                    ImageToCrop.Source = bitmapImage;

                    //ImageScrollViewer.Background = Brushes.Red;
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        public ImageCropper()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                CroppingContainer.MaxHeight = Row1.ActualHeight;
                CroppingContainer.MaxWidth = bg.ActualWidth;
                ImageToCrop.Source = BTM_image;
                Log.Info(ImageScale);
            };

            Canvas.SetLeft(ImageToCrop, 0);
            Canvas.SetTop(ImageToCrop, 0);
        }


        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        Point origin;
        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            origin = e.GetPosition(ImageToCrop);
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            //Log.Info("Mouse Move");
            if (isMouseDown && ImageScale > 1f)
            {
                Point CurrentMousePos = e.GetPosition(ImageToCrop);
                Point ImageOffset = ImageToCrop.TransformToVisual(ImageBorder).Transform(new Point(0, 0));
                ToCropImageOffset = ImageOffset;
                double MouseMovementX = CurrentMousePos.X - origin.X;
                double MouseMovementY = CurrentMousePos.Y - origin.Y;
                if (MouseMovementX > 0 && Canvas.GetLeft(ImageToCrop) <= 0)
                {
                    //translateTransform.X += Math.Abs(MouseMovementX);
                    double left = Canvas.GetLeft(ImageToCrop) + MouseMovementX;
                    Canvas.SetLeft(ImageToCrop, left);
                }
                if (MouseMovementX < 0)
                {
                    //translateTransform.X -= Math.Abs(MouseMovementX);
                    double right = Canvas.GetRight(ImageToCrop) - MouseMovementX;
                    Canvas.SetRight(ImageToCrop, right);
                }

                if (ImageOffset.X > 0)
                {

                }

                if (MouseMovementY > 0 && ImageOffset.Y <= 0)
                {
                    //translateTransform.Y += Math.Abs(MouseMovementY);
                }
                if (MouseMovementY < 0)
                {
                    //translateTransform.Y -= Math.Abs(MouseMovementY);
                }

                Log.Info("Image offset x: " + Canvas.GetLeft(ImageToCrop) + ", right: " + Canvas.GetRight(ImageToCrop));
                Log.Info("TranslateX value: " + translateTransform.X);

                Log.Info("Image Bounds: " + (ImageScaleX - Math.Abs(ImageOffset.X) - ImageBorder.ActualWidth));
            }
            else if (ImageScale <= 1f)
            {
                translateTransform.X = 0;
                translateTransform.Y = 0;
                ImageToCrop.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }
        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
            ReleaseMouseCapture();
        }
        private void ImageViewbox_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            ReleaseMouseCapture();
        }


        private void SaveCroppedImageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Point StartPoint = CropingRect.TransformToVisual(ImageToCrop).Transform(new Point(0, 0));
                Point ImagePoint = ImageToCrop.TransformToAncestor(ImageViewbox).Transform(new Point(0, 0));

                Log.Info("Point of rectangle from LEFT: " + StartPoint);

                double OriginalImageWidth = ImageToCrop.Source.Width;
                double OriginalImageHeight = ImageToCrop.Source.Height;

                double rectPointX = StartPoint.X / ImageScale;
                double rectPointY = StartPoint.Y / ImageScale;

                double CropingRectWidth = CropingRect.ActualWidth;
                double CropingRectHeight = CropingRect.ActualHeight;

                Point EndPoint = new Point(StartPoint.X + CropingRectWidth, StartPoint.Y + CropingRectHeight);


                Log.Info("Starting points: " + Convert.ToInt32(rectPointX) + ", " + Convert.ToInt32(rectPointY));
                Log.Info("End Points: " + EndPoint.X + ", " + EndPoint.Y);

                double virtualRectHeight = Math.Abs(CropingRect.ActualHeight / ImageScale);
                double virtualRectWidth = Math.Abs(CropingRect.ActualWidth / ImageScale);

                Log.Info("Rectangle size with respect to pic : " + virtualRectWidth + ", " + virtualRectHeight);
                Log.Info("Scaled size of image: " + ImageToCrop.ActualWidth + ", " + ImageToCrop.ActualHeight);
                Log.Info("Original size of pic: " + ImageToCrop.Source.Width + ", " + ImageToCrop.Source.Height);

                int x = Convert.ToInt32(rectPointX);
                int y = Convert.ToInt32(rectPointY);
                int width = Convert.ToInt32(virtualRectWidth);
                int height = Convert.ToInt32(virtualRectHeight);

                if (virtualRectWidth >= OriginalImageWidth || x < 0)
                {
                    if (virtualRectWidth >= OriginalImageWidth)
                        width = Convert.ToInt32(OriginalImageWidth);
                    if (x < 0)
                        x = 0;
                }
                if (virtualRectHeight >= OriginalImageHeight || y < 0)
                {
                    if (virtualRectHeight >= OriginalImageHeight)
                        height = Convert.ToInt32(OriginalImageHeight);
                    if (y < 0)
                        y = 0;
                }

                Log.Info("Cropped Image rect: " + new Int32Rect(x, y, width, height));
                BitmapSource src = ImageToCrop.Source as BitmapSource;
                CroppedBitmap croppedBitmap = new CroppedBitmap(src, new Int32Rect(x, y, width, height));


                PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                pngBitmapEncoder.Frames.Add(BitmapFrame.Create(croppedBitmap));
                FileStream fileStream = new FileStream(OutputFileLocation, FileMode.OpenOrCreate);
                pngBitmapEncoder.Save(fileStream);

                Log.Info("File saved in " + OutputFileLocation);
                HidePopup();
                fileStream.Close();
                OnImageSaved(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void ResizeImage(float ImageScale)
        {
            try
            {
                //ScaleX = (ImageScale / 100) * ImageToCrop.Source.Width;
                //ScaleY = (ImageScale / 100) * ImageToCrop.Source.Height;

                ImageToCrop.Width = ImageScaleX;
                ImageToCrop.Height = ImageScaleY;

                Log.Info("New image size: " + ImageScaleY + ", " + ImageScaleY);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        private void scaling_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point ImagePoint = ImageToCrop.TransformToAncestor(ImageViewbox).Transform(new Point(0, 0));
            ToCropImageOffset = ImagePoint;
            float MaxScale = 10f;
            float MinScale = 1f;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                //Point MousePos = Mouse.GetPosition(ImageViewbox);
                //double MousePosX = MousePos.X / ImageViewbox.ActualWidth;
                //double MousePosY = MousePos.Y / ImageViewbox.ActualHeight; 
                //Log.Info("Mouse pos: " + MousePosX + ", " + MousePosY);
                //ImageToCrop.RenderTransformOrigin = new Point(MousePosX,MousePosY);
                if (e.Delta > 0 && ImageScale <= MaxScale)
                {
                    ImageScale += 0.25f;
                }
                if (e.Delta < 0 && ImageScale >= MinScale)
                {
                    ImageScale -= 0.25f;
                }
                //ScaleTransform scaleTransform = transformGroup[0] as ScaleTransform

                ImageScaleX = ImageToCrop.Source.Width * ImageScale;
                ImageScaleY = ImageToCrop.Source.Height * ImageScale;

                ImageToCrop.Width = ImageScaleX;
                ImageToCrop.Height = ImageScaleY;
                Log.Info("Image scale size: " + ImageScaleX + ", " + ImageScaleY);
                if (ImageScale <= 1f)
                {
                    translateTransform.X = 0;
                    translateTransform.Y = 0;
                    ImageToCrop.RenderTransformOrigin = new Point(0.5, 0.5);
                }
            }
        }

        private void CloseImageCropperBtn_Click(object sender, RoutedEventArgs e)
        {
            HidePopup();
        }

        public void ShowPopup()
        {
            this.Visibility = Visibility.Visible;
        }
        private void HidePopup()
        {
            this.Visibility = Visibility.Collapsed;
        }

        public event EventHandler ImageSaved;

        protected virtual void OnImageSaved(EventArgs e)
        {
            EventHandler handler = ImageSaved;
            handler?.Invoke(this, e);
        }
        public delegate void ThresholdReachedEventHandler(object sender, ImageSavedEventArgs e);
    }
    public class ImageSavedEventArgs : EventArgs
    {
        public int OutputLocation { get; set; }
    }
}
//------------------------
/*               
               Point StartPoint = CropingRect.TransformToVisual(ImageToCrop).Transform(new Point(0, 0));
                Point ImagePoint = ImageToCrop.TransformToAncestor(ImageViewbox).Transform(new Point(0, 0));

                Log.Info("Point of rectangle from LEFT: " + StartPoint);

                double OriginalImageWidth = ImageToCrop.Source.Width;
                double OriginalImageHeight = ImageToCrop.Source.Height;

                double rectPointX = StartPoint.X / (ImageScale / 100);
                double rectPointY = StartPoint.Y / (ImageScale / 100);

                double CropingRectWidth = CropingRect.ActualWidth;
                double CropingRectHeight = CropingRect.ActualHeight;

                Point EndPoint = new Point(StartPoint.X + CropingRectWidth, StartPoint.Y + CropingRectHeight);


                Log.Info("Starting points: " + Convert.ToInt32(rectPointX) + ", " + Convert.ToInt32(rectPointY));
                Log.Info("End Points: " + EndPoint.X + ", " + EndPoint.Y);

                double virtualRectHeight = Math.Abs(CropingRect.ActualHeight / (ImageScale/ 100));
                double virtualRectWidth = Math.Abs(CropingRect.ActualWidth / (ImageScale / 100));

                Log.Info("Rectangle size with respect to pic : " + virtualRectWidth + ", " + virtualRectHeight);
                Log.Info("Scaled size of image: " + ImageToCrop.ActualWidth + ", " + ImageToCrop.ActualHeight);
                Log.Info("Original size of pic: " + ImageToCrop.Source.Width + ", " + ImageToCrop.Source.Height);

                int x = Convert.ToInt32(rectPointX);
                int y = Convert.ToInt32(rectPointY);
                int width = Convert.ToInt32(virtualRectWidth);
                int height = Convert.ToInt32(virtualRectHeight);

                if (virtualRectWidth >= OriginalImageWidth || x < 0)
                {
                    if (virtualRectWidth >= OriginalImageWidth)
                        width = Convert.ToInt32(OriginalImageWidth);
                    if (x < 0)
                        x = 0;
                }
                if (virtualRectHeight >= OriginalImageHeight || y < 0)
                {
                    if (virtualRectHeight >= OriginalImageHeight)
                        height = Convert.ToInt32(OriginalImageHeight);
                    if (y < 0)
                        y = 0;
                }

                Log.Info("Cropped Image rect: " + new Int32Rect(x, y, width, height));
                BitmapSource src = ImageToCrop.Source as BitmapSource;
                CroppedBitmap croppedBitmap = new CroppedBitmap(src, new Int32Rect(x, y, width, height));


                PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                pngBitmapEncoder.Frames.Add(BitmapFrame.Create(croppedBitmap));
                FileStream fileStream = new FileStream(OutputFileLocation, FileMode.OpenOrCreate);
                pngBitmapEncoder.Save(fileStream);

                Log.Info("File saved in " + OutputFileLocation);
                HidePopup();
                fileStream.Close();
                OnImageSaved(EventArgs.Empty);
*/