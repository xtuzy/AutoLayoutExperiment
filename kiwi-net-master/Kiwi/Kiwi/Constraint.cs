using System.Collections.Generic;
using System.Linq;

namespace Kiwi
{
    public enum RelationalOperator
    {
        OP_LE,//LessEqual
        OP_GE,//GreatEqual
        OP_EQ//Equal
    }

    public partial class Constraint
    {
        public Constraint(Expression expr, RelationalOperator op, double strength)
        {
            Expression = expr;
            Op = op;
            Strength = Kiwi.Strength.Clip(strength);
        }

        public Constraint(Expression expr, RelationalOperator op)
            : this(expr, op, Kiwi.Strength.Required)
        {
        }

        public Constraint(Constraint other, double strength) 
            : this(other.Expression, other.Op, strength)
        {
        }

        public Expression Expression { get; }

        public RelationalOperator Op { get; }

        public double Strength { get; }

        // TODO: remove
        public static Expression Reduce(Expression expr)
        {
            return expr.Reduce();
        }
    }
}