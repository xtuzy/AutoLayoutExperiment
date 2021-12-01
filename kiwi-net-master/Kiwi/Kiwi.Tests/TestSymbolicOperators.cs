using System;
using Xunit;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Kiwi.Tests
{
    public class TestSymbolicOperators
    {
        [Fact]
        public void VariableTermExpressionConstructors()
        {
            var x = new Variable("x");
            Assert.Equal("x", x.Name);

            var t = new Term(x, 2);
            Assert.Equal(x, t.Variable);
            Assert.Equal(2, t.Coefficient);

            var e = new Expression(t, 10);
            Assert.Single(e.Terms, t);
            Assert.Equal(10, e.Constant);
        }


        [Fact]
        public void VariableMultiplyConstant()
        {
            var x = new Variable("x");

            Term term = x * 3;

            Assert.Equal(term.Variable, x);
            Assert.True(term.Coefficient == 3);
        }

        [Fact]
        public void ConstantMultiplyVariable()
        {
            var x = new Variable("x");

            Term term = 4 * x;

            Assert.Equal(term.Variable, x);
            Assert.Equal(4, term.Coefficient);
        }

        [Fact]
        public void VariableDivideConstant()
        {
            var x = new Variable("x");

            Term term = x / 4;

            Assert.Equal(x, term.Variable);
            Assert.Equal(0.25, term.Coefficient);
        }

        [Fact]
        public void VariableMinus()
        {
            var x = new Variable("x");

            Term term = -x;

            Assert.Equal(x, term.Variable);
            Assert.Equal(-1.0, term.Coefficient);
        }


        [Fact]
        public void TermMultiplyConstant()
        {
            var x = new Variable("x");
            var t = new Term(x, 2);

            Term term = 3 * t;

            Assert.Equal(x, term.Variable);
            Assert.Equal(6, term.Coefficient);
        }

        [Fact]
        public void ConstantMultiplyTerm()
        {
            var x = new Variable("x");
            var t = new Term(x, 2);

            Term term = t * 4;

            Assert.Equal(x, term.Variable);
            Assert.Equal(8, term.Coefficient);
        }

        [Fact]
        public void TermDivideConstant()
        {
            var x = new Variable("x");
            var t = new Term(x, 2);

            Term term = t / 4;

            Assert.Equal(x, term.Variable);
            Assert.Equal(0.5, term.Coefficient);
        }

        [Fact]
        public void TermMinus()
        {
            var x = new Variable("x");
            var t = new Term(x, 2);

            Term term = -t;

            Assert.Equal(x, term.Variable);
            Assert.Equal(-2.0, term.Coefficient);
        }

        [Fact]
        public void ExpressionMultiplyConstant()
        {
            var x = new Variable("x");
            var t = new Term(x, 2);
            var e = new Expression(t, 1);

            Expression expr = e * 3;

            Assert.Single(expr.Terms);
            Assert.Equal(x, expr.Terms[0].Variable);
            Assert.Equal(6, expr.Terms[0].Coefficient);
            Assert.Equal(3, expr.Constant);
        }

        [Fact]
        public void ConstantMultiplyExpression()
        {
            var x = new Variable("x");
            var t = new Term(x, 2);
            var e = new Expression(t, 1);

            Expression expr = 4 * e;

            Assert.Single(expr.Terms);
            Assert.Equal(x, expr.Terms[0].Variable);
            Assert.Equal(8, expr.Terms[0].Coefficient);
            Assert.Equal(4, expr.Constant);
        }

        [Fact]
        public void ExpressionDivideConstant()
        {
            var x = new Variable("x");
            var t = new Term(x, 2);
            var e = new Expression(t, 1);

            Expression expr = e / 4;

            Assert.Single(expr.Terms);
            Assert.Equal(x, expr.Terms[0].Variable);
            Assert.Equal(0.5, expr.Terms[0].Coefficient);
            Assert.Equal(0.25, expr.Constant);
        }

        [Fact]
        public void ExpressionMinus()
        {
            var x = new Variable("x");
            var t = new Term(x, 2);
            var e = new Expression(t, 1);

            Expression expr = -e;

            Assert.Single(expr.Terms);
            Assert.Equal(x, expr.Terms[0].Variable);
            Assert.Equal(-2.0, expr.Terms[0].Coefficient);
            Assert.Equal(-1.0, expr.Constant);
        }


        #region Variable +,- operators
        
        [Fact]
        public void VariableAddExpression()
        {
            var x = new Variable("x");
            var y = new Variable("y");
            var t = new Term(y, 3);
            var e = new Expression(t, 10);

            var expr = x + e;

            Assert.Collection(expr.Terms,
                first => Assert.Equal(x, first.Variable),
                second => Assert.Equal(t, second));
            Assert.Equal(10, expr.Constant);
        }

        [Fact]
        public void VariableSubtactExpression()
        {
            var x = new Variable("x");
            var y = new Variable("y");
            var t = new Term(y, 3);
            var e = new Expression(t, 10);

            var expr = x - e;

            Assert.Collection(expr.Terms,
                first => Assert.Equal(x, first.Variable),
                second =>
                {
                    //TODO: implement Equals for Variable,Term,Expression classes: Assert.Equal(-t, second);
                    Assert.Equal(y, second.Variable);
                    Assert.Equal(-3, second.Coefficient);
                });
            Assert.Equal(-10, expr.Constant);
        }


        [Fact]
        public void VariableAddTerm()
        {
            var x = new Variable("x");
            var y = new Variable("x");

            Expression expr = x + new Term(y, 3);

            Assert.Collection(expr.Terms,
                first => Assert.Equal(x, first.Variable),
                second =>
                {
                    Assert.Equal(y, second.Variable);
                    Assert.Equal(3, second.Coefficient);
                });
        }

        [Fact]
        public void VariableSubtractTerm()
        {
            var x = new Variable("x");
            var y = new Variable("x");

            Expression expr = x - new Term(y, 3);

            Assert.Collection(expr.Terms,
                first =>
                {
                    Assert.Equal(x, first.Variable);
                },
                second =>
                {
                    Assert.Equal(y, second.Variable);
                    Assert.Equal(-3, second.Coefficient);
                }
            );
        }


        [Fact]
        public void VariableAddVariable()
        {
            var x = new Variable("x");
            var y = new Variable("y");

            Expression expr = x + y;

            Assert.Collection(expr.Terms,
                first => Assert.Equal(x, first.Variable),
                second => Assert.Equal(y, second.Variable));
        }

        [Fact]
        public void VariableSubtractVariable()
        {
            var x = new Variable("x");
            var y = new Variable("x");

            Expression expr = x - y;

            Assert.Collection(expr.Terms,
                first => Assert.Equal(x, first.Variable),
                second =>
                {
                    Assert.Equal(y, second.Variable);
                    Assert.Equal(-1, second.Coefficient);
                });
        }


        [Fact]
        public void VariableAddConstant()
        {
            var x = new Variable("x");

            Expression expr = x + 10;

            Assert.Single(expr.Terms);
            Assert.Equal(x, expr.Terms[0].Variable);
            Assert.Equal(10, expr.Constant);
        }

        [Fact]
        public void VariableSubtractConstant()
        {
            var x = new Variable("x");

            Expression expr = x - 10;

            Assert.Single(expr.Terms);
            Assert.Equal(x, expr.Terms[0].Variable);
            Assert.Equal(-10, expr.Constant);
        }


        [Fact]
        public void ConstantAddVariable()
        {
            var x = new Variable("x");

            Expression expr = 10 + x;

            Assert.Single(expr.Terms);
            Assert.Equal(x, expr.Terms[0].Variable);
            Assert.Equal(10, expr.Constant);
        }

        [Fact]
        public void ConstantSubtractVariable()
        {
            var x = new Variable("x");

            Expression expr = 10 - x;

            Assert.Single(expr.Terms);
            Assert.Equal(x, expr.Terms[0].Variable);
            Assert.Equal(10, expr.Constant);
        }
        
        #endregion
    }
}