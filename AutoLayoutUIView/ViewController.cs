using AutoLayoutTest;
using Foundation;
using System;
using UIKit;
using NSLayoutAttribute = AutoLayoutTest.NSLayoutAttribute;
using NSLayoutRelation = AutoLayoutTest.NSLayoutRelation;

namespace AutoLayoutUIView
{
    public partial class ViewController : UIViewController
    {
        public UIButton LeftButton { get; private set; }
        public AutoLayoutPanel panel { get; private set; }

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            panel = new AutoLayoutPanel();
            View = panel;
            View.BackgroundColor = UIColor.White;
            LeftButton = new UIButton(UIButtonType.RoundedRect) { };
            LeftButton.SetTitle("Left", UIControlState.Normal);
            LeftButton.TouchUpInside += LeftButton_TouchUpInside;
            LeftButton.BackgroundColor = UIColor.Brown;
            panel.AddSubview(LeftButton);
            panel.AddLayoutConstraint(LeftButton, "Center", "=", panel, "Center", 1, 0);
            panel.AddLayoutConstraint(LeftButton, "Top", "=", panel, "Top", 1, 200);
            panel.AddLayoutConstraint(LeftButton, "Height", "=", null, "Height", 1, 100);
            panel.AddLayoutConstraint(LeftButton, "Width", "=", panel, "Width", 0.5, 0);
        }

        bool isCons = true;
        private void LeftButton_TouchUpInside(object sender, EventArgs e)
        {
            UIButton Right_LeftButton = new UIButton(UIButtonType.RoundedRect);
            Right_LeftButton.SetTitle("Right_Left", UIControlState.Normal);
            panel.AddSubview(Right_LeftButton);
            panel.AddLayoutConstraint(Right_LeftButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, LeftButton, NSLayoutAttribute.CenterY)
                .AddLayoutConstraint(Right_LeftButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, LeftButton, NSLayoutAttribute.Right, 1, 10)
                .AddLayoutConstraint(Right_LeftButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 1, 100)
                .AddLayoutConstraint(Right_LeftButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 1, 50);
            Right_LeftButton.TouchUpInside += (se, ex) =>
            {
                setupAutoLayoutPanelNestedLayout();
                //SetupDefaultAutoLayoutNestedLayout();
            };
            
            UIButton Bottom_LeftButton = new UIButton(UIButtonType.RoundedRect);
            Bottom_LeftButton.SetTitle("Bottom_Left", UIControlState.Normal);
            panel.AddSubview(Bottom_LeftButton);
            panel.AddLayoutConstraint(Bottom_LeftButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, LeftButton, NSLayoutAttribute.CenterX)
                .AddLayoutConstraint(Bottom_LeftButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, LeftButton, NSLayoutAttribute.Bottom, 1, 10)
                .AddLayoutConstraint(Bottom_LeftButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 1, 100)
                .AddLayoutConstraint(Bottom_LeftButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 1, 50);
            Bottom_LeftButton.TouchUpInside += (s, ea) =>//TODO: Determine if target controls need to be in Controls, ControlVariables, VarContraints
            {
                if (isCons)
                {
                    panel.RemoveLayoutConstraint(Right_LeftButton);
                    isCons = false;
                }
                else
                {
                    panel.AddLayoutConstraint(Right_LeftButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, LeftButton, NSLayoutAttribute.CenterY)
                .AddLayoutConstraint(Right_LeftButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, LeftButton, NSLayoutAttribute.Right, 1, 10)
                .AddLayoutConstraint(Right_LeftButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 1, 100)
                .AddLayoutConstraint(Right_LeftButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 1, 50);
                    isCons = true;
                }
                //panel.UpdateLayout();
                panel.SetNeedsLayout();
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        int viewCount = 500;
        /// <summary>
        /// 测试嵌套
        /// </summary>
        void setupAutoLayoutPanelNestedLayout()
        {
            View = new AutoLayoutPanel();
            View.BackgroundColor = UIColor.White;
            var self = View as AutoLayoutPanel;
            UIView previousView = new UIView();
            previousView.BackgroundColor = UIColor.Cyan;
            self.AddSubview(previousView);
            self.AddLayoutConstraint(previousView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, self, NSLayoutAttribute.Bottom, 1, 0)
                .AddLayoutConstraint(previousView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, self, NSLayoutAttribute.Left, 1, 10)
                 .AddLayoutConstraint(previousView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(previousView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, self, NSLayoutAttribute.Height, 0, 10);
            for (var i = 0; i < viewCount; i++)
            {
                UIView view = new UIView();
                self.AddSubview(view);

                self.AddLayoutConstraint(view, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Bottom, 1, 0)
               .AddLayoutConstraint(view, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.CenterX, 1, 1)
                .AddLayoutConstraint(view, NSLayoutAttribute.Width, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Width, 1, 0)
               .AddLayoutConstraint(view, NSLayoutAttribute.Height, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Height, 1, 1);
                view.BackgroundColor = RandomColor();
                previousView = view;
            }
            View.SetNeedsLayout();
        }

        void SetupDefaultAutoLayoutNestedLayout()
        {
            View = new UIView();
            View.BackgroundColor = UIColor.White;
            var self = View;
            UIView previousView = new UIView() { TranslatesAutoresizingMaskIntoConstraints=false};
            previousView.BackgroundColor = UIColor.Cyan;
            self.AddSubview(previousView);
            previousView.BottomAnchor.ConstraintEqualTo(self.BottomAnchor).Active = true;
            previousView.LeftAnchor.ConstraintEqualTo(self.LeftAnchor,10).Active = true;
            previousView.WidthAnchor.ConstraintEqualTo(20).Active = true;
            previousView.HeightAnchor.ConstraintEqualTo(10).Active = true;
          
            for (var i = 0; i < viewCount; i++)
            {
                UIView view = new UIView() { TranslatesAutoresizingMaskIntoConstraints = false };
                self.AddSubview(view);
                view.BottomAnchor.ConstraintEqualTo(previousView.BottomAnchor).Active = true;
                view.CenterXAnchor.ConstraintEqualTo(previousView.CenterXAnchor,1).Active = true;
                view.WidthAnchor.ConstraintEqualTo(previousView.WidthAnchor).Active = true;
                view.HeightAnchor.ConstraintEqualTo(previousView.HeightAnchor,1,1).Active = true;
                
                view.BackgroundColor = RandomColor();
                previousView = view;
            }
            View.SetNeedsLayout();
        }
        UIColor RandomColor()
        {
            Random ran = new Random();
            int r = ran.Next(0, 255);
            int g = ran.Next(0, 255);
            int b = ran.Next(0, 255);
            return UIColor.FromRGB((byte)r, (byte)g, (byte)b);
        }
    }
}