#if NETCOREAPP //net5-windows Wpf默认条件编译符号
using System;
//using System.Collections;
//using System.Collections.No
using System.Windows;
using System.Windows.Controls;
//using System.Collections.Generic;
using Kiwi;
using ClConstraint = Kiwi.Constraint;
using ClVariable = Kiwi.Variable;
using ClSimplexSolver = Kiwi.Solver;
using ClStrength = Kiwi.Strength;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Helper.Tools;

namespace BetterKiwiLayout
{
    public class BetterKiwiLayoutPanel : Panel
    {
        private class Constraint//类不需要包裹
        {
            public ClConstraint constraint;
            public ClVariable propertyFirstVariable;
            public ClVariable propertySecondVariable;
            public String propertyFirst;
            public String propertySecond;
            public UIElement controlFirst;
            public UIElement controlSecond;
        };

        private Dictionary<string, List<Constraint>> Constraints;//UIElemente-Constraint
        private Dictionary<string, ClVariable> Variables;//UIElemente-Constraint
        private Dictionary<string, Constraint> WillChangedConstraints;//UIElemente-Constraint

        private ClSimplexSolver solver;
        public BetterKiwiLayoutPanel() : base()
        {

            solver = new ClSimplexSolver();

            Constraints = new Dictionary<string, List<Constraint>>();
            Variables = new Dictionary<string, ClVariable>();
            WillChangedConstraints = new Dictionary<string, Constraint>();

            // Force our X/Y to be 0, 0
            //ClVariable clX = FindOrCreateClVariable(this, "X");
            ClVariable clX = FindOrCreateClVariable(this, "Left");
            //ClVariable clY = FindOrCreateClVariable(this, "Y");
            ClVariable clY = FindOrCreateClVariable(this, "Top");

            
            PerformanceTestHelper.StartRecord();
            solver.AddConstraint(new ClConstraint(clX == 0, ClStrength.Required));
            PerformanceTestHelper.OutputRecord("clx");
            solver.AddConstraint(new ClConstraint(clY == 0, ClStrength.Required));
        }

        protected String GetKey(UIElement cntl)
        {
            return cntl.GetHashCode().ToString();
        }

        protected void AddNewControl(UIElement cntl)
        {
           // ClVariable clX = FindOrCreateClVariable(cntl, "X");//使用XY不直接用Left可能是为了区分左右和上下布局的起始位置
            //ClVariable clY = FindOrCreateClVariable(cntl, "Y");
            ClVariable clWidth = FindOrCreateClVariable(cntl, "Width");
            ClVariable clHeight = FindOrCreateClVariable(cntl, "Height");
            ClVariable clLeft = FindOrCreateClVariable(cntl, "Left");
            ClVariable clRight = FindOrCreateClVariable(cntl, "Right");
            ClVariable clCenter = FindOrCreateClVariable(cntl, "Center");
            ClVariable clMiddle = FindOrCreateClVariable(cntl, "Middle");
            ClVariable clTop = FindOrCreateClVariable(cntl, "Top");
            ClVariable clBottom = FindOrCreateClVariable(cntl, "Bottom");

            /**
             * 定义基本的关系
             * 
             */
            // X = Center - (Width/2)
           // solver.AddConstraint(new ClConstraint(clX == clCenter - clWidth / 2, ClStrength.Required));
            solver.AddConstraint(new ClConstraint(clLeft == clCenter - clWidth / 2, ClStrength.Required));

            // X = Right - Width
            //solver.AddConstraint(new ClConstraint(clX == clRight - clWidth, ClStrength.Required));
            solver.AddConstraint(new ClConstraint(clLeft == clRight - clWidth, ClStrength.Required));

            // Y = Top
            //solver.AddConstraint(new ClConstraint(clY == clTop, ClStrength.Required));
          
            // Y = Middle - (Height/2)
            //solver.AddConstraint(new ClConstraint(clY == clMiddle - clHeight / 2, ClStrength.Required));
            solver.AddConstraint(new ClConstraint(clTop == clMiddle - clHeight / 2, ClStrength.Required));

            // Y = Bottom - Height
            //solver.AddConstraint(new ClConstraint(clY == clBottom - clHeight, ClStrength.Required));
            solver.AddConstraint(new ClConstraint(clTop == clBottom - clHeight, ClStrength.Required));
            
            // X = Left
            //solver.AddConstraint(new ClConstraint(clX == clLeft, ClStrength.Required));
        }

