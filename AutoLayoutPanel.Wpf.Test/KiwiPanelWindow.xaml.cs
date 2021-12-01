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
using KiwiLayout;

namespace AutoLayoutPanel.Wpf.Test
{
    /// <summary>
    /// KiwiPanelWindow.xaml 的交互逻辑
    /// </summary>
    public partial class KiwiPanelWindow : Window
    {
        public KiwiPanelWindow()
        {
            InitializeComponent();

            SetFirst();
            SetSecond();
            SetThird();
            SetFourth();
            SetFifth();
            SetNinth();

            SetSeventh();
        }

        private void SetSeventh()
        {
            var First = Seventh;
            var first = new Button() { Content = "Kiwi" };
            First.Children.Add(first);
            First.AddLayoutConstraint(first, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterX, 1, 0)
                 .AddLayoutConstraint(first, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0.5, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0.333, 0);
            first.Click += (sender, e) =>
            {
                new KiwiPanelWindow().Show();
            };
        }

        private void SetNinth()
        {
            var First = Ninth;
            var first = new Button() { Content = "TestPerformance" };
            First.Children.Add(first);
            First.AddLayoutConstraint(first, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterX, 1, 0)
                 .AddLayoutConstraint(first, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0.5, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0.333, 0);
            first.Click += (sender, e) =>
            {
                new KiwiPerformanceTestWindow().Show();
            };
        }

        private void SetFifth()
        {
            var First = Fifth;
            var first = new Button() { Content = "1" };
            First.Children.Add(first);
            First.AddLayoutConstraint(first, NSLayoutAttribute.Top, NSLayoutRelation.Equal, First, NSLayoutAttribute.Top, 1, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Left, 1, 0)
                 .AddLayoutConstraint(first, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0.5, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0.333, 0);
            var second = new Button() { Content = "2" };
            First.Children.Add(second);
            First.AddLayoutConstraint(second, NSLayoutAttribute.Top, NSLayoutRelation.Equal, First, NSLayoutAttribute.Top, 1, 0)
                .AddLayoutConstraint(second, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Right, 0.5, 0)
                 .AddLayoutConstraint(second, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0.5, 0)
                .AddLayoutConstraint(second, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0.333, 0);

            var third = new Button() { Content = "3" };
            First.Children.Add(third);
            First.AddLayoutConstraint(third, NSLayoutAttribute.Top, NSLayoutRelation.Equal, first, NSLayoutAttribute.Bottom, 1, 0)
                .AddLayoutConstraint(third, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Left, 1, 0)
                .AddLayoutConstraint(third, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0.333, 0)
                .AddLayoutConstraint(third, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0.333, 0);
            var fourth = new Button() { Content = "4" };
            First.Children.Add(fourth);
            First.AddLayoutConstraint(fourth, NSLayoutAttribute.Top, NSLayoutRelation.Equal, first, NSLayoutAttribute.Bottom, 1, 0)
                .AddLayoutConstraint(fourth, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Right, 0.333, 0)
                 .AddLayoutConstraint(fourth, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0.333, 0)
                .AddLayoutConstraint(fourth, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0.333, 0);
            var fifth = new Button() { Content = "5" };
            First.Children.Add(fifth);
            First.AddLayoutConstraint(fifth, NSLayoutAttribute.Top, NSLayoutRelation.Equal, first, NSLayoutAttribute.Bottom, 1, 0)
                .AddLayoutConstraint(fifth, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Right, 0.666, 0)
                 .AddLayoutConstraint(fifth, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0.333, 0)
                .AddLayoutConstraint(fifth, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0.333, 0);
        }

