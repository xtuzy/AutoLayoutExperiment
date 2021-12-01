using CassowaryNET.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CassowaryNET;
using System.Diagnostics;
using CassowaryNET.Constraints;

namespace TestLibrary
{
    

    public static class TestCassowaryNET
    {
        public class VirtualView
        {
            public Variable left;
            public Variable top;
            public Variable width;
            public Variable height;
            public Variable right;
            public Variable bottom;
        }

        public static void Test()
        {
            Console.WriteLine("CassowaryNET C# Constrain 500 View Test Start:");
            CassowarySolver solver = new CassowarySolver();
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var superView = new VirtualView()
            {
                left = new Variable(),
                top = new Variable(),
                width = new Variable(),
                height = new Variable(),
                right = new Variable(),
                bottom = new Variable()
            };
            solver.AddConstraint(new EqualityConstraint(superView.left, 0, Strength.Required));
            solver.AddConstraint(new EqualityConstraint(superView.right, 0, Strength.Required));

            solver.AddConstraint(new EqualityConstraint(new LinearExpression(superView.right)-new LinearExpression(superView.left), superView.width, Strength.Required));
            solver.AddConstraint(new EqualityConstraint(new LinearExpression(superView.bottom) -new LinearExpression( superView.top), superView.height, Strength.Required));

            //solver.AddEditVariable(superView.width,Strength.Create(999, 1000, 1000);
            //solver.AddEditVariable(superView.height, Strength.Create(999, 1000, 1000));
            solver.AddStay(superView.width, Strength.Strong);
            solver.AddStay(superView.height, Strength.Strong);
            solver.SetEditedValue(superView.width, 300);
            solver.SetEditedValue(superView.height, 200);
            var preview = superView;
            for (var i = 0; i < 500; i++)
            {
                var subView3 = new VirtualView()
                {
                    left = new Variable(),
                    top = new Variable(),
                    width = new Variable(),
                    height = new Variable(),
                    right = new Variable(),
                    bottom = new Variable()
                };
                //log('-----loop createKiwiSolver 500view in ' + new Date().getTime() + 'ms ...');
                solver.AddConstraint(subView3.width == subView3.right - subView3.left);
                solver.AddConstraint(subView3.height==subView3.bottom - subView3.top);

                // Position sub-views in super-view
                solver.AddConstraint(subView3.left == preview.left);
                solver.AddConstraint(subView3.top == preview.top);
                solver.AddConstraint(subView3.bottom == preview.bottom);
                solver.AddConstraint(subView3.width == preview.width);
                solver.AddConstraint(subView3.right == preview.right);
                preview = subView3;
            }
            timer.Stop();
            Console.WriteLine("Spend time:" + timer.ElapsedMilliseconds + "ms");
            solver.Solve();
        }
    }
}