        protected UIElement FindOrNewConstraintList(UIElement em)
        {
            if (!Constraints.ContainsKey(GetKey(em)))
            {
                Constraints.Add(GetKey(em), new List<Constraint>());
                AddNewControl(em);
            }
            return em;
        }

        protected ClVariable FindOrCreateClVariable(UIElement em, String property)
        {
            string variableName = GetKey(em) + "_" + property;
            if (Variables.ContainsKey(variableName))
            {
                return Variables[variableName];
            }
            else
            {
                var newVariable = new ClVariable(variableName);
                Variables.Add(variableName, newVariable);
                return newVariable;
            }
        }

        public UIElement AddLayoutConstraint(UIElement controlFirst,
            String propertyFirst,
            String relatedBy,
            UIElement controlSecond,
            String propertySecond,
            double multiplier,
            double constant)
        {
            Constraint target = new Constraint();
            target.propertyFirst = propertyFirst;
            target.controlFirst = FindOrNewConstraintList(controlFirst);//会为元素创建ArrayList
            target.propertyFirstVariable = FindOrCreateClVariable(controlFirst, propertyFirst);

            int ndx = Constraints.Count;
            //byte equality = (byte)(relatedBy.Equals("<") ? Cl.LEQ : relatedBy.Equals(">") ? Cl.GEQ : 0);
            var equality = relatedBy.Equals("<=") ? RelationalOperator.OP_LE : relatedBy.Equals(">=") ? RelationalOperator.OP_GE : RelationalOperator.OP_EQ;

            if (controlSecond == null)
            {
                if (equality == RelationalOperator.OP_EQ)
                    target.constraint = new ClConstraint(target.propertyFirstVariable == constant, ClStrength.Required);
                else if (equality == RelationalOperator.OP_LE)//"<="                                   
                    target.constraint = new ClConstraint(target.propertyFirstVariable <= constant, ClStrength.Required);
                else//">="
                    target.constraint = new ClConstraint(target.propertyFirstVariable >= constant, ClStrength.Required);
            }
            else
            {
                //更改Find方法后,这里再用Find方法不太对,但查找不到会自动创建,结果应该一样,所以不修改
                target.controlSecond = FindOrNewConstraintList(controlSecond);
                target.propertySecondVariable = FindOrCreateClVariable(controlSecond, propertySecond);
                target.propertySecond = propertySecond;

                if (equality == RelationalOperator.OP_EQ)
                {
                    // y = m*x + c
                    target.constraint = new ClConstraint(target.propertyFirstVariable == target.propertySecondVariable * multiplier + constant, ClStrength.Required);
                }
                else if (equality == RelationalOperator.OP_LE)//"<="
                {
                    // y < m*x + c 
                    target.constraint = new ClConstraint(target.propertyFirstVariable <= target.propertySecondVariable * multiplier + constant, ClStrength.Required);
                }
                else//">="
                {
                    // y > m * x + c
                    target.constraint = new ClConstraint(target.propertyFirstVariable >= target.propertySecondVariable * multiplier + constant, ClStrength.Required);
                }
            }
            solver.AddConstraint(target.constraint);
            var elementConstraints = Constraints[GetKey(controlFirst)];
            elementConstraints.Add(target);

            return controlFirst;
        }

        //一般是不会使用index来移除的,实际使用是根据控件元素来的
        /*public void RemoveLayoutConstraint(int ndx)
        {
            Constraint c = (Constraint)Constraints[ndx];
            solver.RemoveConstraint(c.constraint);
            //TODO: Determine if target controls need to be in Controls, ControlVariables, VarContraints
            Constraints.RemoveAt(ndx);
        }*/

