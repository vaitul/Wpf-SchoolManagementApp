using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace TestApp
{
    /// <summary>
    /// Class to host nornmal and Gif images and plays the animation.
    /// To make it work, instead of setting ImageSource, set the AnimationSourcePath property.
    /// </summary>
    public class GifImage : Image
    {
        /// <summary>
        /// Dependency property to hold value for AnimationSourcePath.
        /// </summary>
        public static readonly DependencyProperty AnimationSourcePathProperty =
            DependencyProperty.Register(
            "AnimationSourcePath",
            typeof(string),
            typeof(GifImage),
            new UIPropertyMetadata(String.Empty, AnimationSourcePathPropertyChanged));

        /// <summary>
        /// Dependency property to hold value of integer animation timeline values for FrameIndex.
        /// </summary>
        private static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register(
            "FrameIndex",
            typeof(int),
            typeof(GifImage),
            new UIPropertyMetadata(0, new PropertyChangedCallback(ChangingFrameIndex)));

        /// <summary>
        /// Dependency property to hold value of integer animation rate which slows down animation speed if value is more than 1.
        /// </summary>
        private static readonly DependencyProperty FrameRefreshRateProperty =
            DependencyProperty.Register(
            "FrameRefreshRate",
            typeof(int),
            typeof(GifImage),
            new UIPropertyMetadata(1, AnimationSourcePathPropertyChanged));

        /// <summary>
        /// Member to hold animation timeline for integer values.
        /// </summary>
        private Int32Animation anim;

        /// <summary>
        /// Member to hold flag to indicate if animation is working.
        /// </summary>
        private bool animationIsWorking = false;

        /// <summary>
        /// Member to hold Gif Bitmap Decoder.
        /// </summary>
        private GifBitmapDecoder gf;

        /// <summary>
        /// Initializes a new instance of the GifImage class.
        /// </summary>
        public GifImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the GifImage class based on the uri.
        /// </summary>
        /// <param name="uri">Uri of the image source.</param>
        public GifImage(Uri uri)
        {
            GifImage.SetupAnimationSource(this, uri);
        }

        /// <summary>
        /// Gets or sets a value indicating AnimationSourcePath.
        /// </summary>
        public string AnimationSourcePath
        {
            get { return (string)GetValue(AnimationSourcePathProperty); }
            set { SetValue(AnimationSourcePathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating FrameIndex.
        /// </summary>
        public int FrameIndex
        {
            get { return (int)GetValue(FrameIndexProperty); }
            set { SetValue(FrameIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for frame refresh rate. A value more than 1 would slow the animation down.
        /// </summary>
        public int FrameRefreshRate
        {
            get { return (int)GetValue(FrameRefreshRateProperty); }
            set { SetValue(FrameRefreshRateProperty, value); }
        }

        /// <summary>
        /// Method to handle property changed event of AnimationSourcePath property.
        /// </summary>
        /// <param name="obj">Source image.</param>
        /// <param name="ev">Event arguments.</param>
        protected static void AnimationSourcePathPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
        {
            GifImage ob = obj as GifImage;
            ob.BeginAnimation(GifImage.FrameIndexProperty, null);
            ob.anim = null;
            ob.gf = null;
            ob.Source = null;

            if (!String.IsNullOrEmpty(ob.AnimationSourcePath) &&
                Uri.IsWellFormedUriString(ob.AnimationSourcePath, UriKind.RelativeOrAbsolute))
            {
                if (((string)ob.AnimationSourcePath).ToLower().EndsWith(".gif"))
                {
                    Uri uri = new Uri(ob.AnimationSourcePath);
                    GifImage.SetupAnimationSource(ob, uri);
                    ob.BeginAnimation(GifImage.FrameIndexProperty, ob.anim);
                }
                else
                {
                    ob.Source = (new ImageSourceConverter()).ConvertFromString(ob.AnimationSourcePath) as ImageSource;
                    ob.InvalidateVisual();
                }

                ob.animationIsWorking = true;
            }
        }

        /// <summary>
        /// Method to handle property changed event of FrameIndex property.
        /// </summary>
        /// <param name="obj">Source image.</param>
        /// <param name="ev">Event arguments.</param>
        protected static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
        {
            GifImage ob = obj as GifImage;
            ob.Source = ob.gf.Frames[ob.FrameIndex];
            ob.InvalidateVisual();
        }

        /// <summary>
        /// Method to setup animation source against a Gif Image.
        /// </summary>
        /// <param name="ob">Gif image.</param>
        /// <param name="uri">Uri of the gif image source.</param>
        protected static void SetupAnimationSource(GifImage ob, Uri uri)
        {
            ob.gf = new GifBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            double val = (ob.gf.Frames.Count / 15.0) - (ob.gf.Frames.Count / 15);
            TimeSpan tmpSpn = new TimeSpan(0, 0, 0, ob.gf.Frames.Count / 15, (int)(val * 1000));
            Duration durtn = new Duration(new TimeSpan(tmpSpn.Ticks * ob.FrameRefreshRate));
            ob.anim = new Int32Animation(0, ob.gf.Frames.Count - 1, durtn);
            ob.anim.RepeatBehavior = RepeatBehavior.Forever;
            ob.Source = ob.gf.Frames[0];
        }

        /// <summary>
        /// Method to override the OnRender event of the image.
        /// </summary>
        /// <param name="dc">Drawing Context of the image.</param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (!this.animationIsWorking)
            {
                BeginAnimation(GifImage.FrameIndexProperty, this.anim);
                this.animationIsWorking = true;
            }
        }
    }
}