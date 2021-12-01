using System;
using System.Collections.Generic;
using Xunit;

namespace Kiwi.Tests
{
    public class TestTerm
    {

        [Fact]
        public void test_term_creation()
        {
            // Test the Term constructor.

            var v = new Variable("foo");
            var t = new Term(v);
            Assert.Equal(t.Variable, v);
            Assert.Equal(1, t.Coefficient);

            t = new Term(v, 100);
            Assert.Equal(t.Variable, v);
            Assert.Equal(100, t.Coefficient);

            //Assert.Equal("100 * foo", str(t))
        }

        [Fact]
        public void test_term_arith_operators()
        {
            // Test the arithmetic operation on terms.

            var v = new Variable("foo");
            var v2 = new Variable("bar");
            var t = new Term(v, 10);
            var t2 = new Term(v2);

            var neg = -t;
            Assert.Equal(v, neg.Variable);
            Assert.Equal(-10, neg.Coefficient);

            var mul = t * 2;
            Assert.Equal(v, mul.Variable);
            Assert.Equal(20, mul.Coefficient);

            var div = t / 2;
            Assert.Equal(v, div.Variable);
            Assert.Equal(5, div.Coefficient);

            var add = t + 2;
            Assert.Equal(2, add.Constant);
            Assert.Collection(add.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(10, term.Coefficient);
                });

            var add2 = t + v2;
            Assert.Equal(0, add2.Constant);
            Assert.Collection(add2.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(10, term.Coefficient);
                },
                term =>
                {
                    Assert.Equal(v2, term.Variable);
                    Assert.Equal(1, term.Coefficient);
                });

            add2 = t + t2;
            Assert.Equal(0, add2.Constant);
            Assert.Collection(add2.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(10, term.Coefficient);
                },
                term =>
                {
                    Assert.Equal(v2, term.Variable);
                    Assert.Equal(1, term.Coefficient);
                });

            var sub = t - 2;
            Assert.Equal(-2, sub.Constant);
            Assert.Collection(sub.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(10, term.Coefficient);
                });

            var sub2 = t - v2;
            Assert.Equal(0, sub2.Constant);
            Assert.Collection(sub2.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(10, term.Coefficient);
                },
                term =>
                {
                    Assert.Equal(v2, term.Variable);
                    Assert.Equal(-1, term.Coefficient);
                });

            sub2 = t - t2;
            Assert.Equal(0, sub2.Constant);
            Assert.Collection(sub2.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(10, term.Coefficient);
                },
                term =>
                {
                    Assert.Equal(v2, term.Variable);
                    Assert.Equal(-1, term.Coefficient);
                });
        }

        [Fact]
        public void test_term_rich_compare_operations()
        {
            // Test using comparison on variables.

            var v = new Variable("foo");
            var v2 = new Variable("bar");
            var t1 = new Term(v, 10);
            var t2 = new Term(v2, 20);

            var ops = new Dictionary<RelationalOperator, Func<Term, Expression, Constraint>>
            {
                [RelationalOperator.OP_LE] = (left, right) => left <= right,
                [RelationalOperator.OP_EQ] = (left, right) => left == right,
                [RelationalOperator.OP_GE] = (left, right) => left >= right,
            };

            foreach (var op in ops.Keys)
            {
                var c = ops[op](t1, t2 + 1);

                Assert.Collection(c.Expression.Terms, 
                    term =>
                    {
                        Assert.Equal(v, term.Variable);
                        Assert.Equal(10, term.Coefficient);
                    },
                    term =>
                    {
                        Assert.Equal(v2, term.Variable);
                        Assert.Equal(-20, term.Coefficient);
                    });
                Assert.Equal(-1, c.Expression.Constant);
                Assert.Equal(Strength.Required, c.Strength);
                Assert.Equal(op, c.Op);
            }
        }
    }
}
