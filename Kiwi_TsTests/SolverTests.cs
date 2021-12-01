using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kiwi_Ts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiwi_Ts.Tests
{
    [TestClass()]
    public class SolverTests
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

        [TestMethod()]
        public void SolverTest()
        {
            Solver solver = new Solver();
            var strength = Strength.Create(0, 900, 1000);
            var preview = new VirtualView()
            {
                left = new Variable(),
                top = new Variable(),
                width = new Variable(),
                height = new Variable(),
                right = new Variable(),
                bottom = new Variable()
            };
            var ex = new Expression(preview.left, 0);
            var cnLeft = new Constraint(ex, RelationalOperator.OP_EQ);
            solver.AddConstraint(cnLeft);
            var exTop = new Expression(preview.top, 0);
            var csTop = new Constraint(exTop, RelationalOperator.OP_EQ, Strength.Required);
            solver.AddConstraint(csTop);
            solver.AddConstraint(new Constraint(new Expression(preview.right - preview.left, preview.width), RelationalOperator.OP_EQ, Strength.Required));
            solver.AddConstraint(new Constraint(new Expression(preview.bottom - preview.top, preview.height), RelationalOperator.OP_EQ, Strength.Required));
            solver.AddEditVariable(preview.width, Strength.Create(999, 1000, 1000));
            solver.AddEditVariable(preview.height, Strength.Create(999, 1000, 1000));
            solver.SuggestValue(preview.width, 300);
            solver.SuggestValue(preview.height, 200);

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
            solver.AddConstraint(new Constraint(new Expression(subView3.width, subView3.right - subView3.left), RelationalOperator.OP_EQ, Strength.Required));
            solver.AddConstraint(new Constraint(new Expression(subView3.bottom - subView3.top, subView3.height), RelationalOperator.OP_EQ, Strength.Required));

            // Position sub-views in super-view
            solver.AddConstraint(new Constraint(new Expression(subView3.left, preview.left), RelationalOperator.OP_EQ, strength));
            solver.AddConstraint(new Constraint(new Expression(subView3.top, preview.top), RelationalOperator.OP_EQ, strength));
            solver.AddConstraint(new Constraint(new Expression(subView3.bottom, preview.bottom), RelationalOperator.OP_EQ, strength));
            solver.AddConstraint(new Constraint(new Expression(subView3.width, preview.width), RelationalOperator.OP_EQ, strength));
            solver.AddConstraint(new Constraint(new Expression(subView3.right, preview.right), RelationalOperator.OP_EQ, strength));


            // Calculate
            solver.UpdateVariables();
            Assert.Equals();
        }

    }
}