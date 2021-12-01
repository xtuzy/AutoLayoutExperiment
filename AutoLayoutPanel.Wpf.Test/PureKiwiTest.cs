using Kiwi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLayoutPanel.Wpf.Test
{
    internal class PureKiwiTest
    {
        public class Element
        {
            double x;
            double y;
            double w;
            double h;
        }

        private struct MyConstraint
        {
            public int index;
            public Constraint constraint;
            public Variable propertyFirstVariable;
            public Variable propertySecondVariable;
            public string propertyFirst;
            public string propertySecond;
            public Element controlFirst;
            public Element controlSecond;
        };

        private Solver solver;

        public ArrayList Constraints;

        public void CreateConstraint()
        {
            /*Constraints = new ArrayList();
            var vx = new Constraint()
            solver.AddConstraint()*/
        }
    }
}
