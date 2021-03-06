﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImageBhandaar.Droid
{
    [Activity(Theme = "@style/Theme.Splash", //Indicates the theme to use for this activity
          MainLauncher = true, //Set it as boot activity
          NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() =>
            {
                SimulateStartup();
            });

            startupWork.Start();

        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            var currentconnection= Connectivity.NetworkAccess;
            if (currentconnection == NetworkAccess.Internet)
            {
                await Task.Delay(2000); // Simulate a bit of startup work.
                StartActivity(new Intent(Android.App.Application.Context, typeof(MainActivity)));
            }
            else if (currentconnection == NetworkAccess.Local)
            {
                return;
            }
            else
            {
                return;
            }
        }
    }
}