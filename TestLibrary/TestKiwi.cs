using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kiwi;
namespace TestLibrary
{

    

   public static class TestKiwi
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
            Console.WriteLine("Kiwi C# Constrain 500 View Test Start:");
            Solver solver = new Solver();
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
            var ex = new Expression(new Term(superView.left), 0);
            solver.AddConstraint(new Constraint(ex,RelationalOperator.OP_EQ, Strength.Required));
            var exTop = new Expression(new Term(superView.top), 0);
            solver.AddConstraint(new Constraint(exTop,RelationalOperator.OP_EQ, Strength.Required));
            solver.AddConstraint(new Constraint(superView.right - superView.left == superView.width, Strength.Required));
            solver.AddConstraint(new Constraint(superView.bottom - superView.top == superView.height, Strength.Required));
            solver.AddEditVariable(superView.width, Strength.Create(999, 1000, 1000));
            solver.AddEditVariable(superView.height, Strength.Create(999, 1000, 1000));
            solver.SuggestValue(superView.width, 300);
            solver.SuggestValue(superView.height, 200);
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
                solver.AddConstraint(new Constraint(subView3.width == subView3.right - subView3.left, Strength.Required));
                solver.AddConstraint(new Constraint(subView3.bottom - subView3.top == subView3.height, Strength.Required));

                // Position sub-views in super-view
                solver.AddConstraint(new Constraint(subView3.left == preview.left, Strength.Required));
                solver.AddConstraint(new Constraint(subView3.top == preview.top, Strength.Required));
                solver.AddConstraint(new Constraint(subView3.bottom == preview.bottom, Strength.Required));
                solver.AddConstraint(new Constraint(subView3.width == preview.width, Strength.Required));
                solver.AddConstraint(new Constraint(subView3.right == preview.right, Strength.Required));
                preview = subView3;
            }
            timer.Stop();
            Console.WriteLine("Spend time:" + timer.ElapsedMilliseconds + "ms");
            solver.UpdateVariables();
        }
    }
}
