
using static System.Math;
namespace Kiwi_Ts
{
    public class Strength
    {
        public static readonly double Required = 1000;//Create(1000.0, 1000.0, 1000.0);
        public static readonly double Strong = 750;// Create(1.0, 0.0, 0.0);
        public static readonly double Medium = 250;// Create(0.0, 1.0, 0.0);
        public static readonly double Weak = 10;// Create(0.0, 0.0, 1.0);

        /*public static double Create(double a, double b, double c, double w = 1.0)
        {
            var result = 0.0;
            result += Max(0.0, Min(1000.0, a * w)) * 1000000.0;
            result += Max(0.0, Min(1000.0, b * w)) * 1000.0;
            result += Max(0.0, Min(1000.0, c * w));
            return result;
        }
*/

        public static double Create(double a)
        {
            return a;
        }
        public static double Clip(double strength)
        {
            return Max(0.0, Min(Required, strength));
        }
    }
}
