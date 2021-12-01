using Xunit;

namespace Kiwi.Tests
{
    public class TestConstraint
    {
        [Fact]
        public void test_constraint_creation()
        {
            // Test constraints creation and methods.

            var v = new Variable("foo");
            var c = new Constraint(v + 1, RelationalOperator.OP_EQ);

            Assert.Equal(Strength.Required, c.Strength);
            Assert.Equal(RelationalOperator.OP_EQ, c.Op);
            Assert.Equal(1, c.Expression.Constant);
            Assert.Collection(c.Expression.Terms, term =>
            {
                Assert.Equal(v, term.Variable);
                Assert.Equal(1, term.Coefficient);
            });

            // TODO    assert str(c) == "1 * foo + 1 == 0 | strength = 1.001e+09"

            foreach (var strength in new[]
            {
                Strength.Weak,
                Strength.Medium,
                Strength.Strong,
                Strength.Required
            })
            {
                c = new Constraint(v + 1, RelationalOperator.OP_GE, strength);
                Assert.Equal(strength, c.Strength);
            }
        }

        [Fact]
        public void test_constraint_or_operator()
        {
            // Test modifying a constraint strength using the | operator.

            var v = new Variable("foo");
            var c = new Constraint(v + 1, RelationalOperator.OP_EQ);

            foreach (var strength in new[]
            {
                Strength.Weak,
                Strength.Medium,
                Strength.Strong,
                Strength.Required
            })
            {
                var c2 = c | strength;
                Assert.Equal(strength, c2.Strength);
            }
        }
    }
}