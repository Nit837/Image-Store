using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ImageBhandaar.Controls;
using ImageBhandaar.Droid.CustomControlRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(CustomFrame),typeof(CustomFramesRenderer))]
namespace ImageBhandaar.Droid.CustomControlRenderer
{
    public class CustomFramesRenderer:FrameRenderer
    {
        public CustomFramesRenderer()
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && e.OldElement == null)
            {
                this.SetBackgroundResource(Resource.Drawable.FrameRenderValue);
                GradientDrawable drawable = (GradientDrawable)this.Background;
                drawable.SetColor(Android.Graphics.Color.ParseColor("#F0F0F0"));
            }
        }
    }
}