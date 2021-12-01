using System;
using System.Collections.Generic;
using Xunit;

namespace Kiwi.Tests
{
    public class TestVariable
    {
        // Test the variable modification methods.
        [Fact]
        public void test_variable_methods()
        {
            var v = new Variable("foo");

            Assert.Equal("foo", v.Name);
            Assert.Equal(0.0, v.Value);

            //    var ctx = object();
            //    v.setContext(ctx);
            //    Assert.Equal(v.context(), ctx);

            //    Assert.Equal("foo", str(v))
        }

        [Fact]
        public void test_variable_arith_operators()
        {
            // Test the arithmetic operation on variables.

            var v = new Variable("foo");
            var v2 = new Variable("bar");

            var neg = -v;
            Assert.Equal(v, neg.Variable);
            Assert.Equal(-1, neg.Coefficient);

            var mul = v * 2;
            Assert.Equal(v, mul.Variable);
            Assert.Equal(2, mul.Coefficient);

            var div = v / 2;
            Assert.Equal(v, div.Variable);
            Assert.Equal(0.5, div.Coefficient);

            var add = v + 2;
            Assert.Equal(2, add.Constant);
            Assert.Collection(add.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(1, term.Coefficient);
                });

            var add2 = v + v2;
            Assert.Equal(0, add2.Constant);
            Assert.Collection(add2.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(1, term.Coefficient);
                },
                term =>
                {
                    Assert.Equal(v2, term.Variable);
                    Assert.Equal(1, term.Coefficient);
                });

            var sub = v - 2;
            Assert.Equal(-2, sub.Constant);
            Assert.Collection(sub.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(1, term.Coefficient);
                });

            var sub2 = v - v2;
            Assert.Equal(0, sub2.Constant);
            Assert.Collection(sub2.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(1, term.Coefficient);
                }, 
                term =>
                {
                    Assert.Equal(v2, term.Variable);
                    Assert.Equal(-1, term.Coefficient);
                });
        }

        // Test using comparison on variables.
        [Fact]
        public void test_variable_rich_compare_operations()
        {
            var v = new Variable("foo");
            var v2 = new Variable("bar");

            var ops = new Dictionary<RelationalOperator, Func<Variable, Expression, Constraint>>
            {
                [RelationalOperator.OP_LE] = (left, right) => left <= right,
                [RelationalOperator.OP_EQ] = (left, right) => left == right,
                [RelationalOperator.OP_GE] = (left, right) => left >= right,
            };

            foreach (var op in ops.Keys)
            {
                var c = ops[op](v, v2 + 1);

                Assert.Collection(c.Expression.Terms,
                    term =>
                    {
                        Assert.Equal(v, term.Variable);
                        Assert.Equal(1, term.Coefficient);
                    },
                    term =>
                    {
                        Assert.Equal(v2, term.Variable);
                        Assert.Equal(-1, term.Coefficient);
                    });
                Assert.Equal(-1, c.Expression.Constant);
                Assert.Equal(Strength.Required, c.Strength);
                Assert.Equal(op, c.Op);
            }
        }
    }
}
