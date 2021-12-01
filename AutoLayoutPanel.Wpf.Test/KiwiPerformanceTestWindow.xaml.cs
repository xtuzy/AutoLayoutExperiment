using KiwiLayout;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace AutoLayoutPanel.Wpf.Test
{
    /// <summary>
    /// KiwiPerformanceTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class KiwiPerformanceTestWindow : Window
    {
        public KiwiPerformanceTestWindow()
        {
            InitializeComponent();
            setupAutoLayoutNestedLayout();
        }


        int viewCount = 100;
        /// <summary>
        /// 测试嵌套
        /// </summary>
        void setupAutoLayoutNestedLayout()
        {
            Xamarin.Helper.Tools.RecordTimeHelper.StartRecordTime();
            this.Content = new KiwiLayoutPanel();
            var self = Content as KiwiLayoutPanel;
            Canvas previousView = new Canvas();
            previousView.Background = new SolidColorBrush(Colors.AliceBlue);
            Xamarin.Helper.Tools.RecordTimeHelper.RecordTime("Before Add First Child");
            self.Children.Add(previousView);
            Xamarin.Helper.Tools.RecordTimeHelper.RecordTime("Before Add First Child Constraint");
            self.AddLayoutConstraint(previousView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, self, NSLayoutAttribute.Bottom, 1, 0);
            self.AddLayoutConstraint(previousView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, self, NSLayoutAttribute.Left, 1, 10);
            self.AddLayoutConstraint(previousView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20);
            self.AddLayoutConstraint(previousView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, self, NSLayoutAttribute.Height, 0.1, 0);
            Xamarin.Helper.Tools.RecordTimeHelper.RecordTime("After Add First Child Constraint");
            for (var i = 0; i < viewCount; i++)
            {
                Canvas view = new Canvas();
                self.Children.Add(view);
                if (i >= viewCount - 5)
                {
                    Xamarin.Helper.Tools.RecordTimeHelper.RecordTime($"Before Add {i} Child Constraint");
                    self.AddLayoutConstraint(view, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Bottom, 1, 0)
                   .AddLayoutConstraint(view, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.CenterX, 1, 1)
                    .AddLayoutConstraint(view, NSLayoutAttribute.Width, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Width, 1, 0)
                   .AddLayoutConstraint(view, NSLayoutAttribute.Height, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Height, 1, 1);
                    Xamarin.Helper.Tools.RecordTimeHelper.RecordTime($"After Add {i} Child Constraint");
                }
                else if (i <= 5)
                {
                    Xamarin.Helper.Tools.RecordTimeHelper.RecordTime($"Before Add {i} Child Constraint");
                    self.AddLayoutConstraint(view, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Bottom, 1, 0)
                   .AddLayoutConstraint(view, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.CenterX, 1, 1)
                    .AddLayoutConstraint(view, NSLayoutAttribute.Width, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Width, 1, 0)
                   .AddLayoutConstraint(view, NSLayoutAttribute.Height, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Height, 1, 1);
                    Xamarin.Helper.Tools.RecordTimeHelper.RecordTime($"After Add {i} Child Constraint");
                }
                else
                {
                    self.AddLayoutConstraint(view, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Bottom, 1, 0)
                   .AddLayoutConstraint(view, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.CenterX, 1, 1)
                    .AddLayoutConstraint(view, NSLayoutAttribute.Width, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Width, 1, 0)
                   .AddLayoutConstraint(view, NSLayoutAttribute.Height, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Height, 1, 1);

                }
                view.Background = new SolidColorBrush(RandomColor());
                previousView = view;
            }
            Xamarin.Helper.Tools.RecordTimeHelper.RecordTime("After Add All");
        }

        ///测试独立
       /* void setupIndependentLayout()
        {
            for (NSUInteger i = 0; i < viewCount; i++)
            {
                UIView* view = [[UIView alloc] init];
                view.translatesAutoresizingMaskIntoConstraints = NO;
                [self addSubview:view];
                CGFloat x = [self randomNumber] * 2 + 0.0000001;
                CGFloat y = [self randomNumber] * 2 + 0.0000001;
                [self addConstraint:[NSLayoutConstraint constraintWithItem:view attribute:NSLayoutAttributeCenterX relatedBy:NSLayoutRelationEqual toItem:self attribute:NSLayoutAttributeCenterX multiplier:x constant:0]];
                [self addConstraint:[NSLayoutConstraint constraintWithItem:view attribute:NSLayoutAttributeCenterY relatedBy:NSLayoutRelationEqual toItem:self attribute:NSLayoutAttributeCenterY multiplier:y constant:0]];
                [self addConstraint:[NSLayoutConstraint constraintWithItem:view attribute:NSLayoutAttributeWidth relatedBy:NSLayoutRelationEqual toItem:nil attribute:NSLayoutAttributeNotAnAttribute multiplier:1 constant: 20]];
                [self addConstraint:[NSLayoutConstraint constraintWithItem:view attribute:NSLayoutAttributeHeight relatedBy:NSLayoutRelationEqual toItem:nil attribute:NSLayoutAttributeNotAnAttribute multiplier:1 constant: 20]];
                view.backgroundColor = [self randomColor];
            }
        }*/

        Color RandomColor()
        {
            Random ran = new Random();
            int r = ran.Next(0, 255);
            int g = ran.Next(0, 255);
            int b = ran.Next(0, 255);
            return Color.FromRgb((byte)r, (byte)g, (byte)b);
        }
    }
}
