using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kiwi_Ts;
namespace TestLibrary
{
    internal class TestKiwiTs
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
            Console.WriteLine("Kiwi.js=>C# Constrain 500 View Test Start:");
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
            var ex = new Expression(superView.left);
            var cnLeft = new Constraint(ex, RelationalOperator.OP_EQ);
            solver.AddConstraint(cnLeft);
            var exTop = new Expression(superView.top);
            var csTop = new Constraint(exTop, RelationalOperator.OP_EQ, Strength.Required);
            solver.AddConstraint(csTop);
            var ex4 = new Expression((-1, superView.right), superView.left, superView.width);
            var cn4 = new Constraint(ex4, RelationalOperator.OP_EQ, Strength.Required);
            solver.AddConstraint(cn4);
            solver.AddConstraint(new Constraint(new Expression((-1,superView.bottom),superView.top, superView.height), RelationalOperator.OP_EQ, Strength.Required));
            solver.AddEditVariable(superView.width, Strength.Create(999));
            solver.AddEditVariable(superView.height, Strength.Create(999));
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
                
                solver.AddConstraint(new Constraint(new Expression((-1,subView3.right),subView3.left,subView3.width), RelationalOperator.OP_EQ, Strength.Required));
                solver.AddConstraint(new Constraint(new Expression((-1,subView3.bottom ), subView3.top, subView3.height), RelationalOperator.OP_EQ, Strength.Required));

                solver.AddConstraint(new Constraint(new Expression((-1,subView3.left), preview.left), RelationalOperator.OP_EQ));
                solver.AddConstraint(new Constraint(new Expression((-1,subView3.top), preview.top), RelationalOperator.OP_EQ));
                solver.AddConstraint(new Constraint(new Expression((-1, subView3.bottom), preview.bottom), RelationalOperator.OP_EQ));
                solver.AddConstraint(new Constraint(new Expression((-1, subView3.width), preview.width), RelationalOperator.OP_EQ));
                if (i > 400)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    solver.AddConstraint(new Constraint(new Expression((-1, subView3.right), preview.right), RelationalOperator.OP_EQ));
                    sw.Stop();
                    //Console.WriteLine($"{i} View Single Constraint Spend Time:" + sw.ElapsedMilliseconds + "ms");
                }
                else
                {
                    solver.AddConstraint(new Constraint(new Expression((-1, subView3.right), preview.right), RelationalOperator.OP_EQ));
                }
                preview = subView3;
            }
            timer.Stop();
            Console.WriteLine("Spend Time:" + timer.ElapsedMilliseconds + "ms");
            
            solver.UpdateVariables();
        }

        public static void TestSimpleConstraint()
        {
            var solver = new Solver();
            
            var preview = new VirtualView()
            {
                left = new Variable(),
                top = new Variable(),
                width = new Variable(),
                height = new Variable(),
                right = new Variable(),
                bottom = new Variable()
            };
            var ex = new Expression(preview.left);
            var cnLeft = new Constraint(ex, RelationalOperator.OP_EQ);
            solver.AddConstraint(cnLeft);
            var exTop = new Expression(preview.top);
            var csTop = new Constraint(exTop, RelationalOperator.OP_EQ, Strength.Required);
            solver.AddConstraint(csTop);
            var ex3 = new Expression((-1,preview.right),preview.left, preview.width);
            var cn3 = new Constraint(ex3, RelationalOperator.OP_EQ, Strength.Required);
            solver.AddConstraint(cn3);
            solver.AddConstraint(new Constraint(new Expression((-1,preview.bottom),preview.top, preview.height), RelationalOperator.OP_EQ, Strength.Required));
            solver.AddEditVariable(preview.width, Strength.Create(999));
            solver.AddEditVariable(preview.height, Strength.Create(999));
            solver.SuggestValue(preview.width, 300);
            solver.SuggestValue(preview.height, 300);
            var subView = new VirtualView()
            {
                left = new Variable(),
                top = new Variable(),
                width = new Variable(),
                height = new Variable(),
                right = new Variable(),
                bottom = new Variable()
            };
            solver.AddConstraint(new Constraint(new Expression((-1, subView.right),subView.left, subView.width), RelationalOperator.OP_EQ));
            solver.AddConstraint(new Constraint(new Expression((-1,subView.bottom),subView.top, subView.height), RelationalOperator.OP_EQ));

            // Position sub-views in super-view
            solver.AddConstraint(new Constraint(new Expression((-1, subView.left), preview.left), RelationalOperator.OP_EQ));
            solver.AddConstraint(new Constraint(new Expression((-1, subView.top), preview.top), RelationalOperator.OP_EQ));
            solver.AddConstraint(new Constraint(new Expression((-1,subView.bottom), (0.5,preview.bottom)), RelationalOperator.OP_EQ));
            solver.AddConstraint(new Constraint(new Expression((-1,subView.width), (0.5,preview.width)), RelationalOperator.OP_EQ));
            //solver.AddConstraint(new Constraint(new Expression(subView3.right, preview.right), RelationalOperator.OP_EQ));


            // Calculate
            solver.UpdateVariables();
            var a = solver;
            Console.WriteLine("subview");
            Console.WriteLine("left " + subView.left.Value);
            Console.WriteLine("top " + subView.top.Value);
            Console.WriteLine("right " + subView.right.Value);
            Console.WriteLine("bottom " + subView.bottom.Value);
            Console.WriteLine("width " + subView.width.Value );
            Console.WriteLine("height " + subView.height.Value );

            var id = solver.GetType();
        }
    }
}
