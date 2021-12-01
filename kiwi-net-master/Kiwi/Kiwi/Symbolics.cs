using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Kiwi
{
    partial class Variable
    {
        private const string _notSupported = "Not supported";

        public static Term operator *(Variable v, double c) => new Term(v, c);
        public static Term operator *(double c, Variable v) => new Term(v, c);
        public static Term operator /(Variable v, double c) => new Term(v, 1/c);
        public static Term operator -(Variable v) => new Term(v, -1.0);

        public static Expression operator +(Variable left, Expression right) => new Term(left) + right;
        public static Expression operator +(Variable left, Term right) => new Term(left) + right;
        public static Expression operator +(Variable left, Variable right) => new Term(left) + right;
        public static Expression operator +(Variable left, double right) => new Term(left) + right;
        public static Expression operator +(double left, Variable right) => new Term(right) + left;

        public static Expression operator -(Variable left, Expression right) => new Term(left) - right;
        public static Expression operator -(Variable left, Term right) => new Term(left) - right;
        public static Expression operator -(Variable left, Variable right) => new Term(left) - right;
        public static Expression operator -(Variable left, double right) => new Term(left) - right;
        public static Expression operator -(double left, Variable right) => left - new Term(right);

        public static Constraint operator ==(Variable left, Expression right) => new Term(left) == right;
        public static Constraint operator ==(Variable left, Term right) => new Term(left) == right;
        public static Constraint operator ==(Variable left, Variable right) => new Term(left) == right;
        public static Constraint operator ==(Variable left, double right) => new Term(left) == right;
        public static Constraint operator ==(double left, Variable right) => new Term(right) == left;

        [Obsolete(_notSupported, true)] public static Constraint operator !=(Variable left, Expression right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(Variable left, Term right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(Variable left, Variable right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(Variable left, double right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(double left, Variable right) => throw new InvalidOperationException();

        public static Constraint operator <=(Variable left, Expression right) => new Term(left) <= right;
        public static Constraint operator <=(Variable left, Term right) => new Term(left) <= right;
        public static Constraint operator <=(Variable left, Variable right) => new Term(left) <= right;
        public static Constraint operator <=(Variable left, double right) => new Term(left) <= right;
        public static Constraint operator <=(double left, Variable right) => new Term(right) <= left;

        public static Constraint operator >=(Variable left, Expression right) => new Term(left) >= right;
        public static Constraint operator >=(Variable left, Term right) => new Term(left) >= right;
        public static Constraint operator >=(Variable left, Variable right) => new Term(left) >= right;
        public static Constraint operator >=(Variable left, double right) => new Term(left) >= right;
        public static Constraint operator >=(double left, Variable right) => new Term(right) >= left;

    }

    partial class Term
    {
        private const string _notSupported = "Not supported";

        public static Term operator *(Term t, double c) => new Term(t.Variable, t.Coefficient * c);
        public static Term operator *(double c, Term t) => new Term(t.Variable, t.Coefficient * c);
        public static Term operator /(Term t, double c) => new Term(t.Variable, t.Coefficient / c);
        public static Term operator -(Term t) => new Term(t.Variable, -t.Coefficient);

        public static Expression operator +(Term left, Expression right) => new Expression(left).Add(right);
        public static Expression operator +(Term left, Term right) => new Expression(left).Add(right);
        public static Expression operator +(Term left, Variable right) => new Expression(left).Add(right);
        public static Expression operator +(Term left, double right) => new Expression(left, right);
        public static Expression operator +(double left, Term right) => new Expression(right, left);

        public static Expression operator -(Term left, Expression right) => new Expression(left).Add(-right);
        public static Expression operator -(Term left, Term right) => new Expression(left).Add(-right);
        public static Expression operator -(Term left, Variable right) => new Expression(left).Add(-right);
        public static Expression operator -(Term left, double right) => new Expression(left, -right);
        public static Expression operator -(double left, Term right) => new Expression(-right, left);

        public static Constraint operator ==(Term left, Expression right) => new Constraint(left - right, RelationalOperator.OP_EQ);
        public static Constraint operator ==(Term left, Term right) => new Constraint(left - right, RelationalOperator.OP_EQ);
        public static Constraint operator ==(Term left, Variable right) => new Constraint(left - right, RelationalOperator.OP_EQ);
        public static Constraint operator ==(Term left, double right) => new Constraint(left - right, RelationalOperator.OP_EQ);
        public static Constraint operator ==(double left, Term right) => new Constraint(left - right, RelationalOperator.OP_EQ);

        [Obsolete(_notSupported, true)] public static Constraint operator !=(Term left, Expression right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(Term left, Term right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(Term left, Variable right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(Term left, double right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(double left, Term right) => throw new InvalidOperationException();

        public static Constraint operator <=(Term left, Expression right) => new Constraint(left - right, RelationalOperator.OP_LE);
        public static Constraint operator <=(Term left, Term right) => new Constraint(left - right, RelationalOperator.OP_LE);
        public static Constraint operator <=(Term left, Variable right) => new Constraint(left - right, RelationalOperator.OP_LE);
        public static Constraint operator <=(Term left, double right) => new Constraint(left - right, RelationalOperator.OP_LE);
        public static Constraint operator <=(double left, Term right) => new Constraint(left - right, RelationalOperator.OP_LE);

        public static Constraint operator >=(Term left, Expression right) => new Constraint(left - right, RelationalOperator.OP_GE);
        public static Constraint operator >=(Term left, Term right) => new Constraint(left - right, RelationalOperator.OP_GE);
        public static Constraint operator >=(Term left, Variable right) => new Constraint(left - right, RelationalOperator.OP_GE);
        public static Constraint operator >=(Term left, double right) => new Constraint(left - right, RelationalOperator.OP_GE);
        public static Constraint operator >=(double left, Term right) => new Constraint(left - right, RelationalOperator.OP_GE);
    }

    
    partial class Expression
    {
        private const string _notSupported = "Not supported";


        #region Array Concat Methods

        [Pure]
        private static Term[] Concat(Term[] a, Term[] b)
        {
            var result = new Term[a.Length + b.Length];
            a.CopyTo(result, 0);
            b.CopyTo(result, a.Length);
            return result;
        }

        [Pure]
        private static Term[] Concat(Term[] a, Term b)
        {
            var result = new Term[a.Length + 1];
            a.CopyTo(result, 0);
            result[a.Length] = b;
            return result;
        }

        #endregion

        [Pure]
        public Expression Scale(double scale)
        {
            var scaledTs = new Term[Terms.Length];
            for (int i = 0; i < Terms.Length; i++)
            {
                scaledTs[i] = Terms[i] * scale;
            }
            return new Expression(scaledTs, Constant * scale);
        }

        #region Append Methods

        [Pure]
        public Expression Add(Expression other)
        {
            return new Expression(Concat(Terms, other.Terms), Constant + other.Constant);
        }

        [Pure]
        public Expression Add(Term other)
        {
            return new Expression(Concat(Terms, other), Constant);
        }

        // TODO can be removed if there is Variable cast to Term operator
        [Pure]
        public Expression Add(Variable v)
        {
            return new Expression(Concat(Terms, new Term(v)), Constant);
        }

        [Pure]
        public Expression Add(double c)
        {
            return new Expression(Terms, Constant + c);
        }

        #endregion

        public static Expression operator *(Expression e, double c) => e.Scale(c);
        public static Expression operator *(double c, Expression e) => e.Scale(c);
        public static Expression operator /(Expression e, double c) => e.Scale(1/c);
        public static Expression operator -(Expression e) => e.Scale(-1.0);

        public static Expression operator +(Expression left, Expression right) => left.Add(right);
        public static Expression operator +(Expression left, Term right) => left.Add(right);
        public static Expression operator +(Expression left, Variable right) => left.Add(right);
        public static Expression operator +(Expression left, double right) => left.Add(right);
        public static Expression operator +(double left, Expression right) => right.Add(left);

        public static Expression operator -(Expression left, Expression right) => left.Add(-right);
        public static Expression operator -(Expression left, Term right) => left.Add(-right);
        public static Expression operator -(Expression left, Variable right) => left.Add(-right);
        public static Expression operator -(Expression left, double right) => left.Add(-right);
        public static Expression operator -(double left, Expression right) => right.Add(-left);

        public static Constraint operator ==(Expression left, Expression right) => new Constraint(left - right, RelationalOperator.OP_EQ);
        public static Constraint operator ==(Expression left, Term right) => new Constraint(left - right, RelationalOperator.OP_EQ);
        public static Constraint operator ==(Expression left, Variable right) => new Constraint(left - right, RelationalOperator.OP_EQ);
        public static Constraint operator ==(Expression left, double right) => new Constraint(left - right, RelationalOperator.OP_EQ);
        public static Constraint operator ==(double left, Expression right) => new Constraint(left - right, RelationalOperator.OP_EQ);

        [Obsolete(_notSupported, true)] public static Constraint operator !=(Expression left, Expression right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(Expression left, Term right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(Expression left, Variable right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(Expression left, double right) => throw new InvalidOperationException();
        [Obsolete(_notSupported, true)] public static Constraint operator !=(double left, Expression right) => throw new InvalidOperationException();

        public static Constraint operator <=(Expression left, Expression right) => new Constraint(left - right, RelationalOperator.OP_LE);
        public static Constraint operator <=(Expression left, Term right) => new Constraint(left - right, RelationalOperator.OP_LE);
        public static Constraint operator <=(Expression left, Variable right) => new Constraint(left - right, RelationalOperator.OP_LE);
        public static Constraint operator <=(Expression left, double right) => new Constraint(left - right, RelationalOperator.OP_LE);
        public static Constraint operator <=(double left, Expression right) => new Constraint(left - right, RelationalOperator.OP_LE);

        public static Constraint operator >=(Expression left, Expression right) => new Constraint(left - right, RelationalOperator.OP_GE);
        public static Constraint operator >=(Expression left, Term right) => new Constraint(left - right, RelationalOperator.OP_GE);
        public static Constraint operator >=(Expression left, Variable right) => new Constraint(left - right, RelationalOperator.OP_GE);
        public static Constraint operator >=(Expression left, double right) => new Constraint(left - right, RelationalOperator.OP_GE);
        public static Constraint operator >=(double left, Expression right) => new Constraint(left - right, RelationalOperator.OP_GE);
    }

    partial class Constraint
    {
        public static Constraint operator |(Constraint cnt, double strength) => new Constraint(cnt, strength);
        public static Constraint operator |(double strength, Constraint cnt) => new Constraint(cnt, strength);
    }
}
