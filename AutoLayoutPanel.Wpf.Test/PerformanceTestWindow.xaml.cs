using AutoLayout;
using System;
using System.Collections.Generic;
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
    /// PerformanceTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PerformanceTestWindow : Window
    {
        public PerformanceTestWindow()
        {
            InitializeComponent();
            
            setupAutoLayoutNestedLayout();
            //setupCanvasNestedLayout();
        }


        int viewCount = 50;
        /// <summary>
        /// 测试嵌套
        /// </summary>
        void setupAutoLayoutNestedLayout()
        {
            this.Content = new AutoLayout.AutoLayoutPanel();
            var self = Content as AutoLayout.AutoLayoutPanel;
            Canvas previousView = new Canvas();
            previousView.Background = new SolidColorBrush(Colors.AliceBlue);
            self.Children.Add(previousView);
            self.AddLayoutConstraint(previousView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, self, NSLayoutAttribute.Bottom , 1, 0)
                .AddLayoutConstraint(previousView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, self, NSLayoutAttribute.Left, 1, 10)
                 .AddLayoutConstraint(previousView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(previousView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, self, NSLayoutAttribute.Height, 0.1, 0);
            for (var i = 0; i < viewCount; i++)
            {
                Canvas view = new Canvas();
                self.Children.Add(view);

                self.AddLayoutConstraint(view, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Bottom, 1, 0)
               .AddLayoutConstraint(view, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.CenterX, 1, 1)
                .AddLayoutConstraint(view, NSLayoutAttribute.Width, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Width, 1, 0)
               .AddLayoutConstraint(view, NSLayoutAttribute.Height, NSLayoutRelation.Equal, previousView, NSLayoutAttribute.Height, 1, 1);
                view.Background = new SolidColorBrush(RandomColor());
                previousView = view;
            }
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

        void setupCanvasNestedLayout()
        {
            this.Content = new Canvas();
            var self = Content as Canvas;
            self.Background = new SolidColorBrush(Colors.AliceBlue);
            var button = new Button() { Content = "Canvas"};
            Canvas.SetLeft(button, 100);
            Canvas.SetTop(button, 100);
            button.Width = 100;
            button.Height = 100;
            self.Children.Add(button);
            
            button.Click += (se, e) =>
            {
                Canvas previousView = new Canvas();
                previousView.Background = new SolidColorBrush(Colors.AliceBlue);
                previousView.Width = 20;
                previousView.Height = self.ActualWidth * 0.1;
                previousView.Margin = new Thickness(10, self.ActualHeight-10, 0, 0);
                self.Children.Add(previousView);
                for (var i = 0; i < viewCount; i++)
                {
                    Canvas view = new Canvas();
                    view.Height = previousView.Height + 1;
                    view.Width = previousView.Width;
                    view.Margin = new Thickness(previousView.Margin.Left + 1, previousView.Margin.Top + previousView.Height - view.Height, 0, 0);
                    view.Background = new SolidColorBrush(RandomColor());
                    self.Children.Add(view);
                    previousView = view;
                }
            };


            
        }

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
