using System;
using System.Collections.Generic;
using Xunit;

namespace Kiwi.Tests
{
    public class TestExpression
    {

        [Fact]
        public void test_expression_creation()
        {
            // Test the Term constructor.

            var v = new Variable("foo");
            var v2 = new Variable("bar");
            var v3 = new Variable("aux");
            //var e1 = new Expression((v * 1, v2 * 2, v3 * 3));
            //var e2 = new Expression((v * 1, v2 * 2, v3 * 3), 10);

            //    for e, val in ((e1, 0), (e2, 10)):
            //        var t = e.Terms;
            //        assert(len(t) == 3 and
            //                t[0].Variable is v and t[0].Coefficient == 1 and
            //                t[1].Variable is v2 and t[1].Coefficient == 2 and
            //                t[2].Variable is v3 and t[2].Coefficient == 3);
            //        Assert.Equal(val, e.Constant)

            //    Assert.Equal("1 * foo + 2 * bar + 3 * aux + 10", str(e2))
        }


        [Fact]
        public void test_expression_arith_operators()
        {
            // Test the arithmetic operation on terms.

            var v = new Variable("foo");
            var v2 = new Variable("bar");
            var t = new Term(v, 10);
            var t2 = new Term(v2);
            var e = t + 5;
            var e2 = v2 - 10;

            var neg = -e;
            Assert.Equal(-5, neg.Constant);
            Assert.Collection(neg.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(-10, term.Coefficient);
                });

            var mul = e * 2;
            Assert.Equal(10, mul.Constant);
            Assert.Collection(mul.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(20, term.Coefficient);
                });

            var div = e / 2;
            Assert.Equal(2.5, div.Constant);
            Assert.Collection(div.Terms, 
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(5, term.Coefficient);
                });

            var add = e + 2;
            Assert.Equal(7, add.Constant);
            Assert.Collection(add.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(10, term.Coefficient);
                });

            var add2 = e + v2;
            Assert.Equal(5, add2.Constant);
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

            var add3 = e + t2;
            Assert.Equal(5, add3.Constant);
            Assert.Collection(add3.Terms,
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

            var add4 = e + e2;
            Assert.Equal(-5, add4.Constant);
            Assert.Collection(add4.Terms,
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

            var sub = e - 2;
            Assert.Equal(3, sub.Constant);
            Assert.Collection(sub.Terms,
                term =>
                {
                    Assert.Equal(v, term.Variable);
                    Assert.Equal(10, term.Coefficient);
                });

            var sub2 = e - v2;
            Assert.Equal(5, sub2.Constant);
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

            var sub3 = e - t2;
            Assert.Equal(5, sub3.Constant);
            Assert.Collection(sub3.Terms,
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

            var sub4 = e - e2;
            Assert.Equal(15, sub4.Constant);
            Assert.Collection(sub4.Terms,
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
        public void test_expression_rich_compare_operations()
        {
            // Test using comparison on variables.

            var v = new Variable("foo");
            var v2 = new Variable("bar");
            var t1 = new Term(v, 10);
            var e1 = t1 + 5;
            var e2 = v2 - 10;

            var ops= new Dictionary<RelationalOperator, Func<Expression, Expression, Constraint>>()
            {
                [RelationalOperator.OP_LE] = (left, right) => left <= right,
                [RelationalOperator.OP_EQ] = (left, right) => left == right,
                [RelationalOperator.OP_GE] = (left, right) => left >= right,
            };

            foreach (var op in ops.Keys)
            {
                var c = ops[op](e1, e2);

                Assert.Collection(c.Expression.Terms,
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
                Assert.Equal(15, c.Expression.Constant);
                Assert.Equal(op, c.Op);
                Assert.Equal(Strength.Required, c.Strength);
            }
        }
    }
}
