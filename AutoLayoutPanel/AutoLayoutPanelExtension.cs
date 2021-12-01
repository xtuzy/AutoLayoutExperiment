using System;

#if NETCOREAPP 
using System.Windows;
#elif __ANDROID__
using UIElement = Android.Views.View;
#elif __IOS__
using UIElement = UIKit.UIView;
#endif

namespace AutoLayout
{
    public static class AutoLayoutPanelExtension
    {
        static string[] NSLayoutAttributeString = new string[]
        {
            "Left",
            "Right",
            "Top",
            "Bottom",
            "Center",
            "Middle",
            "Width",
            "Height",
        };

        static string[] ConstraintRelationString = new string[]
        {
            "=",
            "<",
            ">",
        };

        public static AutoLayout.AutoLayoutPanel AddLayoutConstraint(
            this AutoLayout.AutoLayoutPanel panel,
            UIElement controlFirst,
            NSLayoutAttribute propertyFirst,
             NSLayoutRelation relatedBy,
            UIElement controlSecond,
            NSLayoutAttribute propertySecond,
            double multiplier = 1,
            double constant = 0)
        {
            panel.AddLayoutConstraint(controlFirst, NSLayoutAttributeString[(int)propertyFirst],ConstraintRelationString[(int)relatedBy], controlSecond, NSLayoutAttributeString[(int)propertySecond], Math.Round(multiplier,4),Math.Round(constant,4));
            return panel;
        }
    }

    public enum NSLayoutAttribute
    {
        //[Register("LEFT")]
        Left,// = "Left";

        //[Register("RIGHT")]
        Right,// = "Right";

        //[Register("TOP")]
        Top,// = "Top";

        //[Register("BOTTOM")] 
        Bottom,// = "Bottom";

        //[Register("BASELINE")] 
        /// <summary>
        /// The baseline of the text in a view.
        /// </summary>
        //Baseline = ConstraintSet.Baseline,//5 ,

        //[Register("START")]
        /// <summary>
        /// The left side of a view in left to right languages.
        /// </summary>
        //Leading = ConstraintSet.Start,// 6,

        //[Register("END")]
        /// <summary>
        /// The right side of a view in right to left languages.
        /// </summary>
        //Trailing = ConstraintSet.End,// 7,
        //根据AutoLayout添加
        CenterX,// = "Center";
        CenterY,// = "Middle";
        Width,
        Height,
    }
    public enum  NSLayoutRelation
    {
        Equal,// = "=";
        LessThanOrEqual,// = "<";
        GreaterThanOrEqual,// = ">";
    }
}
