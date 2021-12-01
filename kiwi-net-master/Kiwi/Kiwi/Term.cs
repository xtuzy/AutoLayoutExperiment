namespace Kiwi
{
    public partial class Term
    {
        public Term(Variable variable, double coefficient = 1.0)
        {
            Variable = variable;
            Coefficient = coefficient;
        }

        public Variable Variable { get; }
        public double Coefficient { get; }
        public double Value => Coefficient * Variable.Value;
    }
}