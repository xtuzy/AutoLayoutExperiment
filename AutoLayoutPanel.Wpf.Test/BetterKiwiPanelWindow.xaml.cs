using BetterKiwiLayout;
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
using Xamarin.Helper.Tools;

namespace AutoLayoutPanel.Wpf.Test
{
    /// <summary>
    /// BetterKiwiPanelWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BetterKiwiPanelWindow : Window
    {
        public BetterKiwiPanelWindow()
        {
            InitializeComponent();
            Debug.WriteLine(GetHashCode());
            Debug.WriteLine(GetHashCode());
            Debug.WriteLine(GetHashCode());
            First = new BetterKiwiLayoutPanel();
            First.Background = new SolidColorBrush(Colors.Cyan);
            this.Content = First;
            //SetFirst();
            setupAutoLayoutNestedLayout();
        }

        public BetterKiwiLayoutPanel First { get; }

        private void SetFirst()
        {
            var first = new Button() { Content = "*" };
            First.Children.Add(first);
            First.AddLayoutConstraint(first, NSLayoutAttribute.Top, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 0.5, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Left, 1, 10)
                .AddLayoutConstraint(first, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(first, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0, 20);
            var second = new Button() { Content = "**" };
            First.Children.Add(second);
            First.AddLayoutConstraint(second, NSLayoutAttribute.Top, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 0.5, 0)
                .AddLayoutConstraint(second, NSLayoutAttribute.Left, NSLayoutRelation.Equal, first, NSLayoutAttribute.Right, 1, 10)
                .AddLayoutConstraint(second, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(second, NSLayoutAttribute.Height, NSLayoutRelation.Equal, first, NSLayoutAttribute.Height, 2, 0);
            var third = new Button() { Content = "***" };
            First.Children.Add(third);
            First.AddLayoutConstraint(third, NSLayoutAttribute.Top, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 0.5, 0)
                .AddLayoutConstraint(third, NSLayoutAttribute.Left, NSLayoutRelation.Equal, second, NSLayoutAttribute.Right, 1, 10)
                .AddLayoutConstraint(third, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(third, NSLayoutAttribute.Height, NSLayoutRelation.Equal, second, NSLayoutAttribute.Height, 2, 0);
            First.UpdateLayout();

            var b1 = new Button() { Content = "BetterKiwi" };
            First.Children.Add(b1);
            First.AddLayoutConstraint(b1, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1, 0)
                .AddLayoutConstraint(b1, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterX, 1, 0)
                 .AddLayoutConstraint(b1, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0.5, 0)
                .AddLayoutConstraint(b1, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0.333, 0);
        }

        public static int ViewIndex=0;

        int viewCount = 500;
        /// <summary>
        /// 测试嵌套
        /// </summary>
        void setupAutoLayoutNestedLayout()
        {
            //PerformanceTestHelper.CreateTest();
            //Xamarin.Helper.Tools.RecordTimeHelper.StartRecordTime();
            var self = First;
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
                ViewIndex = i;

                Canvas view = new Canvas();
                self.Children.Add(view);
                if (i >= viewCount - 5)
                {
                    Xamarin.Helper.Tools.RecordTimeHelper.RecordTime($"Before Add {i} Child Constraint");
                    PerformanceTestHelper.StartRecord();
                    self.AddLayoutConstraint(view, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Bottom, 1, 0);
                    PerformanceTestHelper.OutputRecord(i.ToString());
                    self.AddLayoutConstraint(view, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.CenterX, 1, 1)
                .AddLayoutConstraint(view, NSLayoutAttribute.Width, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Width, 1, 0)
               .AddLayoutConstraint(view, NSLayoutAttribute.Height, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Height, 1, 1);
                    Xamarin.Helper.Tools.RecordTimeHelper.RecordTime($"After Add {i} Child Constraint");
                }
                else if (i <= 5)
                {
                    Xamarin.Helper.Tools.RecordTimeHelper.RecordTime($"Before Add {i} Child Constraint");
                    PerformanceTestHelper.StartRecord();
                    self.AddLayoutConstraint(view, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Bottom, 1, 0);
                    PerformanceTestHelper.OutputRecord(i.ToString());
                    self.AddLayoutConstraint(view, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.CenterX, 1, 1)
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
