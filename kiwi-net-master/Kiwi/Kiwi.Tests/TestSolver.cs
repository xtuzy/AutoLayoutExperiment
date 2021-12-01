using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kiwi.Tests
{
    public class TestSolver
    {
        [Fact]
        public void test_managing_edit_variable()
        {
            // Test adding/removing edit variables.

            var s = new Solver();
            var v1 = new Variable("foo");
            var v2 = new Variable("bar");


            Assert.False(s.HasEditVariable(v1));
            s.AddEditVariable(v1, Strength.Weak);
            Assert.True(s.HasEditVariable(v1));
            Assert.Throws<DuplicateEditVariable>(() => s.AddEditVariable(v1, Strength.Medium));
            Assert.Throws<UnknownEditVariable>(() => s.RemoveEditVariable(v2));
            s.RemoveEditVariable(v1);
            Assert.False(s.HasEditVariable(v1));

            Assert.Throws<BadRequiredStrength>(() => s.AddEditVariable(v1, Strength.Required));

            s.AddEditVariable(v2, Strength.Strong);
            Assert.True(s.HasEditVariable(v2));
            Assert.Throws<UnknownEditVariable>(() => s.SuggestValue(v1, 10));

            s.Reset();
            Assert.False(s.HasEditVariable(v2));
        }

        [Fact]
        public void test_managing_constraints()
        {
            // Test adding/removing constraints.

            var s = new Solver();
            var v = new Variable("foo");
            var c1 = v >= 1;
            var c2 = v <= 0;

            Assert.False(s.HasConstraint(c1));
            s.AddConstraint(c1);
            Assert.True(s.HasConstraint(c1));
            Assert.Throws<DuplicateConstraint>(() => s.AddConstraint(c1));
            Assert.Throws<UnknownConstraint>(() => s.RemoveConstraint(c2));
            Assert.Throws<UnsatisfiableConstraint>(() => s.AddConstraint(c2));
            s.RemoveConstraint(c1);
            Assert.False(s.HasConstraint(c1));

            s.AddConstraint(c2);
            Assert.True(s.HasConstraint(c2));
            s.Reset();
            Assert.False(s.HasConstraint(c2));
        }

        [Fact]
        public void test_solving_under_constrained_system()
        {
            // Test solving an under constrained system.
            var s = new Solver();
            var v = new Variable("foo");
            var c = 2 * v + 1 >= 0;
            s.AddEditVariable(v, Strength.Weak);
            s.AddConstraint(c);
            s.SuggestValue(v, 10);
            s.UpdateVariables();

            Assert.Equal(21, c.Expression.Value);
            Assert.Equal(20, c.Expression.Terms[0].Value);
            Assert.Equal(10, c.Expression.Terms[0].Variable.Value);
        }

        [Fact]
        public void test_solving_with_strength()
        {
            // Test solving a system with unstatisfiable non-required constraint.

            var v1 = new Variable("foo");
            var v2 = new Variable("bar");
            var s = new Solver();

            s.AddConstraint(v1 + v2 == 0);
            s.AddConstraint(v1 == 10);
            s.AddConstraint((v2 >= 0) | Strength.Weak);
            s.UpdateVariables();
            Assert.Equal(10, v1.Value);
            Assert.Equal(-10, v2.Value);

            s.Reset();

            s.AddConstraint(v1 + v2 == 0);
            s.AddConstraint((v1 >= 10) | Strength.Medium);
            s.AddConstraint((v2 == 2) | Strength.Strong);
            s.UpdateVariables();
            Assert.Equal(-2, v1.Value);
            Assert.Equal(2, v2.Value);
        }
    }
}