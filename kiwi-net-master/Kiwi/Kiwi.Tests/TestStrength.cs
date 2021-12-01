using Xunit;

namespace Kiwi.Tests
{
    public class TestStrength
    {
        [Fact]
        public void test_accessing_predefined_strength()
        {
            // Test getting the default values for the strength.
            Assert.True(Strength.Weak < Strength.Medium);
            Assert.True(Strength.Medium < Strength.Strong);
            Assert.True(Strength.Strong < Strength.Required);
        }


        [Fact]
        public void test_creating_strength()
        {
            // Test creating strength from constitutent values.
            Assert.True(Strength.Create(0, 0, 1) < Strength.Create(0, 1, 0));
            Assert.True(Strength.Create(0, 1, 0) < Strength.Create(1, 0, 0));
            Assert.True(Strength.Create(1, 0, 0, 1) < Strength.Create(1, 0, 0, 4));
        }
    }
}
