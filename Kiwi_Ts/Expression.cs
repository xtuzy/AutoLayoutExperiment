using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiwi_Ts
{
    /**
     * An expression of variable terms and a constant.
     *
     * The constructor accepts an arbitrary number of parameters,
     * each of which must be one of the following types:
     *  - number
     *  - Variable
     *  - Expression
     *  - 2-tuple of [number, Variable|Expression]
     *
     * The parameters are summed. The tuples are multiplied.
     *
     * @class
     * @param {...(number|Variable|Expression|Array)} args
     */
    public partial class Expression:IDisposable
    {
        public IMap<Variable, double> terms = IMap<Variable, double>.createMap();
        public double Constant = 0.0;

        #region constructor

        //    /**
        //     * An internal argument parsing function.
        //     * @private
        //     */
        //        function parseArgs(args: IArguments ): IParseResult {
        //    let constant = 0.0;
        //        let factory = () => 0.0;
        //        let terms = createMap<Variable, number>();
        //    for (let i = 0, n = args.length; i<n; ++i ) {
        //        let item = args[i];
        //        if ( typeof item === "number" ) {
        //            constant += item;
        //        } else if (item instanceof Variable ) {
        //            terms.setDefault(item, factory).second += 1.0;
        //        } else if (item instanceof Expression) {
        //    constant += item.constant();
        //    let terms2 = item.terms();
        //    for (let j = 0, k = terms2.size(); j < k; j++)
        //    {
        //        let termPair = terms2.itemAt(j);
        //        terms.setDefault(termPair.first, factory).second += termPair.second;
        //    }
        //} else if (item instanceof Array ) {
        //    if (item.length !== 2)
        //    {
        //        throw new Error("array must have length 2");
        //    }
        //    let value: number = item[0];
        //    let value2 = item[1];
        //    if (typeof value !== "number")
        //    {
        //        throw new Error("array item 0 must be a number");
        //    }
        //    if (value2 instanceof Variable) {
        //        terms.setDefault(value2, factory).second += value;
        //    } else if (value2 instanceof Expression) {
        //        constant += (value2.constant() * value);
        //        let terms2 = value2.terms();
        //        for (let j = 0, k = terms2.size(); j < k; j++)
        //        {
        //            let termPair = terms2.itemAt(j);
        //            terms.setDefault(termPair.first, factory).second += (termPair.second * value);
        //        }
        //    } else
        //    {
        //        throw new Error("array item 1 must be a variable or expression");
        //    }
        //} else
        //{
        //    throw new Error("invalid Expression argument: " + item);
        //}
        //    }
        //    return { terms, constant };
        //}


        /// <summary>
        /// constant
        /// </summary>
        /// <param name="variable"></param>
        public Expression(double constant)
        {
            //处理constant
            Constant += constant;
        }

        /// <summary>
        /// variable
        /// </summary>
        /// <param name="variable"></param>
        public Expression(Variable variable)
        {
            //处理Variable
            terms.setDefault(variable, ()=> 0.0).Value += 1.0;
        }

        /// <summary>
        /// multiple*variable+constant
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="constant"></param>
        public Expression((double, Variable) expression, double constant)
        {
            //解析数组(ts中为数组)
            var value = expression.Item1;
            var value2 = expression.Item2;
            terms.setDefault(value2, () => 0.0).Value += value;
            //处理constant
            Constant += constant;
        }

        /// <summary>
        /// expression+multiple*expresion
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="expression1"></param>
        public Expression(Expression expression, (double, Expression) expression1)
        {
            //处理表达式
            Constant += expression.Constant;
            var terms1 = expression.terms;
            var k = terms1.size();
            for (var j = 0; j < k; j++)
            {
                var termPair = terms1.itemAt(j);
                terms.setDefault(termPair.Key, () => 0.0).Value += termPair.Value;
            }
            //处理数组
            var multiple = expression1.Item1;
            var e = expression1.Item2;
            Constant += (e.Constant * multiple);
            var terms2 = e.terms;
            var count = terms2.size();
            for (var j = 0; j < count; j++)
            {
                var termPair = terms2.itemAt(j);
                terms.setDefault(termPair.Key, () => 0.0).Value += (termPair.Value * multiple);
            }
        }

        /// <summary>
        /// multiple*variable+variable
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variable"></param>
        public Expression((double,Variable) expression, Variable variable)
        {
            //解析数组(ts中为数组)
            var value = expression.Item1;
            var value2 = expression.Item2;
            terms.setDefault(value2, ()=>0.0).Value += value;
            //解析Variable
            terms.setDefault(variable, () => 0.0).Value += 1.0;
        }

        /// <summary>
        /// multiple*variable+variable+variable
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variable"></param>
        public Expression((double, Variable) expression, Variable variable, Variable variable1)
        {
            //解析数组(ts中为数组)
            var value = expression.Item1;
            var value2 = expression.Item2;
            terms.setDefault(value2, () => 0.0).Value += value;
            //解析Variable
            terms.setDefault(variable, () => 0.0).Value += 1.0;
            terms.setDefault(variable1, () => 0.0).Value += 1.0;
        }

        /// <summary>
        /// multiple*variable+multiple*variable
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variable"></param>
        public Expression((double, Variable) expression, (double, Variable) expression1)
        {
            //解析数组(ts中为数组)
            var value = expression.Item1;
            var value2 = expression.Item2;
            terms.setDefault(value2, () => 0.0).Value += value;

            terms.setDefault(expression1.Item2, () => 0.0).Value += expression1.Item1;
        }

        #endregion

        /**
         * Returns the computed value of the expression.
         *
         * @private
         * @return {Number} computed value of the expression
         */
        public double Value()
        {
            var result = this.Constant;
            var n = this.terms.size();
            for (var i = 0; i < n; i++)
            {
                var pair = this.terms.itemAt(i);
                result += pair.Key.Value * pair.Value;
            }
            return result;
        }



        public bool IsConstant()
        {
            return terms.size() == 0;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    /**
     * An internal interface for the argument parse results.
     */
    public class IParseResult
    {
        IMap<Variable, int> terms;
        int Constraint;
    }
}
