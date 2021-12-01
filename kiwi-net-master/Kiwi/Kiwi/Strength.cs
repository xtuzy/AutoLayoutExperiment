using static System.Math;

namespace Kiwi
{
    public class Strength
    {
        public static readonly double Required = Create(1000.0, 1000.0, 1000.0);
        public static readonly double Strong = Create(1.0, 0.0, 0.0);
        public static readonly double Medium = Create(0.0, 1.0, 0.0);
        public static readonly double Weak = Create(0.0, 0.0, 1.0);

        public static double Create(double a, double b, double c, double w = 1.0)
        {
            var result = 0.0;
            result += Max(0.0, Min(1000.0, a * w)) * 1000000.0;
            result += Max(0.0, Min(1000.0, b * w)) * 1000.0;
            result += Max(0.0, Min(1000.0, c * w));
            return result;
        }

        public static double Clip(double value)
        {
            return Max(0.0, Min(Required, value));
        }
    }

//    public struct Strength2
//    {
//        public static readonly Strength2 Required = new Strength2(1000.0, 1000.0, 1000.0);
//        public static readonly Strength2 Strong = new Strength2(1.0, 0.0, 0.0);
//        public static readonly Strength2 Medium = new Strength2(0.0, 1.0, 0.0);
//        public static readonly Strength2 Weak = new Strength2(0.0, 0.0, 1.0);
//
//        private readonly double _value;
//
//        public Strength2(double a, double b, double c, double w = 1.0)
//        {
//            _value = Max(0.0, Min(1000.0, a * w)) * 1000000.0 +
//                     Max(0.0, Min(1000.0, b * w)) * 1000.0 +
//                     Max(0.0, Min(1000.0, c * w));
//        }
//
//        public static double Clip(double value)
//        {
//            return Clamp(value, 0, Required._value);
//            return Max(0.0, Min(Required._value, value));
//        }
//
//        private static double Clamp(double value, double min, double max)
//        {
//            return Max(min, Min(max, value));
//        }
//    }
}