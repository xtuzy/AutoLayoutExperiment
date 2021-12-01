using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kiwi
{
    public partial class Expression
    {
        public double Constant { get; }
        public Term[] Terms { get; }

        public Expression(Term t, double constant = 0.0)
        {
            Terms = new[] { t };
            Constant = constant;
        }

        private Expression(Term[] ts, double constant)
        {
            Terms = ts;
            Constant = constant;
        }

        public class Builder
        {
            public Builder Add(Term term)
            {
                return this;
            }

            public Builder Add(double exprConstant)
            {
                return this;
            }
        }


        // TODO: optimize Reduce()?
        [Pure]
        public Expression Reduce()
        {
            var vars = new Dictionary<Variable, double>();
            foreach (var term in Terms)
            {
                vars.TryGetValue(term.Variable, out var coefficient);
                vars[term.Variable] = coefficient + term.Coefficient;
            }

            var reducedTerms = new Term[vars.Count];
            int i = 0;
            foreach (var entry in vars)
            {
                var variable = entry.Key;
                var coefficient = entry.Value;
                reducedTerms[i] = new Term(variable, coefficient);
                i++;
            }

            return new Expression(reducedTerms, Constant);
        }




        /*
        public Expression(double constant = 0.0)
        {
            Terms = new List<Term>();
            Constant = constant;
        }

        public Expression(Term term, double constant = 0.0)
        {
            Terms = new List<Term> {term};
            Constant = constant;
        }

        public Expression(List<Term> terms, double constant = 0.0)
        {
            Terms = terms; // TODO this looks weird
            Constant = constant;
        }

        public List<Term> Terms { get; }

        public double Constant { get; }
        //*/




        // TODO should be converter to method, as getter is not trivial
        public double Value
        {
            get
            {
                var result = Constant;
                foreach (var term in Terms)
                {
                    result += term.Value;
                }
                return result;
            }
        }
    }
}