using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoLayout.Droid.Test
{
    [Activity(Label = "ConstraintLayoutPerformanceTestActivity")]
    public class ConstraintLayoutPerformanceTestActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            content = new ConstraintLayout(this) { Id = View.GenerateViewId() };

            SetContentView(content);

            SetupDefaultAutoLayoutNestedLayout();
        }

        int viewCount = 10;

        public ConstraintLayout content { get; private set; }

        void SetupDefaultAutoLayoutNestedLayout()
        {
            var Page = content;
            Page.SetBackgroundColor(Color.White);
            var self = Page;
            View previousView = new View(this) { Id = View.GenerateViewId() };
            previousView.SetBackgroundColor(Color.Cyan);
            previousView.LayoutParameters = new ConstraintLayout.LayoutParams(0, 0);
            self.AddView(previousView);
            var set = new ConstraintSet();
            set.Clone(Page);
            set.Connect(previousView.Id, ConstraintSet.Bottom, Page.Id, ConstraintSet.Bottom);
            set.Connect(previousView.Id, ConstraintSet.Left, Page.Id, ConstraintSet.Left, 10);
            set.ConstrainWidth(previousView.Id, 20);
            set.ConstrainHeight(previousView.Id, 10);

            for (var i = 0; i < viewCount; i++)
            {
                View view = new View(this) { Id = View.GenerateViewId() };
                view.LayoutParameters = new ConstraintLayout.LayoutParams(0, 0);
                self.AddView(view);
                set.Connect(view.Id, ConstraintSet.Bottom, previousView.Id, ConstraintSet.Bottom);
                set.Connect(view.Id, ConstraintSet.Left, previousView.Id, ConstraintSet.Left,1);
                set.ConstrainWidth(view.Id, previousView.Id);
                set.ConstrainHeight(view.Id, previousView.Id);


                if (i == viewCount - 1)
                    view.SetBackgroundColor(Color.Blue);
                else
                    view.SetBackgroundColor(RandomColor());
                previousView = view;
            }
            set.ApplyTo(Page);
            Page.Invalidate();
        }

        Color RandomColor()
        {
            Random ran = new Random();
            int r = ran.Next(0, 255);
            int g = ran.Next(0, 255);
            int b = ran.Next(0, 255);
            return Color.Rgb((byte)r, (byte)g, (byte)b);
        }
    }
}