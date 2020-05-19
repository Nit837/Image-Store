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

[assembly:ExportRenderer(typeof(CustomEntry),typeof(CustomEntriesRenderer))]
namespace ImageBhandaar.Droid.CustomControlRenderer
{
    public class CustomEntriesRenderer:EntryRenderer
    {
        public CustomEntriesRenderer(Context context):base(context)
        {
            AutoPackage = false;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
        }
    }
}