        private void SetFourth()
        {
            var First = Fourth;
            var first = new Button() { Content = "1" };
            First.Children.Add(first);
            First.AddLayoutConstraint(first, NSLayoutAttribute.Top, NSLayoutRelation.Equal, First, NSLayoutAttribute.Top, 1, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Left, 1, 0)
                 .AddLayoutConstraint(first, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(first, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 0, 20);
            var second = new Button() { Content = "2" };
            First.Children.Add(second);
            First.AddLayoutConstraint(second, NSLayoutAttribute.Top, NSLayoutRelation.Equal, First, NSLayoutAttribute.Top, 1, 0)
                .AddLayoutConstraint(second, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterX, 1, 0)
                 .AddLayoutConstraint(second, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(second, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 0, 20);
            var third = new Button() { Content = "3" };
            First.Children.Add(third);
            First.AddLayoutConstraint(third, NSLayoutAttribute.Top, NSLayoutRelation.Equal, First, NSLayoutAttribute.Top, 1, 0)
                .AddLayoutConstraint(third, NSLayoutAttribute.Right, NSLayoutRelation.Equal, First, NSLayoutAttribute.Right, 1, 0)
                .AddLayoutConstraint(third, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(third, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 0, 20);
            var fourth = new Button() { Content = "4" };
            First.Children.Add(fourth);
            First.AddLayoutConstraint(fourth, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1, 0)
                .AddLayoutConstraint(fourth, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Left, 1, 0)
                 .AddLayoutConstraint(fourth, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(fourth, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 0, 20);
            var fifth = new Button() { Content = "5" };
            First.Children.Add(fifth);
            First.AddLayoutConstraint(fifth, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1, 0)
                .AddLayoutConstraint(fifth, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterX, 1, 0)
                 .AddLayoutConstraint(fifth, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(fifth, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 0, 20);
            var sixth = new Button() { Content = "6" };
            First.Children.Add(sixth);
            First.AddLayoutConstraint(sixth, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1, 0)
                .AddLayoutConstraint(sixth, NSLayoutAttribute.Right, NSLayoutRelation.Equal, First, NSLayoutAttribute.Right, 1, 0)
                .AddLayoutConstraint(sixth, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(sixth, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 0, 20);
            var seventh = new Button() { Content = "7" };
            First.Children.Add(seventh);
            First.AddLayoutConstraint(seventh, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, First, NSLayoutAttribute.Bottom, 1, 0)
                .AddLayoutConstraint(seventh, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Left, 1, 0)
                 .AddLayoutConstraint(seventh, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(seventh, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 0, 20);
            var eighth = new Button() { Content = "8" };
            First.Children.Add(eighth);
            First.AddLayoutConstraint(eighth, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, First, NSLayoutAttribute.Bottom, 1, 0)
                .AddLayoutConstraint(eighth, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterX, 1, 0)
                 .AddLayoutConstraint(eighth, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(eighth, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 0, 20);
            var ninth = new Button() { Content = "8" };
            First.Children.Add(ninth);
            First.AddLayoutConstraint(ninth, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, First, NSLayoutAttribute.Bottom, 1, 0)
                .AddLayoutConstraint(ninth, NSLayoutAttribute.Right, NSLayoutRelation.Equal, First, NSLayoutAttribute.Right, 1, 0)
                 .AddLayoutConstraint(ninth, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(ninth, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.Height, 0, 20);
        }

        private void SetThird()
        {
            var First = Third;
            var first = new Button() { Content = "*" };
            First.Children.Add(first);
            First.AddLayoutConstraint(first, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1.5, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Left, 1, 10)
                .AddLayoutConstraint(first, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(first, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0, 20);
            var second = new Button() { Content = "**" };
            First.Children.Add(second);
            First.AddLayoutConstraint(second, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1.5, 0)
                .AddLayoutConstraint(second, NSLayoutAttribute.Left, NSLayoutRelation.Equal, first, NSLayoutAttribute.Right, 1, 10)
                .AddLayoutConstraint(second, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(second, NSLayoutAttribute.Height, NSLayoutRelation.Equal, first, NSLayoutAttribute.Height, 2, 0);
            var third = new Button() { Content = "***" };
            First.Children.Add(third);
            First.AddLayoutConstraint(third, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1.5, 0)
                .AddLayoutConstraint(third, NSLayoutAttribute.Left, NSLayoutRelation.Equal, second, NSLayoutAttribute.Right, 1, 10)
                .AddLayoutConstraint(third, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(third, NSLayoutAttribute.Height, NSLayoutRelation.Equal, second, NSLayoutAttribute.Height, 2, 0);
        }

        private void SetSecond()
        {
            var First = Second;
            var first = new Button() { Content = "*" };
            First.Children.Add(first);
            First.AddLayoutConstraint(first, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1, 0)
                .AddLayoutConstraint(first, NSLayoutAttribute.Left, NSLayoutRelation.Equal, First, NSLayoutAttribute.Left, 1, 10)
                .AddLayoutConstraint(first, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(first, NSLayoutAttribute.Height, NSLayoutRelation.Equal, First, NSLayoutAttribute.Height, 0, 20);
            var second = new Button() { Content = "**" };
            First.Children.Add(second);
            First.AddLayoutConstraint(second, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1, 0)
                .AddLayoutConstraint(second, NSLayoutAttribute.Left, NSLayoutRelation.Equal, first, NSLayoutAttribute.Right, 1, 10)
                .AddLayoutConstraint(second, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(second, NSLayoutAttribute.Height, NSLayoutRelation.Equal, first, NSLayoutAttribute.Height, 2, 0);
            var third = new Button() { Content = "***" };
            First.Children.Add(third);
            First.AddLayoutConstraint(third, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, First, NSLayoutAttribute.CenterY, 1, 0)
                .AddLayoutConstraint(third, NSLayoutAttribute.Left, NSLayoutRelation.Equal, second, NSLayoutAttribute.Right, 1, 10)
                .AddLayoutConstraint(third, NSLayoutAttribute.Width, NSLayoutRelation.Equal, First, NSLayoutAttribute.Width, 0, 20)
                .AddLayoutConstraint(third, NSLayoutAttribute.Height, NSLayoutRelation.Equal, second, NSLayoutAttribute.Height, 2, 0);
        }

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
        }
    }
}
