using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace AutoLayoutTest
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

        public static AutoLayoutTest.AutoLayoutPanel AddLayoutConstraint(
            this AutoLayoutTest.AutoLayoutPanel panel,
            UIView controlFirst,
            NSLayoutAttribute propertyFirst,
             NSLayoutRelation relatedBy,
            UIView controlSecond,
            NSLayoutAttribute propertySecond,
            double multiplier = 1,
            double constant = 0)
        {
            panel.AddLayoutConstraint(controlFirst, NSLayoutAttributeString[(int)propertyFirst],ConstraintRelationString[(int)relatedBy], controlSecond, NSLayoutAttributeString[(int)propertySecond], multiplier, constant);
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
