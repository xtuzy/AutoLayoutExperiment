using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiwi_Ts
{
    public enum RelationalOperator
    {
        OP_LE,//LessEqual
        OP_GE,//GreatEqual
        OP_EQ//Equal
    }
    public partial class Constraint:IKeyId
    {
        /// <summary>
        /// expression = 0?
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="relationalOperator"></param>
        /// <param name="strength">默认为Strength.Required</param>
        public Constraint(Expression expression,RelationalOperator relationalOperator,double strength=-1)
        {
            this.expression = expression;
            this.Op = relationalOperator;
            if (strength == -1)
                strength = Kiwi_Ts.Strength.Required;
            this.Strength = Kiwi_Ts.Strength.Clip(strength);
        }

        /// <summary>
        /// expression = 0?
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="strength"></param>
        public Constraint((Expression, RelationalOperator) constraint,double strength = -1):this(constraint.Item1,constraint.Item2,strength)
        {
        }

        /// <summary>
        /// lhs = rhs?
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="relationalOperator"></param>
        /// <param name="rhs"></param>
        /// <param name="strength"></param>
        public Constraint(Expression lhs, RelationalOperator relationalOperator, Expression rhs, double strength=-1)
        {
            this.expression = new Expression( lhs, (-1, rhs));
            this.Op = relationalOperator;

            if (strength == -1)
                strength = Kiwi_Ts.Strength.Required;
            this.Strength = Kiwi_Ts.Strength.Clip(strength);
        }

        /// <summary>
        /// lhs = rhs?
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="strength"></param>
        public Constraint((Expression, RelationalOperator,Expression) constraint, double strength = -1) : this(constraint.Item1, constraint.Item2,constraint.Item3, strength)
        {
        }

        public Expression expression { get; private set; }
        public RelationalOperator Op { get; private set; }
        public double Strength { get; private set; }
        int Id = CnId++;

        ///**
        // * The internal constraint id counter.
        // * @private
        // */
        // let CnId = 0;
        static int CnId=0;

        public int id() => Id;
    }

    public partial class Constraint
    {
      
    }

    public partial class Variable
    {
        /// <summary>
        /// multiple*variable
        /// </summary>
        /// <param name="multiple"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public static Expression operator *(double multiple, Variable variable) => new Expression((multiple,variable),0);

        /// <summary>
        /// variable=constant
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        public static (Expression, RelationalOperator, Expression) operator ==(Variable variable, double constant) => (-1 * variable == constant);
        public static (Expression, RelationalOperator, Expression) operator !=(Variable variable, double constant) => throw new NotImplementedException();
    }

    public partial class Expression
    {
        public static (Expression, RelationalOperator, Expression) operator ==(Expression lhs, Expression rhs) => (lhs, RelationalOperator.OP_EQ, rhs);
        public static (Expression, RelationalOperator, Expression) operator >=(Expression lhs, Expression rhs) => (lhs, RelationalOperator.OP_GE, rhs);
        public static (Expression, RelationalOperator, Expression) operator <=(Expression lhs, Expression rhs) => (lhs, RelationalOperator.OP_LE, rhs);
        public static (Expression, RelationalOperator, Expression) operator !=(Expression lhs, Expression rhs) => throw new NotImplementedException();

        public static (Expression, RelationalOperator, Expression) operator ==(Expression lhs, double rhs) => (lhs, RelationalOperator.OP_EQ, new Expression(rhs));
        public static (Expression, RelationalOperator, Expression) operator !=(Expression lhs, double rhs) => throw new NotImplementedException();
    }
}
