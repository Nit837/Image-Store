﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ImageBhandaar.Controls;
using ImageBhandaar.Droid.CustomControlRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EditorControl), typeof(EditorControlRenderer))]
namespace ImageBhandaar.Droid.CustomControlRenderer
{
    class EditorControlRenderer:EditorRenderer
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public EditorControlRenderer()
        {
        }
#pragma warning restore CS0618 // Type or member is obsolete

        public EditorControlRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            Control.Background.SetColorFilter(Android.Graphics.Color.Transparent, PorterDuff.Mode.SrcIn);
        }
    }
}