using System;

namespace Kiwi
{
    public class UnsatisfiableConstraint : Exception
    {
        public UnsatisfiableConstraint(Constraint constraint)
        {
            Constraint = constraint;
        }

        public Constraint Constraint { get; }
        public override string Message => "The constraint can not be satisfied.";
    }


    public class UnknownConstraint : Exception
    {
        public UnknownConstraint(Constraint constraint)
        {
            Constraint = constraint;
        }

        public Constraint Constraint { get; }
        public override string Message => "The constraint has not been added to the solver.";
    }


    public class DuplicateConstraint : Exception
    {
        public DuplicateConstraint(Constraint constraint)
        {
            Constraint = constraint;
        }

        public Constraint Constraint { get; }
        public override string Message => "The constraint has already been added to the solver.";
    }


    public class UnknownEditVariable : Exception
    {
        public UnknownEditVariable(Variable variable)
        {
            Variable = variable;
        }

        public Variable Variable { get; }

        public override string Message => "The edit variable has not been added to the solver.";
    }


    public class DuplicateEditVariable : Exception
    {
        public DuplicateEditVariable(Variable variable)
        {
            Variable = variable;
        }

        public Variable Variable { get; }
        public override string Message => "The edit variable has already been added to the solver.";
    }


    public class BadRequiredStrength : Exception
    {
        public override string Message => "A required strength cannot be used in this context.";
    }


    public class InternalSolverError : Exception
    {
        public InternalSolverError()
        {
            Message = "An internal solver error ocurred.";
        }

        public InternalSolverError(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}