        /// <summary>
        /// 我添加的Extension
        /// </summary>
        /// <param name="uIElement"></param>
        public void RemoveLayoutConstraint(UIElement uIElement)
        {
            var elementConstraints = Constraints[GetKey(uIElement)];

            foreach (var element in elementConstraints)
            {
                solver.RemoveConstraint(element.constraint);
            }
            Constraints.Remove(GetKey(uIElement));
        }


        protected void SetMeasuredWHValue(UIElement ele, string property, double x, double s)
        {
            string variableName = GetKey(ele) + "_" + property;
            Constraint needChangeConstraint = null;
            if (WillChangedConstraints.ContainsKey(variableName))
            {
                needChangeConstraint = WillChangedConstraints[variableName];
                solver.RemoveConstraint(needChangeConstraint.constraint);
                WillChangedConstraints.Remove(variableName);
            }
            else
            {
                needChangeConstraint = new Constraint();
                needChangeConstraint.propertyFirstVariable = FindOrCreateClVariable(ele, property);
            }
            ClConstraint newConstraint = new ClConstraint(needChangeConstraint.propertyFirstVariable == x, s);
            needChangeConstraint.constraint = newConstraint;
            solver.AddConstraint(newConstraint);
            WillChangedConstraints.Add(variableName, needChangeConstraint);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                if (!child.IsMeasureValid)
                    child.Measure(availableSize);
            }
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Xamarin.Helper.Tools.RecordTimeHelper.RecordTime("Before SetValue");
            SetMeasuredWHValue(this, "Width",
                finalSize.Width, ClStrength.Required);
            SetMeasuredWHValue(this, "Height",
                finalSize.Height, ClStrength.Required);

            foreach (UIElement child in InternalChildren)
            {
                SetMeasuredWHValue(child, "Width",
                    child.DesiredSize.Width, ClStrength.Strong);
                SetMeasuredWHValue(child, "Height",
                    child.DesiredSize.Height, ClStrength.Strong);
            }

            Xamarin.Helper.Tools.RecordTimeHelper.RecordTime("Before solver");
            //solver.Resolve();
            solver.UpdateVariables();
            Xamarin.Helper.Tools.RecordTimeHelper.RecordTime("After solver");
            Xamarin.Helper.Tools.RecordTimeHelper.StopRecordTime();
            foreach (UIElement child in InternalChildren)
            {
                string Id = GetKey(child);
                /*child.Arrange(new Rect(
                                new Point(((ClVariable)ControlVariables[Id + "_X"]).Value,
                                    ((ClVariable)ControlVariables[Id + "_Y"]).Value),
                                new Size(((ClVariable)ControlVariables[Id + "_Width"]).Value,
                                    ((ClVariable)ControlVariables[Id + "_Height"]).Value)));*/
                //var elementConstraints = Constraints[Id];
                //var p = new Point(Variables[Id + "_X"].Value,Variables[Id + "_Y"].Value);
                var p = new Point(Variables[Id + "_Left"].Value,Variables[Id + "_Top"].Value);
                var s = new Size(Variables[Id + "_Width"].Value,Variables[Id + "_Height"].Value);
                /*foreach (var element in elementConstraints)
                {
                    if (element.propertyFirstVariable.Name == Id + "_X")
                    {
                        p.X = element.propertyFirstVariable.Value;
                        System.Diagnostics.Debug.WriteLine(child + " X " + p.X);
                    }
                    else if (element.propertyFirstVariable.Name == Id + "_Y")
                    {
                        p.Y = element.propertyFirstVariable.Value;
                    }
                    else if (element.propertyFirstVariable.Name == Id + "_Width")
                    {
                        s.Width = element.propertyFirstVariable.Value;
                    }
                    else if (element.propertyFirstVariable.Name == Id + "_Height")
                    {
                        s.Height = element.propertyFirstVariable.Value;
                    }
                    else
                    {
                        continue;
                    }
                }*/

                child.Arrange(new Rect(p, s));

            }
            return finalSize;

        }
    }
}
#endif